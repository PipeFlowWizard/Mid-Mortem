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
            var player = other.GetComponentInParent<Player>();
            if(player)
            {
                player.Rigidbody.AddForce((player.transform.position - parent.transform.position).normalized * 10,
                    ForceMode.Impulse);
                player.TakeDamage(parent.entityStats.attackDamage);
            }
        }
    }
}
