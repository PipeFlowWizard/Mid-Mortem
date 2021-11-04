using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBossState : State
{
    // IdleBossState takes a Boss Object in constructor
    public IdleBossState(Boss boss) : base(boss)
    {

    }

    // Boss performs no Action while in Idle state, except looking for Player
    public override void Action()
    {
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
        // If Player is in same scene as boss, boss switches State
        else if (boss.target != null)
        {
            // Get boss distance from Player
            float distance = GetPlayerDistance();
            // If ditance is less than max_range and greater than chase_range, set boss state to RangedBossState
            if (distance < boss.characterStats.max_range && distance > boss.characterStats.chase_range)
            {
                boss.SetState(new RangedBossState(boss));
            }
            // Else if distance is less than chase_range, them set boss State to MeleeBossState
            else if (distance <= boss.characterStats.chase_range)
            {
                boss.SetState(new MeleeBossState(boss));
            }
        }
        // Else, just keep searching for Player
        else
        {
            boss.GetPlayer();
        }
    }

    // OnStateEnter Stop boss Movement
    public override void OnStateEnter()
    {
        boss.StopBoss();
    }

    // GetPlayerDistance returns distance to Player object
    private float GetPlayerDistance()
    {
        return Vector3.Distance(boss.transform.position, boss.target.position);
    }
}
