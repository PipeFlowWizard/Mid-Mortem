using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DefenseAbility : Ability
{
    
    public override void SoulAbility(Vector3 position, Vector3 direction, Animator anim = null, Rigidbody rb = null)
    {
        if (playerSoulCount.runTimeValue < soulCost) return;

        playerSoulCount.runTimeValue -= soulCost;
        playerAbilityUsedEvent.Raise();
        if (rb)
        {

            var entity = rb.GetComponentInParent<Entity>();
            
            if (!entity) return;
            entity.ToggleInvincibility();
        }

    }
}
