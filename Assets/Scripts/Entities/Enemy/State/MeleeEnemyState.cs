using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyState : State
{
    
    // MeleeEnemyState takes an Enenmy Object in constructor
    public MeleeEnemyState(Enemy enemy) : base(enemy)
    {

    }

    // MeleeEnemyState can perform different actions based on distance to Enemy and health
    public override void Action()
    {
        // Get Player distance
        float distance = GetPlayerDistance();
        // If Enemy health is 0, then Enemy State is DeadEnemyState
        // CurrentHealthState returns 3 when health is 0
        if (enemy.CurrentHealthState() == 3)
        {
            enemy.SetState(new DeadEnemyState(enemy));
        }
        // If Enemy health is 25% or below and canReap , then Enemy State is ReapEnemyState
        // CurrentHealthState returns 2 when enemy health is below or equal to 25% of original
        else if (enemy.CurrentHealthState() == 2 && enemy.canReap)
        {
            enemy.SetState(new ReapEnemyState(enemy));
        }
        // If target of enemy is null or distance > max_range, then set Enemy State to IdleEnemyState
        else if (enemy.target == null || distance >= enemy.entityStats.maxRange)
        {
            enemy.SetState(new IdleEnemyState(enemy));
        }
        // If distance to enemy is greater than chase_range, then Enemy State is RangedEnemyState
        else if (distance > enemy.entityStats.chaseRange)
        {
            enemy.SetState(new RangedEnemyState(enemy));
        }
        // Else
        else
        {
            // MeleeEnemyState attacks Player at intervals
            //enemy.Movement.TurnEnemy();
            //enemy.Movement.MoveEnemy();
            
        }
    }

    // GetPlayerDistance returns distance to Player object
    private float GetPlayerDistance()
    {
        return Vector3.Distance(enemy.transform.position, enemy.target.position);
    }
}
