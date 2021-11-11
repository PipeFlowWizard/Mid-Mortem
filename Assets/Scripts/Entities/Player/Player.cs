using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(WeaponController))]
public class Player : Entity
{
    // Movements
    private PlayerMovement playerMovement;
    //Combat
    private PlayerCombat playerCombat;
    // [SerializeField] private WeaponController weaponController;
   [SerializeField] private Ability currentAbility;
   //VFX
    [SerializeField] private Material reaperMaterial;
    private Rigidbody _rigidbody;
    
    public PlayerMovement Movement => playerMovement;
    public Ability CurrentAbility => currentAbility;

    [HideInInspector] public bool canDash = true; 

    public GameEvent playerDeathEvent;

    public PlayerCombat PlayerCombat => playerCombat;

    public PlayerCombat Combat => playerCombat;

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
    }


    public IEnumerator AbilityCo(float abilityDuration)
    {
        // Change state
        //...
        currentAbility.SoulAbility(_rigidbody.position, playerMovement.mesh.transform.forward,
            Combat.anim, _rigidbody);
        yield return new WaitForSeconds(abilityDuration);
        // Change back state
        // ...
    }

    #region Combat

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
