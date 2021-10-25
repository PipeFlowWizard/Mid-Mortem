using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Spell : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float lifetime = 10.0f;
    [SerializeField] private Rigidbody rb;

    private float _spawnTime;


    private void Start()
    {
        _spawnTime = Time.time;
    }

    // I'll call this myself, as opposed to Start()
    public void Initialize(Vector3 forward, int attack)
    {
        rb.velocity = forward * speed;
        damage = attack;
    }

    private void Update()
    {
        if (Time.time > _spawnTime + lifetime)
        {
            if (gameObject)
            {
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Damage
            Debug.Log("Enemy hit");
            other.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if (gameObject && !other.CompareTag("Player") && !other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
