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
        enemy.StopEnemy();
    }

    // On Exiting this State, rangeAttack is set to false
    public override void OnStateExit()
    {
        enemy.rangeAttack = false;
    }

    // RangedEnemyState can perform different actions based on distance to Enemy and health
    public override void Action()
    {
        // Get Player distance
        float distance = GetPlayerDistance();
        // If Enemy health is 0, then Enemy State is DeadEnemyState
        // CurrentHealthState returns 3 when health is 0
        if(enemy.CurrentHealthState() == 3)
        {
            enemy.SetState(new DeadEnemyState(enemy));
        }
        // If Enemy health is 25% or below and canReap , then Enemy State is ReapEnemyState
        // CurrentHealthState returns 2 when enemy health is below or equal to 25% of original
        else if(enemy.CurrentHealthState() == 2 && enemy.canReap)
        {
            enemy.SetState(new ReapEnemyState(enemy));
        }
        // If target of enemy is null or distance > max_range, then set Enemy State to IdleEnemyState
        else if(enemy.target == null || distance >= enemy.characterStats.max_range)
        {
            enemy.SetState(new IdleEnemyState(enemy));
        }
        // If distance to enemy is less than or equal to chase_range, then Enemy State is MeleeEnemyState
        else if(distance <= enemy.characterStats.chase_range)
        {
            enemy.SetState(new MeleeEnemyState(enemy));
        }
        // Else
        else
        {
            // Turn enemy toward Player
            enemy.TurnEnemy();
            // If attack is true, then can call RangeAttack and Start Coroutine AttackTimer
            // to wait 3 seconds before next ranged attack
            if (enemy.rangeAttack)
            {
                enemy.rangeAttack = false;
                enemy.RangedAttack();
            }
        }
    }

    // GetPlayerDistance returns distance to Player object
    private float GetPlayerDistance()
    {
        return Vector3.Distance(enemy.transform.position, enemy.target.position);
    }
}
