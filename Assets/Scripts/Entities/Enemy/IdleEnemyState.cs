using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEnemyState : State
{
    // IdleEnemyState takes an Enenmy Object in constructor
    public IdleEnemyState(Enemy enemy) : base(enemy)
    {

    }

    // Enemy performs no Action while in Idle state, except looking for Player
    public override void Action()
    {
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
        // If Player is in same scene as enemy, Enemy switches State
        else if (enemy.target != null)
        {
            // Get Enemy distance from Player
            float distance = GetPlayerDistance();
            // If ditance is less than max_range and greater than chase_range, set Enemy state to RangedEnemyState
            if (distance < enemy.entityStats.maxRange && distance > enemy.entityStats.chaseRange)
            {
                enemy.SetState(new RangedEnemyState(enemy));
            }
            // Else if distance is less than chase_range, them set Enemy State to MeleeEnemyState
            else if(distance <= enemy.entityStats.chaseRange)
            {
                enemy.SetState(new MeleeEnemyState(enemy));
            }
        }
        // Else, just keep searching for Player
        else
        {
            enemy.GetPlayer();
        }
    }

    // OnStateEnter Stop Enemy Movement
    public override void OnStateEnter()
    {
        enemy.StopEnemy();
    }

    // GetPlayerDistance returns distance to Player object
    private float GetPlayerDistance()
    {
        return Vector3.Distance(enemy.transform.position, enemy.target.position);
    }
}
