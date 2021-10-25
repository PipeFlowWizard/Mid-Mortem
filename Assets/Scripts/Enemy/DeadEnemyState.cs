using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEnemyState : State
{
    // DeadEnemyState takes an Enenmy Object in constructor
    public DeadEnemyState(Enemy enemy) : base(enemy)
    {

    }

    // OnStateEnter call the KillEnemy function
    public override void OnStateEnter()
    {
        enemy.KillEnemy();
    }

    // Killed Enemy doesn't do anything on Action
    public override void Action()
    {
        
    }
}
