using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerVFX))]
[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(WeaponController))]
public class Player : Entity
{
    // Movements
    private PlayerMovement playerMovement;
    private Rigidbody _rigidbody;
    //Combat
    private PlayerCombat playerCombat;
    public FloatValue soulCount;
    [SerializeField] private Ability firstAbility;
    [SerializeField] private Ability secondAbility;
    [SerializeField] private Ability dashAbility;
    private CinemachineImpulseSource _impulseSource;
    
    private bool _hasDashAbility = false;
    private bool _hasFirstAbility = false;
    private bool _hasSecondAbility = false;

    private List<int> _abilityProgression = new List<int>() {1, 2, 3};

    public GameEvent dashGotEvent;
    public GameEvent firstAbilityGotEvent;
    public GameEvent secondAbilityGotEvent;
    
    public GameEvent gameOverEvent;

    
    //VFX
    [SerializeField] private Material reaperMaterial;
    private PlayerVFX playerVFX;
    
    // Properties
    public PlayerMovement Movement => playerMovement;
    public Ability FirstAbility => firstAbility;
    public Ability SecondAbility => secondAbility;
    public Ability DashAbility => dashAbility;

    public bool HasDashAbility => _hasDashAbility;
    public bool HasFirstAbility => _hasFirstAbility;
    public bool HasSecondAbility => _hasSecondAbility;


    public PlayerCombat Combat => playerCombat;
    public PlayerVFX Vfx => playerVFX;
    public CinemachineImpulseSource ImpulseSource => _impulseSource;


    [Header("Events")]
    public GameEvent playerDeathEvent;
    public GameEvent playerHurtEvent;
    public GameEvent playerSpellEvent;
    public GameEvent playerAttackEvent;

    public Rigidbody Rigidbody => _rigidbody;

    public float iFramesDuration = 2f;


    public void Start()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _rigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        playerVFX = GetComponent<PlayerVFX>();
        
        //TODO: move in vfx
        reaperMaterial.color = Color.black;
    }


    public IEnumerator AbilityCo(float abilityDuration, Ability ability)
    {
        ability.SoulAbility(_rigidbody.position, playerMovement.mesh.transform.forward,
            Combat.anim, _rigidbody);
        
        yield return new WaitForSeconds(abilityDuration);
        // For invincibility ability
        if(IsInvincible) ToggleInvincibility();
    }

    private IEnumerator InvincibilityFramesCo()
    {
        yield return new WaitForSeconds(iFramesDuration);
        if(IsInvincible) ToggleInvincibility();
    }
    
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (IsInvincible) return;
        
        ToggleInvincibility();
        playerHurtEvent.Raise();

        _impulseSource.GenerateImpulse();
        if(Vfx.playerHurtParticles)
            Vfx.playerHurtParticles.Play();
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
        StartCoroutine(InvincibilityFramesCo());
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
    public override void Die()
    {
        // HUD and Game manager can listen to this event
        playerDeathEvent.Raise();
        base.Die();
    }
    public void OnLevelProgression()
    {
        int n = Random.Range(0, _abilityProgression.Count);
        int pick = _abilityProgression[n];
        _abilityProgression.Remove(pick);
        
        switch (pick)
        {
            case 1:
                _hasDashAbility = true;
                dashGotEvent.Raise();
                break;
            case 2:
                _hasFirstAbility = true;
                firstAbilityGotEvent.Raise();
                break;
            case 3:
                _hasSecondAbility = true;
                secondAbilityGotEvent.Raise();
                break;
            case 4:
                gameOverEvent.Raise();
                break;
                
        }


    }
    

}
