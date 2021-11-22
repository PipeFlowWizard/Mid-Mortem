using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    // Enemy Movement
    private Rigidbody _rigidbody;                           // Reference to RigidBody of Enemy

    public NavMeshAgent _navMeshAgent;                     // Reference to NavMeshAgent
    public LayerMask groundLayer;                          // Reference to Ground LayerMask
    
    //private Vector3 patrolPoint;                            // Point Enemy is moving towards while Patrolling
    public bool patrolPointSet;                             // Determines if Patrol Point is set for Enemy
    public float patrolPointRange;                          // Range in which Enemy patrols around

    
    //public float runAwayDistance;                           // How far away from chaseRange enemy should be
    public bool suicide;                                    // If Enemy is going to perform a suicide run

    public bool isDashing;                                  // When Enemy is dashing after Player
    [SerializeField] private float dashBoost;               // Dash Boost when Enemy is Dashin

    private Enemy _enemy;
    [SerializeField] private float rotationDamp = 0.5f;     // Rotational Dampening so rotation is gradual
    [SerializeField] private float pushBackForce = 15.0f;   // Push Enemy back after attacking Player with melee
    public bool meleeAttack = true;

    public Vector3 desiredLocation;

    public Rigidbody Rigidbody => _rigidbody;

    private void Start()
    {
        desiredLocation = transform.position;
        _enemy = GetComponent<Enemy>();
        _rigidbody = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _enemy.entityStats.speed;
    }

    private void Awake()
    {
       // patrolPointSet = false;
        //runAwayPointSet = false;
        isDashing = false;
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void FixedUpdate()
    {
        
    }

    /*#region Patrol
    // Function for Enemy patrolling
    public void PatrolEnemy()
    {
        // If Patrol Point not set, then search for a patrol point
        if (!patrolPointSet)
        {
            SearchPatrolPoint();
        }

        // If patrolPointSet is now true, then use NavMeshAgent to move towards patrolPoint
        if (patrolPointSet && !_enemy.isDead && !_enemy.waitingForReap)
        {
            TurnEnemy(patrolPoint);
            _navMeshAgent.SetDestination(patrolPoint);
        }
        // Get distance to patrolPoint
        Vector3 distanceToPatrolPoint = transform.position - patrolPoint;
        // Once the patrolPoint is reached, patrolPointSet is false
        if(distanceToPatrolPoint.magnitude < 0.5f)
        {
            patrolPointSet = false;
        }
    }

    // Function to check for a Patrol Point
    public void SearchPatrolPoint()
    {
        // Calculate random patrol point in patrolPointRange
        float randomZ = Random.Range(-patrolPointRange, patrolPointRange);
        float randomX = Random.Range(-patrolPointRange, patrolPointRange);
        // Set new Patrol Point
        patrolPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        // Check that patrolPoint still on ground
        if (Physics.Raycast(patrolPoint + Vector3.up, Vector3.down, 3.0f, groundLayer))
        {
            // If it is, then patrolPointSet is true
            patrolPointSet = true;
        }
    }
    
    #endregion*/

    // Rotate Enemy toward Player

    public void TurnEnemy(Vector3 destination)
    {
        // Enemy can only turn if it is Not Dead and Not Waiting For Reap
        if (!_enemy.isDead)
        {
            // Get vector pointing towards Player
            Vector3 direction = destination - transform.position;
            direction.y = 0;
            // Rotate Enemy is direction is not zero
            if (direction != Vector3.zero)
            {
                // Get Quaternion to rotate towards Player
                Quaternion rotate = Quaternion.LookRotation(direction, Vector3.up);
                // Rotate Enemy, use Slerp to make Rotation gradual
                transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotationDamp);
            }
        }
    }

    // Move Enemy toward Player

    public void MoveEnemy(Vector3 destination)
    {
        // Enemy can only move if it is Not Dead and Not Waiting For Reap
        if (!_enemy.isDead && !_enemy.waitingForReap)
        {
            // Move Enemy toward Player using SetDestination
            _navMeshAgent.SetDestination(destination);
        }
    }
    

    // Enemy makes a suicide dash towards Player
    public void TestDash(Vector3 destination)
    {
        // Check that Player has a direct path to Enemy
        RaycastHit player;
        // If ray hits Player, then Enemy increases in speed to dash
        if (Physics.Raycast(transform.position, transform.forward, out player, Mathf.Infinity) && player.collider.CompareTag("Player"))
        {
            // If Dash just started, set isDashin to true and increase speed
            if (!isDashing)
            {
                // Set isDashing to true
                isDashing = true;
                // Move Enemy toward Player using SetDestination, with dashingBoost
                _navMeshAgent.speed = dashBoost * _enemy.entityStats.speed;
                StartCoroutine(StopDashing());
            }
        }
        // Else, enemy doesn't dash
        else
        {
            isDashing = false;
            _navMeshAgent.speed = _enemy.entityStats.speed;
        }
    }

    // Turns off Enemy Dash Speed after a while if Enemy hasn't collided with Player
    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_enemy.entityStats.rangedSpawn + 2);
        if (isDashing)
        {
            isDashing = false;
            _navMeshAgent.speed = _enemy.entityStats.speed;
        }
    }

    // Stop Enemy Movement
    public void StopEnemy()
    {
        // Set Destination to current position
        _navMeshAgent.enabled = false;
        _rigidbody.velocity = Vector3.zero;
    }
    
    // Wrong place for this type of logic -> this has to do with dealing damage, so it should
    // be done within EnemyCombat or during a combative state, not movement
    // Just like with the player, the enemy hit box should not be damaging the player.
    // Enemies should "put out" a hitbox via a function that can be called by melee states
    /*private void OnCollisionEnter(Collision col)
    {
        //Movement
        
        // If collide with a Player, they take damage and then they move back
        if (col.transform.CompareTag("Player"))
        {
            meleeAttack = false;
            if (isDashing)
            {
                isDashing = false;
                _navMeshAgent.speed = _enemy.entityStats.speed;
            }
            _navMeshAgent.enabled = false;
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
        // Else, if collide with anything besides ground, patrolPointSet is set to false
        else if(!col.transform.CompareTag("Ground"))
        {
            patrolPointSet = false;
        }
    }*/
}
