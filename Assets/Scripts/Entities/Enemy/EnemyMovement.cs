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
    public LayerMask groundLayer;                         // Reference to Ground LayerMask
    public bool suicide;                                    // If Enemy is going to perform a suicide run
    
    [SerializeField] private float dashBoost;               // Dash Boost when Enemy is Dashin

    private Enemy _enemy;
    [SerializeField] private float rotationDamp = 0.5f;     // Rotational Dampening so rotation is gradual

    public Vector3 desiredLocation;

    public Rigidbody Rigidbody => _rigidbody;

    public Animator anim;

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
        groundLayer = LayerMask.GetMask("Ground");
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
                transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotationDamp );
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
        RaycastHit hit;
        // If ray hits Player, then Enemy increases in speed to dash
        if (Physics.Raycast(transform.position, _enemy.target.transform.position - transform.position, out hit, Mathf.Infinity) && hit.collider.CompareTag("Player"))
        {
            
            anim.Play("Dash");
            Debug.Log("Dash Attack");
            Debug.DrawRay(transform.position, hit.point-transform.position,Color.red);
            // If Dash just started, set isDashin to true and increase speed
            // Set isDashing to true
            //isDashing = true;
            // Move Enemy toward Player using SetDestination, with dashingBoost
            _rigidbody.AddForce((hit.point - transform.position).normalized * dashBoost, ForceMode.Impulse);
            //_navMeshAgent.speed = dashBoost * _enemy.entityStats.speed;
            
            
        }
        // Else, enemy doesn't dash
        else
        {
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
}
