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
                enemy.Combat.RaiseReapEvent();
                enemy.Movement.StopEnemy();
                //Add and play an animation instead of this! it's also a bit wonky
                enemy.gameObject.AddComponent<ReapLevitation>();
                enemy.Combat.KillAfterSeconds(5f);
            }
            // Else, the Enemy just takes normal damage
            else
            {
                enemy.TakeDamage(parent.entityStats.attack);
                enemy.Movement.Rigidbody.AddForce(parent.transform.forward * 2, ForceMode.Impulse);
                other.GetComponentInParent<EnemyVFX>().SetEnemyHealthState();
            }
        }
    }
}
