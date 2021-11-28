using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyCombat : MonoBehaviour
{
    private Enemy _enemy;
    public bool rangeAttack = true;
    private bool raisedReapEvent = false;

    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private GameObject enemySpell;         // Reference to EnemySpell GameObject
    [SerializeField] private float tripleAttackOffset;      // Offset for TripleRangedAttack between spells
    [SerializeField] private float xAttackSpeed;            // Speed for Special XAttack of boss

    [SerializeField] private int invincibleTime;            // Time the Enemy is takes less damage than normal
    public bool isInvincible;                               // Enemy takes less damage than normal using ability
    [SerializeField] private float invincibleSize;          // Enemy gets slightly larger when taking less damage
    public float damageReduction;                           // Attack is reduced when in near invincible state

    public GameObject meteorFallObject;                     // GameObject for MeteorFall attack
    public GameObject heatSeekerObject;                     // GameObject for HeatSeeker attack

    [SerializeField] private float rotationDamp = 0.5f;
    [SerializeField] private float pushBackForce = 15.0f;
    
    [Header("Events")]
    public GameEvent deathEvent;
    public GameEvent reapedEvent;

    public Animator anim;
    
    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        deathEvent = _enemy.DeathEvent;
        reapedEvent = _enemy.ReapedEvent;
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public IEnumerator RangeAttackTimer()
    {
        // Every 3 seconds set launch to true
        yield return new WaitForSeconds(_enemy.entityStats.rangedSpawn);
        rangeAttack = true;   
    }

    // Make Enemy wait 10 seconds in Reaped State
    public IEnumerator ReapTimer()
    {
        // After 10 seconds, Enemy can no longer be reaped and returns to previous state
        yield return new WaitForSeconds(_enemy.entityStats.reapTime);
        _enemy.canReap = false;
        _enemy.waitingForReap = false;
        _enemy.Movement._navMeshAgent.enabled = true;
    }

    // Make Enemy Invincible for 3 seconds
    public IEnumerator InvincibleTimer()
    {
        yield return new WaitForSeconds(invincibleTime);
        // Set isInvincible to false and Enemy back to original size    
        isInvincible = false;
        transform.localScale = Vector3.one;
    }


    // ReapEnemyTimer starts ReapTimer so Player has 10 seconds to reap Enemy
    public void ReapEnemyTimer()
    {
        // Start ReapTimer Coroutine
        StartCoroutine(ReapTimer());
    }

    // Avoir raising event multiple times
    public void RaiseReapEvent()
    {
        if (raisedReapEvent) return;

        Debug.Log("I T S  R E A P I N'  T I M E");
        raisedReapEvent = true;
        reapedEvent.Raise();
    }

    // Kill after seconds, used to delay death in reaping
    public void KillAfterSeconds(float seconds)
    {
        StartCoroutine(KillAfterSecondsCo(seconds));
    }
    
    private IEnumerator KillAfterSecondsCo(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        KillEnemy();
    }
    
    // KillEnemy knocks the enemy down and Stops all Coroutines and sets attack to false
    //TODO: move to Enemy.cs
    public void KillEnemy()
    {
        deathEvent.Raise();
        _enemy.isDead = true;
        rangeAttack = false;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _enemy.Movement._navMeshAgent.enabled = false;
        _rigidbody.AddForce(-(pushBackForce) * rotationDamp * transform.forward, ForceMode.Impulse);
        _rigidbody.velocity = Vector3.zero;
        if(_enemy.CurrentRoom)
        {
            if (!_enemy.isDead) _enemy.CurrentRoom.CurrentEnemyCount = _enemy.CurrentRoom.CurrentEnemyCount - 1;
        }
        Destroy(gameObject,3);
    }
    
    public void RangedAttack()
    {
        // Enemy can only attack if it is Not Dead and if it is Not Waiting For Reap
        if (!_enemy.isDead && !_enemy.waitingForReap)
        {
            // Offset is transform.forward
            GameObject spellObj = Instantiate(enemySpell, transform.position + transform.forward + transform.up, Quaternion.identity);
            EnemySpell spell = spellObj.GetComponent<EnemySpell>();
            // Set direction and speed of spell

            spell.FireSpell(transform.forward, _enemy.entityStats.attackSpeed, _enemy.entityStats.attack);
            // Start Timer to wait for next Ranged Attack
            StartCoroutine(RangeAttackTimer());
        }
    }

    // Special RangedAttack that fires three projectiles at once
    public void TripleRangedAttack()
    {
        // Enemy can only attack if it is Not Dead and if it is Not Waiting For Reap
        if (!_enemy.isDead && !_enemy.waitingForReap)
        {
            // Offset is transform.forward
            // Fire an attack in front of Enemy
            GameObject spellObj = Instantiate(enemySpell, transform.position + transform.forward + transform.up, Quaternion.identity);
            EnemySpell spell = spellObj.GetComponent<EnemySpell>();
            // Fire attack slightly to the right of Enemy
            GameObject spellObj2 = Instantiate(enemySpell, transform.position + transform.forward + (transform.right * tripleAttackOffset) + transform.up, Quaternion.identity);
            EnemySpell spell2 = spellObj2.GetComponent<EnemySpell>();
            // Fire attack slightly to the left of Enemy
            GameObject spellObj3 = Instantiate(enemySpell, transform.position + transform.forward - (transform.right * tripleAttackOffset) + transform.up, Quaternion.identity);
            EnemySpell spell3 = spellObj3.GetComponent<EnemySpell>();
            // Set direction and speed of spell
            spell.FireSpell(transform.forward, _enemy.entityStats.attackSpeed, _enemy.entityStats.attack);
            spell2.FireSpell(transform.forward, _enemy.entityStats.attackSpeed, _enemy.entityStats.attack);
            spell3.FireSpell(transform.forward, _enemy.entityStats.attackSpeed, _enemy.entityStats.attack);
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

    // XAttack is a special ranged attack that fires multiple ranged spells in the form of an X
    public void XAttack()
    {
        // Offset is transform.forward
        // Fire an attack in front of Enemy
        GameObject spellObj = Instantiate(enemySpell, transform.position + transform.forward + (2 * transform.up), Quaternion.identity);
        EnemySpell spell = spellObj.GetComponent<EnemySpell>();
        // Fire attack slightly to the right and above Enemy
        GameObject spellObj2 = Instantiate(enemySpell, transform.position + transform.forward + (transform.right * tripleAttackOffset) + (3 * transform.up), Quaternion.identity);
        EnemySpell spell2 = spellObj2.GetComponent<EnemySpell>();
        // Fire attack slightly to the left and above Enemy
        GameObject spellObj3 = Instantiate(enemySpell, transform.position + transform.forward - (transform.right * tripleAttackOffset) + (3 * transform.up), Quaternion.identity);
        EnemySpell spell3 = spellObj3.GetComponent<EnemySpell>();
        // Fire attack slightly to the right and below Enemy
        GameObject spellObj4 = Instantiate(enemySpell, transform.position + transform.forward + (transform.right * tripleAttackOffset) + transform.up, Quaternion.identity);
        EnemySpell spell4 = spellObj4.GetComponent<EnemySpell>();
        // Fire attack slightly to the left and below Enemy
        GameObject spellObj5 = Instantiate(enemySpell, transform.position + transform.forward - (transform.right * tripleAttackOffset) + transform.up, Quaternion.identity);
        EnemySpell spell5 = spellObj5.GetComponent<EnemySpell>();
        // Set direction and speed of spell
        spell.FireSpell(transform.forward, xAttackSpeed * _enemy.entityStats.attackSpeed, _enemy.entityStats.attack);
        spell2.FireSpell(transform.forward, xAttackSpeed * _enemy.entityStats.attackSpeed, _enemy.entityStats.attack);
        spell3.FireSpell(transform.forward, xAttackSpeed * _enemy.entityStats.attackSpeed, _enemy.entityStats.attack);
        spell4.FireSpell(transform.forward, xAttackSpeed * _enemy.entityStats.attackSpeed, _enemy.entityStats.attack);
        spell5.FireSpell(transform.forward, xAttackSpeed * _enemy.entityStats.attackSpeed, _enemy.entityStats.attack);
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
