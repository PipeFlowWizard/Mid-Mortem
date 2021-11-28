using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// RangedEnemyState is when Enemies fire ranged attacks
public class RangedEnemyState : State
{ 
    // RangedEnemyState takes an Enenmy Object in constructor
    public RangedEnemyState(Enemy enemy) : base(enemy)
    {

    }

    // On Entering this State, Enemy Stops moving then turns
    public override void OnStateEnter()
    {
        enemy.Movement.StopEnemy();
    }

    // On Exiting this State, rangeAttack is set to false
    public override void OnStateExit()
    {
        enemy.Combat.rangeAttack = false;
        enemy.Movement._navMeshAgent.enabled = true;
    }

    // RangedEnemyState can perform different actions based on distance to Enemy and health
    public override void Action()
    {
        // Turn enemy toward Player
        enemy.Movement.TurnEnemy(enemy.target.position);
        // If attack is true, then can call RangeAttack and Start Coroutine AttackTimer
        // to wait 3 seconds before next ranged attack
        if (enemy.Combat.rangeAttack)
        {
            // Get randomNumber to detemrine if special attack used
            int randomNumber = UnityEngine.Random.Range(1, 101);
            // If Enemy is Boss, then it can use Boss Attacks
            if (enemy.isBossEnemy)
            {
                // If randomNumber is between 1 & 8 and current level is greater than or equal to 1, then Boss can use XAttack
                if (randomNumber >= 1 && randomNumber <= 8 && enemy.currentLevel >= 1)
                {
                    enemy.Combat.rangeAttack = false;
                    enemy.Combat.XAttack();
                }
                // If randomNumber is between 9 and 16 and current level is greater than or equal to 2, then Boss can use MeteorFall
                if (randomNumber >= 9 && randomNumber <= 16 && enemy.currentLevel >= 2)
                {
                    enemy.Combat.rangeAttack = false;
                    enemy.Combat.MeteorFall();
                }
                // If randomNumer is between 17 and 24 and current level is greater than or equal to 3, then Boss can use HeatSeeker
                if(randomNumber >= 17 && randomNumber <= 24 && enemy.currentLevel >= 3)
                {
                    enemy.Combat.rangeAttack = false;
                    enemy.Combat.HeatSeeker();
                }
                // Else, just normal Ranged Attack
                else
                {
                    enemy.Combat.rangeAttack = false;
                    enemy.Combat.RangedAttack();
                }
            }
            // If Enemy is normal enemy, it can use abilities
            else
            {
                // If randomNumber is between 1 & 5 and Enemy is DEFENSE, then use Invincible
                if (randomNumber >= 1 && randomNumber <= 5 && enemy.entityStats.entityType == EntityStats.EntityType.DEFENSE)
                {
                    enemy.Combat.Invincible();
                }
                // If randomNumber is between 6 and 10 and Enemy is ATTACK, then fire TripleRangedAttack
                if (randomNumber >= 6 && randomNumber <= 10 && enemy.entityStats.entityType == EntityStats.EntityType.ATTACK)
                {
                    enemy.Combat.rangeAttack = false;
                    enemy.Combat.TripleRangedAttack();
                }
                else
                {
                    enemy.Combat.rangeAttack = false;
                    enemy.Combat.RangedAttack();
                }
            }
        }
    }
    
}
