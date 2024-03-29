using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DirectionalDashAbility : Ability
{
    [SerializeField] private float dashForce;
    
    public override void SoulAbility(Vector3 position, Vector3 direction, Animator anim = null, Rigidbody rb = null)
    {
        if (playerSoulCount.runTimeValue < soulCost) return;
        
        // Enough souls
        playerSoulCount.runTimeValue -= soulCost;
        playerAbilityUsedEvent.Raise();

        if (rb)
        {
            
            rb.AddForce(rb.velocity.normalized * dashForce, ForceMode.VelocityChange );
        }
    }
}
