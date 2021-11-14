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
        // If Enemy is not Boss, it Patrols around room
        if (!enemy.isBossEnemy)
        {
            enemy.Movement.PatrolEnemy();
            enemy.GetPlayer();
        }
    }

    // OnStateEnter Stop Enemy Movement
    public override void OnStateExit()
    {
        enemy.Movement.patrolPointSet = false;
    }
}
