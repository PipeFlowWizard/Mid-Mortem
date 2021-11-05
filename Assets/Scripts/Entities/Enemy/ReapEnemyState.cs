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
        enemy.StopEnemy();
        enemy.waitingForReap = true;
        enemy.ReapEnemyTimer();
    }

    // RangedEnemyState can perform different actions based on distance to Enemy and health
    public override void Action()
    {
        // If canReap is true, then Enemy does nothing and faces enemy
        enemy.TurnEnemy();
        // If Enemy health is 0, then Enemy State is DeadEnemyState
        // CurrentHealthState returns 3 when health is 0
        if (enemy.CurrentHealthState() == 3)
        {
            enemy.SetState(new DeadEnemyState(enemy));
        }
        else if (!enemy.canReap)
        {
            // Get Player distance
            float distance = GetPlayerDistance();
            // If target of enemy is null or distance > max_range, then set Enemy State to IdleEnemyState
            if (enemy.target == null || distance >= enemy.characterStats.max_range)
            {
                enemy.SetState(new IdleEnemyState(enemy));
            }
            // If distance to enemy is greater than chase_range but still less then max_range, then Enemy State is RangedEnemyState
            else if (distance > enemy.characterStats.chase_range)
            {
                enemy.SetState(new RangedEnemyState(enemy));
            }
            // If distance to enemy is less than or equal to chase_range, then Enemy State is MeleeEnemyState
            else if (distance <= enemy.characterStats.chase_range)
            {
                enemy.SetState(new MeleeEnemyState(enemy));
            }
        }
    }

    // GetPlayerDistance returns distance to Player object
    private float GetPlayerDistance()
    {
        return Vector3.Distance(enemy.transform.position, enemy.target.position);
    }
}
