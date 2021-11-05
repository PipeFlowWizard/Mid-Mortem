using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// EntityStats holds all stats for Player and Enemy Objects
[CreateAssetMenu(menuName = "Entity Stats")]
public class EntityStats : ScriptableObject
{
    
    public enum EntityType { ATTACK, SPEED, DEFENSE, HEALTH };   // Type of buff Entity can provide
    
    public int health = 100;                                        // Base health of Entities is 100
    public int mp = 100;                                            // Base mp of Entities is 100
    public int attack = 10;                                         // Base attack of Entity is 10
    public int rangedSpawn = 3;                                     // Base time between Enemy ranged attacks is 3 seconds  --> time between attacks
    public int meleeSpawn = 2;                                      // Base time between Enemy melee attacks is 2 seconds   --> time between attacks
    public int reapTime = 10;                                       // Player has 10 seconds to Reap Enemy
    public float defense = 0.75f;                                   // Base defense of Entity is Entity takes 75% of damage
    public float speed = 3.0f;                                      // Base speed of Entity is 3.0
    public float maxRange = 15.0f;                                 // Base attack range of Entity is 15
    public float chaseRange = 5.0f;                                // Base melee attack range of Entity is 5
    public float attackSpeed = 5.0f;                               // Base attack speed of Entity is 5
    public int modStatBoost = 10;                                   // Base modifier stat boost is 10
    
    public EntityType entityType = EntityType.HEALTH;                 // Base EntityType is HEALTH    
                                                                    // ATTACK means Entity provides an ATTACK modifier after Reap
                                                                    // SPEED means Entity provides a SPEED modifier after Reap
                                                                    // DEFENSE means Entity provides a DEFENSE modifier after Reap
                                                                    // HEALTH means Entity provides a HEALTH modifier after Reap

    
       
    

     
    
    
}
