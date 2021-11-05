using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// All Enemies and Players inherit from Entity Class
public abstract class Entity : MonoBehaviour, IDamageable
{
    // Reference to EntityStats 
    public EntityStats entityStats;
    /* For Enemy, EntityStats come in 3 forms: ATTACK Enemy - Higher attack but lower defense from Base
     *                                            DEFENSE Enemy - Higher defense but lower speed from Base
     *                                            SPEED Enemy - Higher speed but lower attack from Base
    */


    // Reference to stats of Character
    private int _currentHealth;

    // Get Base Health and MP of Character
    protected virtual void Awake()
    {
        _currentHealth = entityStats.health;
    }

    public int CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }

    // TakeDamage subtracts passed amount from _currentHealth
    // Passed value is also multiplied by current defense of character
    public virtual void TakeDamage(int amount)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= (int)(entityStats.defense * amount);
        }
    }

    // CurrentHealthState returns what range Character _currentHealth is in
    public int CurrentHealthState()
    {
        // If current Health is greater than 50%, return 0
        if(_currentHealth > entityStats.health / 2)
        {
            return 0;
        }
        // Else if current Health is less than 50% and greater than 25%, return 1
        else if(_currentHealth <= entityStats.health / 2 && _currentHealth > entityStats.health / 4)
        {
            return 1;
        }
        // Else, current Health is less than 25% and greater than 0, return 2
        else if(_currentHealth <= entityStats.health / 4 && _currentHealth > 0)
        {
            return 2;
        }
        // Else, if current Health is 0, then return 3
        else
        {
            return 3;
        }
    }
}
