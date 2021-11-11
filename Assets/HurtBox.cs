using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    private Player parent;
    private void Start()
    {
        parent = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered scythe");
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponentInParent<Enemy>();
            enemy.TakeDamage(10);
            enemy.Movement.Rigidbody.AddForce(parent.transform.forward * 5, ForceMode.Impulse);
            other.GetComponentInParent<EnemyVFX>().SetEnemyHealthState();
            
            Debug.Log("enemy damaged");
        }
    }
}
