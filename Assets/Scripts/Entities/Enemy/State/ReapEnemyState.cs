using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReapEnemyState is when Player can Reap Enemy for Powerup
public class ReapEnemyState : State
{    
    // ReapEnemyState takes an Enenmy Object in constructor
    public ReapEnemyState(Enemy enemy) : base(enemy)
    {

    }

    // On Entering this State, Enemy should stop moving
    public override void OnStateEnter()
    {
        Debug.Log("Enter Reap State");
        enemy.Movement.StopEnemy();
        enemy.waitingForReap = true;
        enemy.Combat.ReapEnemyTimer();
    }

    // RangedEnemyState can perform different actions based on distance to Enemy and health
    public override void Action()
    {
        // If canReap is true, then Enemy does nothing and faces enemy
        enemy.Movement.TurnEnemy(enemy.target.position);
    }
}
