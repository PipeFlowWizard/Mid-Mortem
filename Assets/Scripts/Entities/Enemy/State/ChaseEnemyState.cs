using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemyState : State
{
    
    // ChaseEnemyState takes an Enenmy Object in constructor
    public ChaseEnemyState(Enemy enemy) : base(enemy)
    {

    }

    // ChaseEnemyState can perform different actions based on distance to Enemy and health
    public override void Action()
    {
        // If attack is true, then keep moving toward enemy
        if (enemy.Combat.meleeAttack)
        {
            // Get randomNumber to detemrine if special ability used
            int randomNumber = UnityEngine.Random.Range(1, 1001);
            enemy.Movement.TurnEnemy(enemy.target.position);
            // If randomNumber is between 2 and 4 then Enemy can try and Dash to Player, if Enemy is SPEED
            if ((randomNumber >= 1 && randomNumber <= 3 && enemy.entityStats.entityType == EntityStats.EntityType.SPEED && !enemy.isBossEnemy) || enemy.Movement.isDashing)
            {
               enemy.Movement.TestDash(enemy.target.position);
            }
            // MeleeEnemyState attacks Player at intervalsenemy.Movement.MoveEnemy(enemy.target.position);
        }
    }
}
