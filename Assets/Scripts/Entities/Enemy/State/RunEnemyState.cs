using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEnemyState : State
{
    // IdleEnemyState takes an Enenmy Object in constructor
    public RunEnemyState(Enemy enemy) : base(enemy)
    {

    }

    // Enemy runs away from Player
    public override void Action()
    {
        enemy.Movement.RunAway();
    }
}
