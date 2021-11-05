using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyCombat : MonoBehaviour
{
    private Enemy _enemy;
    public bool rangeAttack = true;
    private bool meleeAttack = true;
    private bool canReap = true;
    public bool waitingForReap;
    private bool isDead = false;

    private Rigidbody _rigidbody;

    [SerializeField] private GameObject enemySpell;         // Reference to EnemySpell GameObject
    [SerializeField] private float rotationDamp = 0.5f;
    [SerializeField] private float pushBackForce = 15.0f;
    
    [Header("Events")]
    public GameEvent deathEvent;
    public GameEvent reapedEvent;
    
    
    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        deathEvent = _enemy.deathEvent;
        reapedEvent = _enemy.reapedEvent;

    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void OnCollisionExit(Collision col)
    {
        //Combat
        
        // After Enemy collides with Player, they stop moving, and Call AttackTimer for 2 seconds
        if (col.transform.CompareTag("Player"))
        {
            // Debug.Log("Player collision");
            var player = col.gameObject.GetComponent<Player>();
            player.TakeDamage(_enemy.entityStats.attack);
            _enemy.Movement.StopEnemy();
            StartCoroutine(_enemy.Combat.MeleeAttackTimer());
        }
        // After Enemy collides with Another Enemy, they stop moving
        if (col.transform.CompareTag("Enemy"))
        {
            _enemy.Movement.StopEnemy();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        // Combat 
        // If Enemy is hit by Player Scythe, they take damage or can be reaped if waiting for Reap
        if (other.CompareTag("PlayerHurtBox"))
        {
            var player = other.GetComponentInParent<Player>();

            // If Enemy is waitingForReap, then they can call the ReapEnemy Function
            // TODO: Add in Reap Animation and adding modifier 
            if (waitingForReap)
            {
                Debug.Log("I T S  R E A P I N'  T I M E");
                reapedEvent.Raise();
                _enemy.Movement.StopEnemy();
                KillEnemy();
            }
            // Else, the Enemy just takes normal damage
            else
            {
                if (player)
                {
                    _enemy.TakeDamage(player.entityStats.attack);
                }

            }
        }

        if (other.CompareTag("PlayerSpell"))
        {
            var player = other.GetComponentInParent<Player>();
            
            if (player)
            {
                _enemy.TakeDamage(player.entityStats.attack);
            }
        }
    }
    
    
    public IEnumerator RangeAttackTimer()
    {
        // Every 3 seconds set launch to true
        yield return new WaitForSeconds(_enemy.entityStats.rangedSpawn);
        rangeAttack = true;
    }


    // Timer between attacks for Enemy

    public IEnumerator MeleeAttackTimer()
    {
        // Every 3 seconds set launch to true
        yield return new WaitForSeconds(_enemy.entityStats.meleeSpawn);
        meleeAttack = true;
    }

    

    // Make Enemy wait 10 seconds in Reaped State
    public IEnumerator ReapTimer()
    {
        // After 10 seconds, Enemy can no longer be reaped and returns to previous state
        yield return new WaitForSeconds(_enemy.entityStats.reapTime);
        canReap = false;
        waitingForReap = false;
    }


    // Destroy Enemy after 3 seconds
    public IEnumerator EnemyKilled()
    {
        
        deathEvent.Raise();
        // After 3 seconds, destroy Enemy Game Object
        yield return new WaitForSeconds(_enemy.entityStats.rangedSpawn);
        
        Destroy(gameObject);
    }

    // ReapEnemyTimer starts ReapTimer so Player has 10 seconds to reap Enemy
    public void ReapEnemyTimer()
    {
        // Start ReapTimer Coroutine
        StartCoroutine(ReapTimer());
    }

    // KillEnemy knocks the enemy down and Stops all Coroutines and sets attack to false
    public void KillEnemy()
    {
        meleeAttack = false;
        rangeAttack = false;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.AddForce(-pushBackForce * rotationDamp * transform.forward, ForceMode.Impulse);
        _rigidbody.velocity = Vector3.zero;
        if(!isDead) _enemy.CurrentRoom.CurrentEnemyCount = _enemy.CurrentRoom.CurrentEnemyCount - 1;
        isDead = true;
        Destroy(gameObject,3);
    }
    
    // ReapEnemy Destroys the Enemy Game Object and returns the type of Enemy and modifier boost

    // Can only get a modifier boost if Enemy is waiting for reap, being in ReapEnemyState
    

    // Used to create Enemy Ranged Attack

    public void RangedAttack()
    {
        // Offset is transform.forward
        // Instantiate Spell
        GameObject spellObj = Instantiate(enemySpell, transform.position + transform.forward, Quaternion.identity);
        // Get Reference to EnemySpell Component
        EnemySpell spell = spellObj.GetComponent<EnemySpell>();
        // Set direction and speed of spell
        spell.FireSpell(transform.forward, _enemy.entityStats.attackSpeed, _enemy.entityStats.attack);
        // Start Timer to wait for next Ranged Attack
        StartCoroutine(RangeAttackTimer());
    }
}
