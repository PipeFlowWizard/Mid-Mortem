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
        // Get randomNumber to detemine if special ability used
        int randomNumber = UnityEngine.Random.Range(1, 1001);
        // If randomNumber is between 2 and 4 then Enemy can try and Dash to Player, if Enemy is SPEED
        if ((randomNumber >= 1 && randomNumber <= 10 && enemy.entityStats.entityType == EntityStats.EntityType.SPEED && !enemy.isBossEnemy) || enemy.Movement.isDashing)
        {
            enemy.Movement.TestDash(enemy.target.position);
        }
        enemy.Movement.TurnEnemy(enemy.target.position);
        enemy.Movement.MoveEnemy(enemy.target.position);
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}
