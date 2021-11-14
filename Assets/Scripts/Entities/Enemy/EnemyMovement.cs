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
    private LayerMask groundLayer;                          // Reference to Ground LayerMask
    
    private Vector3 patrolPoint;                            // Point Enemy is moving towards while Patrolling
    public bool patrolPointSet;                             // Determines if Patrol Point is set for Enemy
    public float patrolPointRange;                          // Range in which Enemy patrols around

    private Vector3 runAwayPoint;                           // Point Enemy is moving towards while running away from Player
    private bool runAwayPointSet;                           // Determines if RunAway point is set
    public float runAwayDistance;                           // How far away from chaseRange enemy should be
    public bool suicide;                                    // If Enemy is going to perform a suicide run

    public bool isDashing;                                  // When Enemy is dashing after Player
    [SerializeField] private float dashBoost;               // Dash Boost when Enemy is Dashin

    public bool isMoving;                                   // Bool to determine if Player is moving
    private Enemy _enemy;
    [SerializeField] private float rotationDamp = 0.5f;     // Rotational Dampening so rotation is gradual
    [SerializeField] private float pushBackForce = 15.0f;   // Push Enemy back after attacking Player with melee
    public bool meleeAttack = true;

    public Rigidbody Rigidbody
    {
        get => _rigidbody;
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _navMeshAgent.speed = _enemy.entityStats.speed;
        //_rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        patrolPointSet = false;
        runAwayPointSet = false;
        isDashing = false;
        groundLayer = LayerMask.GetMask("Ground");
    }

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
            // Set isMoving to true
            isMoving = true;
            // Move Enemy toward Player using SetDestination
            _navMeshAgent.SetDestination(destination);
        }
    }

    // RunAway Moves Enemy away from Player
    public void RunAway()
    {
        // If Run Away Point not set, then search for a RunAwayPoint
        if (!runAwayPointSet)
        {
            SearchRunAwayPoint();
        }

        // If runAwayPointSet is now true, then use NavMeshAgent to move towards runAwayPoint
        if (runAwayPointSet && !_enemy.isDead && !_enemy.waitingForReap)
        {
            TurnEnemy(runAwayPoint);
            _navMeshAgent.SetDestination(runAwayPoint);
        }
        // Get distance to runAwayPoint
        Vector3 distanceToPatrolPoint = transform.position - runAwayPoint;
        // Once the runAwayPoint is reached, runAwayPointSet is false
        if (distanceToPatrolPoint.magnitude < 0.5f)
        {
            runAwayPointSet = false;
        }
    }

    // Function to check for a Run Away Point
    public void SearchRunAwayPoint()
    {
        // Get Distance between Enemy and Player
        float distance = Vector3.Distance(transform.position, _enemy.target.position);
        // Get difference of maxRange and Enemy-Player distance
        float difference = (_enemy.entityStats.chaseRange + runAwayDistance) - distance;
        // Get vector pointing towards Player
        Vector3 direction = -1 * (_enemy.target.position - transform.position);
        direction.y = 0;
        direction = direction.normalized;
        // Set new RunAwayPoint away from Enemy
        runAwayPoint = transform.position + (direction * difference);
        // If runAwayPoint still on ground then runAwayPointSet is true
        if (Physics.Raycast(runAwayPoint + Vector3.up, Vector3.down, 3.0f, groundLayer))
        {
            // If it is, then patrolPointSet is true
            runAwayPointSet = true;
        }
        // Else, Enemy performs suicide run to Player
        else
        {
            suicide = true;
        }
    }

    // Enemy makes a suicide dash towards Player
    public void TestDash(Vector3 destination)
    {
        // Check that Player has a direct path to Enemy
        RaycastHit player;
        // If ray hits Player, then Enemy increases in speed to dash
        Physics.Raycast(transform.position, transform.forward, out player, Mathf.Infinity);
        if (player.collider.tag == "Player")
        {
            // If Dash just started, set isDashin to true and increase speed
            if (!isDashing)
            {
                // Set isDashing to true
                isDashing = true;
                // Move Enemy toward Player using SetDestination, with dashingBoost
                _navMeshAgent.speed = dashBoost * _enemy.entityStats.speed;
            }
        }
        // Else, enemy doesn't dash
        else
        {
            isDashing = false;
            _navMeshAgent.speed = _enemy.entityStats.speed;
        }
    }

    // Stop Enemy Movement

    public void StopEnemy()
    {
        // Set isMoving to false
        isMoving = false;
        // Set Destination to current position
        _navMeshAgent.enabled = false;
    }
    
    private void OnCollisionEnter(Collision col)
    {
        //Movement
        
        // If collide with a Player, they take damage and then they move back
        if (col.transform.CompareTag("Player"))
        {
            meleeAttack = false;
            if (isDashing)
            {
                isDashing = false;
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
    }
}
