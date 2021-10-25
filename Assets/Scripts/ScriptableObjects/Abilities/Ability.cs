using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// See MisterTaftCreates
// https://youtu.be/cNeQx_wYoog
[CreateAssetMenu]
public class Ability : ScriptableObject
{
    public float soulCost;
    public float duration;

    public FloatValue playerSoulCount;
    public GameEvent playerAbilityUsedEvent;
    
    public virtual void SoulAbility(Vector3 position, Vector3 forward, Animator anim = null, Rigidbody rb = null) {}
}
