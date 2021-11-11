using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpellCaster))]
//[RequireComponent(typeof(WeaponController))]
public class Player : Entity
{
    
    [SerializeField] private PlayerController playerController;
    //Combat
    [SerializeField] private SpellCaster spellCaster;
   // [SerializeField] private WeaponController weaponController;
   [SerializeField] private Ability currentAbility;
    [SerializeField] private Transform shootOut;
    
    //VFX
    [SerializeField] private Material reaperMaterial;
    private Rigidbody _rigidbody;
    
    public PlayerController PlayerController => playerController;
    public Ability CurrentAbility => currentAbility;
    public SpellCaster SpellCaster => spellCaster;

    [HideInInspector] public bool canDash; 

    public GameEvent playerDeathEvent;
    

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        canDash = true;
    }


    public IEnumerator AbilityCo(float abilityDuration)
    {
        // Change state
        //...
        currentAbility.SoulAbility(_rigidbody.position, playerController.mesh.transform.forward,
            playerController.anim, _rigidbody);
        yield return new WaitForSeconds(abilityDuration);
        // Change back state
        // ...
    }

    #region Combat

    private void MeleeAttack()
    {
        playerController.anim.Play("MeleeAttack");
    }
    
    private void ChargedMeleeAttack()
    {
    }
    public void OnMeleeInput()
    {
        MeleeAttack();
    }

    public void OnMeleeChargeInput()
    {
        ChargedMeleeAttack();
    }

    public void OnRangedInput()
    {
        spellCaster.Cast(shootOut.position, playerController.mesh.transform.forward, entityStats.attack);
    }
    
    public void OnRangedChargeInput()
    {
    }

    #endregion

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        
        // Player ded
        if (CurrentHealth <= 0)
        {
            Die();
        }

        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(DamageFlash());
        }
        // Make sure reaper is back to origin color (just in case)
        reaperMaterial.color = Color.black;
    }
    #region VFX
    private IEnumerator DamageFlash()
    {
        for (int i = 0; i < 5; i++)
        {
            Color flash = Color.white;
            reaperMaterial.color = flash;
            yield return new WaitForSeconds(.2f);
            reaperMaterial.color = Color.black;
            yield return new WaitForSeconds(.2f);
        }

    }
#endregion
    protected override void Die()
    {
        // HUD and Game manager can listen to this event
        playerDeathEvent.Raise();
        base.Die();
    }
    

}