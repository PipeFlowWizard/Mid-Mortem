using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyCombat : MonoBehaviour
{
    private Enemy _enemy;
    public bool rangeAttack = true;
    public float projectileSpeed = 5;

    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private GameObject enemySpell;         // Reference to EnemySpell GameObject
    [SerializeField] private float tripleAttackOffset;      // Offset for TripleRangedAttack between spells
    [SerializeField] private float xAttackSpeed;            // Speed for Special XAttack of boss

    [SerializeField] private int invincibleTime;            // Time the Enemy is takes less damage than normal
    public bool isInvincible;                               // Enemy takes less damage than normal using ability
    [SerializeField] private float invincibleSize;          // Enemy gets slightly larger when taking less damage
    public float damageReduction;                           // Attack is reduced when in near invincible state

    public GameObject meteorFallObject;                     // GameObject for MeteorFall attackDamage
    public GameObject heatSeekerObject;                     // GameObject for HeatSeeker attackDamage

    [SerializeField] private float rotationDamp = 0.5f;
    
    

    public Animator anim;
    
    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public IEnumerator RangeAttackTimer()
    {
        // Every 3 seconds set launch to true
        yield return new WaitForSeconds(_enemy.entityStats.rangedAttackSpeed);
        rangeAttack = true;   
    }

   

    // Make Enemy Invincible for 3 seconds
    public IEnumerator InvincibleTimer()
    {
        yield return new WaitForSeconds(invincibleTime);
        // Set isInvincible to false and Enemy back to original size    
        isInvincible = false;
        transform.localScale = Vector3.one;
    }
    
    
    // KillEnemy knocks the enemy down and Stops all Coroutines and sets attackDamage to false
    //TODO: move to Enemy.cs
   
    
    public void RangedAttack()
    {
        // Enemy can only attackDamage if it is Not Dead and if it is Not Waiting For Reap
        if (!_enemy.isDead && !_enemy.waitingForReap)
        {
            // Offset is transform.forward
            GameObject spellObj = Instantiate(enemySpell, transform.position + transform.forward + transform.up, Quaternion.identity);
            EnemySpell spell = spellObj.GetComponent<EnemySpell>();
            // Set direction and speed of spell

            spell.FireSpell(transform.forward, projectileSpeed, _enemy.entityStats.attackDamage);
            // Start Timer to wait for next Ranged Attack
            StartCoroutine(RangeAttackTimer());
        }
    }

    // Special RangedAttack that fires three projectiles at once
    public void TripleRangedAttack()
    {
        // Enemy can only attackDamage if it is Not Dead and if it is Not Waiting For Reap
        if (!_enemy.isDead && !_enemy.waitingForReap)
        {
            // Offset is transform.forward
            // Fire an attackDamage in front of Enemy
            GameObject spellObj = Instantiate(enemySpell, transform.position + transform.forward + transform.up, Quaternion.identity);
            EnemySpell spell = spellObj.GetComponent<EnemySpell>();
            // Fire attackDamage slightly to the right of Enemy
            GameObject spellObj2 = Instantiate(enemySpell, transform.position + transform.forward + (transform.right * tripleAttackOffset) + transform.up, Quaternion.identity);
            EnemySpell spell2 = spellObj2.GetComponent<EnemySpell>();
            // Fire attackDamage slightly to the left of Enemy
            GameObject spellObj3 = Instantiate(enemySpell, transform.position + transform.forward - (transform.right * tripleAttackOffset) + transform.up, Quaternion.identity);
            EnemySpell spell3 = spellObj3.GetComponent<EnemySpell>();
            // Set direction and speed of spell
            spell.FireSpell(transform.forward, projectileSpeed, _enemy.entityStats.attackDamage);
            spell2.FireSpell(transform.forward,projectileSpeed, _enemy.entityStats.attackDamage);
            spell3.FireSpell(transform.forward, projectileSpeed, _enemy.entityStats.attackDamage);
            // Start Timer to wait for next Ranged Attack
            StartCoroutine(RangeAttackTimer());
        }
    }

    // Set Enemy to be invincible and 1.5 times it's regular size for 3 seconds
    public void Invincible()
    {
        // Set isInvincible to true
        isInvincible = true;
        print("Invincible");
        // Make Enemy 1.5 times bigger
        transform.localScale = Vector3.one * invincibleSize;
        StartCoroutine(InvincibleTimer());
    }

    // MeteorFall Instantiates a Meteor object and drops it above Player
    public void MeteorFall()
    {
        GameObject meteor = Instantiate(meteorFallObject, _enemy.target.position, Quaternion.identity);
        MeteorFallAttack meteorAttack = meteor.GetComponent<MeteorFallAttack>();
        // Start Timer to wait for next Ranged Attack
        StartCoroutine(RangeAttackTimer());
        meteorAttack.MeteorCrash();
    }

    // HeatSeeker Instantiates a HeatSeeker object that chases after Player
    public void HeatSeeker()
    {
        GameObject heatSeeker = Instantiate(heatSeekerObject, transform.position + (4 * transform.forward) + transform.up, Quaternion.identity);
        // Start Timer to wait for next Ranged Attack
        StartCoroutine(RangeAttackTimer());
    }

    // XAttack is a special ranged attackDamage that fires multiple ranged spells in the form of an X
    public void XAttack()
    {
        // Offset is transform.forward
        // Fire an attackDamage in front of Enemy
        GameObject spellObj = Instantiate(enemySpell, transform.position + transform.forward + (2 * transform.up), Quaternion.identity);
        EnemySpell spell = spellObj.GetComponent<EnemySpell>();
        // Fire attackDamage slightly to the right and above Enemy
        GameObject spellObj2 = Instantiate(enemySpell, transform.position + transform.forward + (transform.right * tripleAttackOffset) + (3 * transform.up), Quaternion.identity);
        EnemySpell spell2 = spellObj2.GetComponent<EnemySpell>();
        // Fire attackDamage slightly to the left and above Enemy
        GameObject spellObj3 = Instantiate(enemySpell, transform.position + transform.forward - (transform.right * tripleAttackOffset) + (3 * transform.up), Quaternion.identity);
        EnemySpell spell3 = spellObj3.GetComponent<EnemySpell>();
        // Fire attackDamage slightly to the right and below Enemy
        GameObject spellObj4 = Instantiate(enemySpell, transform.position + transform.forward + (transform.right * tripleAttackOffset) + transform.up, Quaternion.identity);
        EnemySpell spell4 = spellObj4.GetComponent<EnemySpell>();
        // Fire attackDamage slightly to the left and below Enemy
        GameObject spellObj5 = Instantiate(enemySpell, transform.position + transform.forward - (transform.right * tripleAttackOffset) + transform.up, Quaternion.identity);
        EnemySpell spell5 = spellObj5.GetComponent<EnemySpell>();
        // Set direction and speed of spell
        spell.FireSpell(transform.forward, xAttackSpeed *  projectileSpeed, _enemy.entityStats.attackDamage);
        spell2.FireSpell(transform.forward, xAttackSpeed * projectileSpeed, _enemy.entityStats.attackDamage);
        spell3.FireSpell(transform.forward, xAttackSpeed * projectileSpeed, _enemy.entityStats.attackDamage);
        spell4.FireSpell(transform.forward, xAttackSpeed * projectileSpeed, _enemy.entityStats.attackDamage);
        spell5.FireSpell(transform.forward, xAttackSpeed * projectileSpeed, _enemy.entityStats.attackDamage);
        // Start Timer to wait for next Ranged Attack
        StartCoroutine(RangeAttackTimer());
    }
    
    public void MeleeAttack()
    {
        // TODO: enemy melee animation and hurtbox
        // Debug.Log("Melee Attempt");
        anim.Play("EnemyMelee");
    }
}
