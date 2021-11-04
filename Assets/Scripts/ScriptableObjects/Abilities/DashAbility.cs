using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DashAbility : Ability
{
    [SerializeField] private float dashForce;
    
    public override void SoulAbility(Vector3 position, Vector3 forward, Animator anim = null, Rigidbody rb = null)
    {
        if (playerSoulCount.runTimeValue < soulCost) return;
        
        // Enough souls
        playerSoulCount.runTimeValue -= soulCost;
        playerAbilityUsedEvent.Raise();

        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(forward.normalized * dashForce, ForceMode.VelocityChange);
        }
    }
}
