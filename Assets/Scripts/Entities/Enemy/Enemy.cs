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
    
    private EnemyMovement _movement;
    private EnemyCombat _combat;
    [SerializeField] private IdleEnemyState _idleEnemyState;
    [SerializeField] private Rigidbody _rigidbody;
    private Room _currentRoom;
    
    public Transform target; // Enemy target (Player)
    public bool canReap = true; // Enemy is now weakend enough and can be reaped

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
        SetState(new IdleEnemyState(this));
    }


    // Why is this in fixed update?
    void FixedUpdate()
    {
        // Call Action() for currentState 
        if (target)
        {
            currentState.Action();
        }
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
}
