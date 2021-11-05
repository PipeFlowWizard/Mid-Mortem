using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReapBossState : State
{
    // ReapBossState takes a Boss Object in constructor
    public ReapBossState(Boss boss) : base(boss)
    {

    }

    // On Entering this State, Boss should stop moving
    public override void OnStateEnter()
    {
        boss.StopBoss();
        boss.waitingForReap = true;
        boss.ReapBossTimer();
    }

    // RangedBossState can perform different actions based on distance to Player and health
    public override void Action()
    {
        // If canReap is true, then Boss does nothing and faces Boss
        boss.TurnBoss();
        // If Boss health is 0, then Boss State is DeadBossState
        // CurrentHealthState returns 3 when health is 0
        if (boss.CurrentHealthState() == 3)
        {
            boss.SetState(new DeadBossState(boss));
        }
        else if (!boss.canReap)
        {
            // Get Player distance
            float distance = GetPlayerDistance();
            // If target of Boss is null or distance > max_range, then set Boss State to IdleBossState
            if (boss.target == null || distance >= boss.characterStats.max_range)
            {
                boss.SetState(new IdleBossState(boss));
            }
            // If distance to Boss is greater than chase_range but still less then max_range, then Boss State is RangedBossState
            else if (distance > boss.characterStats.chase_range)
            {
                boss.SetState(new RangedBossState(boss));
            }
            // If distance to Boss is less than or equal to chase_range, then Boss State is MeleeBossState
            else if (distance <= boss.characterStats.chase_range)
            {
                boss.SetState(new MeleeBossState(boss));
            }
        }
    }

    // GetPlayerDistance returns distance to Player object
    private float GetPlayerDistance()
    {
        return Vector3.Distance(boss.transform.position, boss.target.position);
    }
}
