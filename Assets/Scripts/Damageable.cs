using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All Enemies and Players inherit from Damageable Class
public abstract class Damageable : MonoBehaviour
{
    // Reference to CharacterStats 
    public CharacterStats characterStats;
    /* For Enemy, CharacterStats come in 3 forms: ATTACK Enemy - Higher attack but lower defense from Base
     *                                            DEFENSE Enemy - Higher defense but lower speed from Base
     *                                            SPEED Enemy - Higher speed but lower attack from Base
    */


    // Reference to stats of Character
    private int BASE_HEALTH;
    private int health;
    private int BASE_MP;
    private int mp;


    // Get Base Health and MP of Character
    private void Awake()
    {
        BASE_HEALTH = characterStats.health;
        health = characterStats.health;
        BASE_MP = characterStats.mp;
        mp = characterStats.mp;
    }

    // GetHealth returns current Health of Character
    public int GetHealth()
    {
        return health;
    }

    // TakeDamage subtracts passed amount from health
    // Passed value is also multiplied by current defense of character
    public virtual void TakeDamage(int amount)
    {
        if (health > 0)
        {
            health -= (int)(characterStats.defense * amount);
        }
    }

    // CurrentHealthState returns what range Character health is in
    public int CurrentHealthState()
    {
        // If current Health is greater than 50%, return 0
        if(health > BASE_HEALTH / 2)
        {
            return 0;
        }
        // Else if current Health is less than 50% and greater than 25%, return 1
        else if(health <= BASE_HEALTH / 2 && health > BASE_HEALTH / 4)
        {
            return 1;
        }
        // Else, current Health is less than 25% and greater than 0, return 2
        else if(health <= BASE_HEALTH / 4 && health > 0)
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
