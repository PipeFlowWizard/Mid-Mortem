using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtBox : MonoBehaviour
{
    private Enemy parent;
    private void Start()
    {
        parent = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            
            player.Rigidbody.AddForce((player.transform.position - transform.position) * 100,ForceMode.Impulse);
            player.TakeDamage(parent.entityStats.attack);
            Debug.Log("player damaged");
            
        }
    }
}
