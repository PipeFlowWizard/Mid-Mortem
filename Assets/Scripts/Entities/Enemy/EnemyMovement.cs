using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    // Enemy Movement
    private Rigidbody _rigidbody;                           // Reference to RigidBody of Enemy
    public bool isMoving;                                   // Bool to determine if Player is moving
    private Enemy _enemy;
    [SerializeField] private float rotationDamp = 0.5f;     // Rotational Dampening so rotation is gradual

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
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
}
