using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyCombat))]
public class Enemy : Entity
{
    private State currentState; // Current State of Enemy

    private EnemyMovement _movement;
    private EnemyCombat _combat;
    [SerializeField] IdleEnemyState _idleEnemyState;

    public EnemyMovement Movement => _movement;
    public EnemyCombat Combat => _combat;

    public Room CurrentRoom
    {
        get => _currentRoom;
        set
        {
            if (!_currentRoom) _currentRoom = value;
        }
    }


    private Rigidbody _rigidbody; // Reference to RigidBody of Enemy

    public Transform target; // Enemy target (Player)

    // Bool to determine when to Melee attack 

    // Bool to determine when to change fadeAmount

    public bool canReap = true; // Enemy is now weakend enough and can be reaped

    [Header("Events")] public GameEvent deathEvent;
    public GameEvent reapedEvent;

    private Room _currentRoom;




    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _movement = GetComponent<EnemyMovement>();
        _combat = GetComponent<EnemyCombat>();
        // Get Transform of Player, Target
        GetPlayer();
        // Get reference to Enemy RigidBody

        // Get reference to Enemy Material and color

        // Set current state to IDLE
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







    // GetPlayer returns reference to Player transform

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


    // SetState sets the current State of Enemy


    public void SetState(State state)
    {
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

    // Change Enemy Material based on health --> Combat or Normal?


}
