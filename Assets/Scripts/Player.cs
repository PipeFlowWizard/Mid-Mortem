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

    [SerializeField] private Material reaperMaterial;
    public PlayerController PlayerController => playerController;
    public Ability CurrentAbility => currentAbility;
    public Rigidbody Rigidbody => rb;
    public SpellCaster SpellCaster => spellCaster;

    [HideInInspector] public bool canDash;

    public GameEvent playerDeathEvent;

    public void Start()
    {
        canDash = true;
        reaperMaterial.color = Color.black;
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
       spellCaster.Cast(shootOut.position, playerController.mesh.transform.forward, characterStats.attack);
    }
    
    public void OnRangedChargeInput()
    {
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        
        // Player ded
        if (GetHealth() <= 0)
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

    private void Die()
    {
        // HUD and Game manager can listen to this event
        playerDeathEvent.Raise();
        Destroy(gameObject);
    }
    

}
