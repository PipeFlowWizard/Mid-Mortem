using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBossState : State
{
    // MeleeBossState takes a Boss Object in constructor
    public MeleeBossState(Boss boss) : base(boss)
    {

    }

    // MeleeBossState can perform different actions based on distance to Player and health
    public override void Action()
    {
        // Get Player distance
        float distance = GetPlayerDistance();
        // If boss health is 0, then boss State is DeadBossState
        // CurrentHealthState returns 3 when health is 0
        if (boss.CurrentHealthState() == 3)
        {
            boss.SetState(new DeadBossState(boss));
        }
        // If boss health is 25% or below and canReap , then boss State is ReapBossState
        // CurrentHealthState returns 2 when boss health is below or equal to 25% of original
        else if (boss.CurrentHealthState() == 2 && boss.canReap)
        {
            boss.SetState(new ReapBossState(boss));
        }
        // If target of boss is null or distance > max_range, then set boss State to IdleBossState
        else if (boss.target == null || distance >= boss.characterStats.max_range)
        {
            boss.SetState(new IdleBossState(boss));
        }
        // If distance to boss is greater than chase_range, then boss State is RangedBossState
        else if (distance > boss.characterStats.chase_range)
        {
            boss.SetState(new RangedBossState(boss));
        }
        // Else
        else
        {
            // MeleeBossState attacks Player at intervals
            boss.TurnBoss();
            if (boss.meleeAttack)
            {
                boss.MoveBoss();
            }
        }
    }

    // GetPlayerDistance returns distance to Player object
    private float GetPlayerDistance()
    {
        return Vector3.Distance(boss.transform.position, boss.target.position);
    }
}
