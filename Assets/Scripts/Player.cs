using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpellCaster))]
[RequireComponent(typeof(WeaponController))]
public class Player : Damageable
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SpellCaster spellCaster;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private Ability currentAbility;
    [SerializeField] private Transform shootOut;
    
    public PlayerController PlayerController => playerController;
    public Ability CurrentAbility => currentAbility;
    
    public Rigidbody Rigidbody => rb;
    public SpellCaster SpellCaster => spellCaster;

    public bool canDash;

    public void Start()
    {
        canDash = true;
    }


    public IEnumerator AbilityCo(float abilityDuration)
    {
        // Change state
        //...
        currentAbility.SoulAbility(rb.position, playerController.mesh.transform.forward,
            playerController.anim, rb);
        yield return new WaitForSeconds(abilityDuration);
        // Change back state
        // ...
    }
    

    private void MeleeAttack()
    {
        playerController.anim.Play("MeleeAttack");
    }
    
    public void OnMeleeInput()
    {
        MeleeAttack();
    }

    public void OnMeleeChargeInput()
    {
    }

    public void OnRangedInput()
    {
       spellCaster.Cast(shootOut.position, playerController.mesh.transform.forward, characterStats.attack);
    }
    
    public void OnRangedChargeInput()
    {
    }

}
