using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyCombat))]
[RequireComponent(typeof(EnemyVFX))]
public class Enemy : Entity
{
    // Fields
    //[SerializeField] private State currentState; // Current State of Enemy
    private EnemyStateMachine _stateMachine; // State Controller of Enemy
    public bool isBossEnemy; // Is Enemy a Boss or Not
    
    private EnemyMovement _movement;
    private EnemyCombat _combat;
    private EnemyVFX _enemyVfx;
    [SerializeField] private Ability _ability;
    
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Room _currentRoom;
    
    public Transform target; // Enemy target (Player)
    public bool canReap = true; // Enemy can still be Reaped if maxHealth drops to below 25%
    public bool waitingForReap = false; // Enemy waits for 10 seconds giving Player chance to Reap
    public bool isDead = false; // Enemy has been killed

    [Header("Events")]
    [SerializeField] private GameEvent deathEvent;
    [SerializeField] private GameEvent reapedEvent;
    [SerializeField] private GameEvent spellEvent;
    [SerializeField] private GameEvent hurtEvent;
    
    [SerializeField] private float pushBackForce = 15.0f;
    private bool raisedReapEvent = false;

    // Properties
    public GameEvent DeathEvent => deathEvent;
    public GameEvent ReapedEvent => reapedEvent;
    public GameEvent SpellEvent => spellEvent;
    public GameEvent HurtEvent => hurtEvent;
    public EnemyMovement Movement => _movement;
    public EnemyCombat Combat => _combat;
    public EnemyVFX VFX => _enemyVfx;
    public EnemyStateMachine StateMachine => _stateMachine;
    public Room CurrentRoom
    {
        get => _currentRoom;
        set {if (!_currentRoom) _currentRoom = value;}
    }
    // Current Level of Game
    public int currentLevel = 1;
    [SerializeField] private int levelBoost; // Boost Enemies get to stats from current level

        // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _movement = GetComponent<EnemyMovement>();
        _combat = GetComponent<EnemyCombat>();
        _enemyVfx = GetComponent<EnemyVFX>();
        GetPlayer();
        _stateMachine = new EnemyStateMachine(this);
        // Get currentLevel
        // Update enemy stats from level
        CurrentAttack = CurrentAttack + (levelBoost * currentLevel);
        CurrentAttackSpeed = CurrentAttackSpeed + (levelBoost * currentLevel);
        CurrentDefense = CurrentDefense - ((levelBoost * currentLevel) / 20);
        CurrentSpeed = CurrentSpeed + (levelBoost * currentLevel);
    }


    // Why is this in fixed update? --> it handles physics as well as logic. Can be separate
    void FixedUpdate()
    {
        _stateMachine.Tick();
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position,Vector3.up,entityStats.meleeRange);
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position,Vector3.up, entityStats.detectionRange);
    }


    public void GetPlayer()
    {
        // Else find GameObject with Player tag
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        // If Player not in scene, target is null
        else
        {
            target = null;
        }
    }
    

    // Override TakeDamage for when Enemy is in near invincible state
    public override void TakeDamage(int amount)
    {
        hurtEvent.Raise();
        // If isInvincible is true, reduce amount by damageReduction
        if (_combat.isInvincible)
        {
            base.TakeDamage((int)(amount / _combat.damageReduction));
        }
        // Else, take damage normally
        else
        {
            base.TakeDamage(amount);
        }
        _stateMachine.SetState(_stateMachine.StunState.SetDuration(.1f));
    }

    public void CastAbility()
    {
        _ability.SoulAbility(target.position, transform.forward,null, _rigidbody);
        /*StartCoroutine(AbilityCo(_ability.duration));*/
    }
    public IEnumerator AbilityCo(float abilityDuration)
    {
        // Change state
        //...
        _ability.SoulAbility(target.position, transform.forward,null, _rigidbody);
        yield return new WaitForSeconds(abilityDuration);
        // Change back state
        // ...
    }
    
    // ReapEnemyTimer starts ReapTimer so Player has 10 seconds to reap Enemy
    public void ReapEnemyTimer()
    {
        // Start ReapTimer Coroutine
        StartCoroutine(ReapWindowTimer());
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

    public void Reap()
    {
        
        TakeDamage(100);
        gameObject.AddComponent<ReapLevitation>(); 
        _stateMachine.SetState(_stateMachine.ReapState);
        StartCoroutine(KillAfterSecondsCo(5f));
    }

    public override void Die()
    {
        deathEvent.Raise();
        isDead = true;
        _rigidbody.constraints = RigidbodyConstraints.None;
        Movement._navMeshAgent.enabled = false;
        _rigidbody.AddForce(-(pushBackForce) * .5f * transform.forward, ForceMode.Impulse);
        _rigidbody.velocity = Vector3.zero;
        if(CurrentRoom)
        {
            /*if (!isDead) */
            CurrentRoom.CurrentEnemyCount = CurrentRoom.CurrentEnemyCount - 1;
        }
        Destroy(gameObject,3);
    }
    
    
    private IEnumerator KillAfterSecondsCo(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Die();
    }
    
    // Make Enemy wait 10 seconds in Reaped State
    public IEnumerator ReapWindowTimer()
    {
        // After 10 seconds, Enemy can no longer be reaped and returns to previous state
        yield return new WaitForSeconds(entityStats.reapTime);
        canReap = false;
        waitingForReap = false;
        Movement._navMeshAgent.enabled = true;
    }
}
