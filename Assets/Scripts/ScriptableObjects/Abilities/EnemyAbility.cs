using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyAbility : Ability
{
    [SerializeField] private GameObject meteorFallObject;
    
    public override void SoulAbility(Vector3 position, Vector3 direction, Animator anim = null, Rigidbody rb = null)
    {
        
        var meteor = Instantiate(meteorFallObject, position, Quaternion.identity);
            MeteorFallAttack meteorAttack = meteor.GetComponent<MeteorFallAttack>();
            // Start Timer to wait for next Ranged Attack
            meteorAttack.MeteorCrash();
    }
}
