using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    // Enemy Movement
    private Rigidbody _rigidbody;                           // Reference to RigidBody of Enemy
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
        //_rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Rotate Enemy toward Player

    public void TurnEnemy()
    {
        // Get vector pointing towards Player
        Vector3 direction = _enemy.target.position - transform.position;
        direction.y = 0;
        // Get Quaternion to rotate towards Player
        Quaternion rotate = Quaternion.LookRotation(direction, Vector3.up);
        // Rotate Enemy, use Slerp to make Rotation gradual
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotationDamp);
    }

    // Move Enemy toward Player

    public void MoveEnemy()
    {
        // Set isMoving to true
        isMoving = true;
        // Move Enemy in direction they are facing using RigidBody
        _rigidbody.MovePosition(transform.position + transform.forward * _enemy.entityStats.speed * Time.deltaTime);
    }

    // Stop Enemy Movement

    public void StopEnemy()
    {
        // Set isMoving to false
        isMoving = false;
        // Set velocity of Enemy
        _rigidbody.velocity = Vector3.zero;
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
}
