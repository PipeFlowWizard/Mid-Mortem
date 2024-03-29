using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    private Player parent;
    [SerializeField] CinemachineImpulseSource _impulseSource;
    public ParticleSystem particles;
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
                enemy.Reap();
                //Add and play an animation instead of this! it's also a bit wonky
            }
            // Else, the Enemy just takes normal damage
            else
            {
                particles.Play();
                if(_impulseSource) _impulseSource.GenerateImpulse();
                enemy.TakeDamage(parent.entityStats.attackDamage);
                enemy.Movement.Rigidbody.AddForce((enemy.transform.position - parent.transform.position).normalized * 10, ForceMode.VelocityChange);
                other.GetComponentInParent<EnemyVFX>().SetEnemyHealthState();
            }
        }
    }
}
