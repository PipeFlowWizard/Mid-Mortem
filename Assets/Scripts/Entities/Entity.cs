using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// All Enemies and Players inherit from Entity Class
public abstract class Entity : MonoBehaviour
{
    // Reference to EntityStats 
    public EntityStats entityStats;
    /* For Enemy, EntityStats come in 3 forms: ATTACK Enemy - Higher attackDamage but lower defense from Base
     *                                            DEFENSE Enemy - Higher defense but lower speed from Base
     *                                            SPEED Enemy - Higher speed but lower attackDamage from Base
    */


    // Reference to stats of Character
    private int _maxHealth;
    private int _currentHealth;
    private int _currentSouls;
    private int _currentAttack;
    private float _currentSpeed;
    private float _currentDefense;
    private bool _isInvincible;
    private float _currentAttackSpeed;

    public bool IsInvincible => _isInvincible;

    // Get Base Health and MP of Character
    protected virtual void Awake()
    {
        _maxHealth = entityStats.maxHealth;
        _currentHealth = _maxHealth;
        // _currentSouls = entityStats.mp;     // TODO: Change mp to souls
        _currentAttack = entityStats.attackDamage;
        _currentSpeed = entityStats.speed;
        _currentDefense = entityStats.defense;
        _currentAttackSpeed = entityStats.rangedAttackSpeed;
    }

    public float HealthPercent
    {
        // ReSharper disable once PossibleLossOfFraction
        get => _currentHealth / _maxHealth;
    }
    public int MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    public float CurrentAttackSpeed
    {
        get => _currentAttackSpeed;
        set => _currentAttackSpeed = value;
    }

    public int CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }

    public int CurrentSouls
    {
        get => _currentSouls;
        set => _currentSouls = value;
    }

    public int CurrentAttack
    {
        get => _currentAttack;
        set => _currentAttack = value;
    }

    public float CurrentSpeed
    {
        get => _currentSpeed;
        set => _currentSpeed = value;
    }

    public float CurrentDefense
    {
        get => _currentDefense;
        set => _currentDefense = value;
    }

    public virtual void ToggleInvincibility()
    {
        _isInvincible = !_isInvincible;
        Debug.Log(_isInvincible ? "Invincibility Active" : "Invincibility Off");
    }
    
    public float PercentHealth
    {
        get => _currentHealth / entityStats.maxHealth;
    }

    // TakeDamage subtracts passed amount from _currentHealth
    // Passed value is also multiplied by current defense of character
    public virtual void TakeDamage(int amount)
    {
        if (_isInvincible) return;
        if (_currentHealth > 0)
        {
            _currentHealth -= (int)(entityStats.defense * amount);
        }
    }

    // CurrentHealthState returns what range Character _currentHealth is in
    public int CurrentHealthState()
    {
        // If current Health is greater than 50%, return 0
        if(_currentHealth > entityStats.maxHealth / 2)
        {
            return 0;
        }
        // Else if current Health is less than 50% and greater than 25%, return 1
        else if(_currentHealth <= entityStats.maxHealth / 2 && _currentHealth > entityStats.maxHealth / 4)
        {
            return 1;
        }
        // Else, current Health is less than 25% and greater than 0, return 2
        else if(_currentHealth <= entityStats.maxHealth / 4 && _currentHealth > 0)
        {
            return 2;
        }
        // Else, if current Health is 0, then return 3
        else
        {
            return 3;
        }
    }

    public virtual void Die()
    {
        // unsubscribe from all events -> die
        Destroy(gameObject);
    }
}
