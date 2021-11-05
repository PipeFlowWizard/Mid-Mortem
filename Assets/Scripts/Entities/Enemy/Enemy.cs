using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyCombat))]
public class Enemy : Entity
{
    private State currentState; // Current State of Enemy

    [SerializeField] private EnemyMovement _movement;
    [SerializeField] private EnemyCombat _combat;
    public EnemyMovement Movement => _movement;
    public EnemyCombat Combat => _combat;

    public Room CurrentRoom
    {
        get => _currentRoom;
        set
        { 
            if(!_currentRoom) _currentRoom = value;
        }
    }

    // Enemy Movement
    private Rigidbody _rigidbody;                           // Reference to RigidBody of Enemy

    // Reference to Player
    public Transform target;                                // Enemy target (Player)

    // Enemy Attack
    [SerializeField] private float pushBackForce = 15.0f;   // Push Enemy back after attacking Player with melee
    public bool meleeAttack = true;                                // Bool to determine when to Melee attack 
    
    // Enemy Material
    private Material _material;                             // Reference to Enemy Material
    private Color enemyColor;                               // Original Enemy Color
    private bool flash;                                     // Bool to determine when to change fadeAmount

    // Reap quantities of Enemy
    public bool canReap = true;                                    // Enemy is now weakend enough and can be reaped

    // Tags for possible Enemy interaction

    [Header("Events")]
    public GameEvent deathEvent;
    public GameEvent reapedEvent;

    private Room _currentRoom;

    public bool isDead = false;

    // TODO: Separate into different components based on regions
    
    
    #region UnityCallbacks


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
        _material = GetComponent<MeshRenderer>().material;
        enemyColor = _material.color;
        // Set current state to IDLE
        SetState(new IdleEnemyState(this));
    }


    // Why is this in fixed update?
    void FixedUpdate()
    {
        // Get Health of Enemy to determine color
        SetEnemyHealthState();
        // Call Action() for currentState 
        if (target)
        {
            currentState.Action();
        }
    }

    
    
    private void OnCollisionEnter(Collision col)
    {
        //Movement
        
        // If collide with a Player, they take damage and then they move back
        if (col.transform.CompareTag("Player"))
        {
            meleeAttack = false;
            _rigidbody.AddForce(-pushBackForce * transform.forward, ForceMode.Impulse);
        }
        // If collide with another Enemy, then move to left or right
        if (col.transform.CompareTag("Enemy"))
        {
            int index = Random.Range(1, 3);
            if (index == 1)
            {
                // Move right
                _rigidbody.AddForce(pushBackForce * transform.right, ForceMode.Impulse);
            }
            else
            {
                // Move left
                _rigidbody.AddForce(-pushBackForce * transform.right, ForceMode.Impulse);
            }
        }
    }

    
    
    #endregion
    
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

    #region  State Machine

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


    private void SetEnemyHealthState()
    {
        // If health is less than 50 but greater than 25, the Enemy turns yellow
        // Call CurrentHealthState
        if (CurrentHealthState() == 1)
        {
            if (flash)
            {
                _material.color = enemyColor;
            }
            else
            {
                Color yellow = Color.yellow;
                yellow.a = 0.5f;
                _material.color = yellow;
            }
        }
        // If health is less than 25, Enemy turns red
        else if (CurrentHealthState() == 2)
        {
            if (flash)
            {
                _material.color = enemyColor;
            }
            else
            {
                Color red = Color.red;
                red.a = 0.5f;
                _material.color = red;
            }
        }
    }

    #endregion
}

// General Enemy class for Enemies