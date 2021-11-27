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
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponentInParent<Enemy>();
            
            // If Enemy is waitingForReap, then they can call the ReapEnemy Function
            // TODO: Add in Reap Animation and adding modifier 
            if (enemy.waitingForReap)
            {
                Debug.Log("I T S  R E A P I N'  T I M E");
                enemy.Combat.reapedEvent.Raise();
                enemy.Movement.StopEnemy();
                enemy.Combat.KillEnemy();
            }
            // Else, the Enemy just takes normal damage
            else
            {
                enemy.TakeDamage(parent.entityStats.attack);
                enemy.Movement.Rigidbody.AddForce(parent.transform.forward * 5, ForceMode.Impulse);
                other.GetComponentInParent<EnemyVFX>().SetEnemyHealthState();
            }

            
            Debug.Log("enemy damaged");
        }
    }
}
