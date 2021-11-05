using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBossState : State
{
    // DeadBossState takes a Boss Object in constructor
    public DeadBossState(Boss boss) : base(boss)
    {

    }

    // OnStateEnter call the KillBoss function
    public override void OnStateEnter()
    {
        boss.KillBoss();
    }

    // Boss doesn't do anything on Action
    public override void Action()
    {

    }
}
