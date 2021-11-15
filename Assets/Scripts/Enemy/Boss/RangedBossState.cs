using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossState : State
{
    // RangedBossState takes a Boss Object in constructor
    public RangedBossState(Boss boss) : base(boss)
    {

    }

    // On Entering this State, boss Stops moving then turns
    public override void OnStateEnter()
    {
        boss.StopBoss();
    }

    // On Exiting this State, rangeAttack is set to false
    public override void OnStateExit()
    {
        boss.rangeAttack = false;
    }

    // RangedBossState can perform different actions based on distance to Player and health
    public override void Action()
    {
        // Get Player distance
        float distance = GetPlayerDistance();
        // If Boss health is 0, then Boss State is DeadBossState
        // CurrentHealthState returns 3 when health is 0
        if (boss.CurrentHealthState() == 3)
        {
            boss.SetState(new DeadBossState(boss));
        }
        // If Boss health is 25% or below and canReap , then Boss State is ReapBossState
        // CurrentHealthState returns 2 when Boss health is below or equal to 25% of original
        else if (boss.CurrentHealthState() == 2 && boss.canReap)
        {
            boss.SetState(new ReapBossState(boss));
        }
        // If target of Boss is null or distance > max_range, then set Boss State to IdleBossState
        else if (boss.target == null || distance >= boss.entityStats.maxRange)
        {
            boss.SetState(new IdleBossState(boss));
        }
        // If distance to Boss is less than or equal to chase_range, then Boss State is MeleeBossState
        else if (distance <= boss.entityStats.chaseRange)
        {
            boss.SetState(new MeleeBossState(boss));
        }
        // Else
        else
        {
            // Turn Boss toward Player
            boss.TurnBoss();
            // If attack is true, then can call RangeAttack and Start Coroutine AttackTimer
            // to wait 3 seconds before next ranged attack
            if (boss.rangeAttack)
            {
                boss.rangeAttack = false;
                boss.RangedAttack();
            }
        }
    }

    // GetPlayerDistance returns distance to Player object
    private float GetPlayerDistance()
    {
        return Vector3.Distance(boss.transform.position, boss.target.position);
    }
}
