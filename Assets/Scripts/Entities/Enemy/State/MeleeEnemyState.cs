using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyState : State
{
    
    // ChaseEnemyState takes an Enenmy Object in constructor
    public MeleeEnemyState(Enemy enemy) : base(enemy)
    {

    }

    // ChaseEnemyState can perform different actions based on distance to Enemy and health
    public override void Action()
    {
        Debug.Log("Enemy attacks!");
        /*// If attack is true, then keep moving toward enemy
        if (enemy.Combat.meleeAttack)
        {
            // Get randomNumber to detemine if special ability used
            int randomNumber = UnityEngine.Random.Range(1, 1001);
            enemy.Movement.TurnEnemy(enemy.target.position);
            // If randomNumber is between 2 and 4 then Enemy can try and Dash to Player, if Enemy is SPEED
            if ((randomNumber >= 1 && randomNumber <= 3 && enemy.entityStats.entityType == EntityStats.EntityType.SPEED && !enemy.isBossEnemy) || enemy.Movement.isDashing)
            {
               enemy.Movement.TestDash(enemy.target.position);
            }
            // MeleeEnemyState attacks Player at intervals
            enemy.Movement.MoveEnemy(enemy.target.position);
            
        }*/
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        enemy.VFX.ChangeColor();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        enemy.VFX.ChangeColor();
    }
}
