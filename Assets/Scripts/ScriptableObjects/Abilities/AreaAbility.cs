using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AreaAbility : Ability
{
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    
    public override void SoulAbility(Vector3 position, Vector3 direction, Animator anim = null, Rigidbody rb = null)
    {
        if (playerSoulCount.runTimeValue < soulCost) return;
        
        // Enough souls
        playerSoulCount.runTimeValue -= soulCost;
        playerAbilityUsedEvent.Raise();



        if (anim)
        {
            anim.Play("AreaOfEffect");
        }

        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.AddExplosionForce(explosionForce, rb.position, explosionRadius);
        }
    }
}
