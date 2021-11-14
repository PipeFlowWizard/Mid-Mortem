using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyCombat))]
public class Enemy : Entity
{
    // Fields
    private State currentState; // Current State of Enemy
    private EnemyStateController stateController; // State Controller of Enemy
    public bool isBossEnemy; // Is Enemy a Boss or Not
    
    private EnemyMovement _movement;
    private EnemyCombat _combat;
    [SerializeField] private IdleEnemyState _idleEnemyState;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] 
    private Room _currentRoom;
    
    public Transform target; // Enemy target (Player)
    public bool canReap = true; // Enemy can still be Reaped if health drops to below 25%
    public bool waitingForReap = false; // Enemy waits for 10 seconds giving Player chance to Reap
    public bool isDead = false; // Enemy has been killed

    [Header("Events")]
    [SerializeField] private GameEvent deathEvent;
    [SerializeField] private GameEvent reapedEvent;

    // Properties
    public GameEvent DeathEvent => deathEvent;
    public GameEvent ReapedEvent => reapedEvent;
    public EnemyMovement Movement => _movement;
    public EnemyCombat Combat => _combat;
    public Room CurrentRoom
    {
        get => _currentRoom;
        set {if (!_currentRoom) _currentRoom = value;}
    }

        // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _movement = GetComponent<EnemyMovement>();
        _combat = GetComponent<EnemyCombat>();
        GetPlayer();
        stateController = new EnemyStateController(this);
    }


    // Why is this in fixed update?
    void FixedUpdate()
    {
        stateController.UpdateEnemyState();
        currentState.Action();
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position,Vector3.up,entityStats.chaseRange);
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position,Vector3.up, entityStats.maxRange);
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
    
    public void SetState(State state)
    {
        //Debug.Log("Changing state");
        // If currentState is already assigned, then call OnStateExit for that State
        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        // Set currentState to new state
        currentState = state;
        // If currentState is now not null, call OnStateEnter for that State
        if (currentState != null)
        {
            currentState.OnStateEnter();
        }
    }

    // Override TakeDamage for when Enemy is in near invincible state
    public override void TakeDamage(int amount)
    {
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
    }
}
