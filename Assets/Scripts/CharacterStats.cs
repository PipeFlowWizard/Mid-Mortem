using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CharacterStats holds all stats for Player and Enemy Objects
[CreateAssetMenu(menuName = "Character Stats")]
public class CharacterStats : ScriptableObject
{
    public int health = 100;                                        // Base health of Characters is 100
    public int mp = 100;                                            // Base mp of Characters is 100
    public int attack = 10;                                         // Base attack of Character is 10
    public int rangedSpawn = 3;                                     // Base time between Enemy ranged attacks is 3 seconds
    public int meleeSpawn = 2;                                      // Base time between Enemy melee attacks is 2 seconds
    public int reapTime = 10;                                       // Player has 10 seconds to Reap Enemy
    public float defense = 0.75f;                                   // Base defense of Character is Character takes 75% of damage
    public float speed = 3.0f;                                      // Base speed of Character is 3.0
    public float max_range = 15.0f;                                 // Base attack range of Character is 15
    public float chase_range = 5.0f;                                // Base melee attack range of Character is 5
    public float attack_speed = 5.0f;                               // Base attack speed of Character is 5
    public int modStatBoost = 10;                                   // Base modifier stat boost is 10
    public enum CharacterType { ATTACK, SPEED, DEFENSE, HEALTH };   // Type of powerup Character can provide
                                                                    // ATTACK means Character provides an ATTACK modifier after Reap
                                                                    // SPEED means Character provides a SPEED modifier after Reap
                                                                    // DEFENSE means Character provides a DEFENSE modifier after Reap
                                                                    // HEALTH means Character provides a HEALTH modifier after Reap
    public CharacterType characterType = CharacterType.HEALTH;      // Base characterType is HEALTH
}
