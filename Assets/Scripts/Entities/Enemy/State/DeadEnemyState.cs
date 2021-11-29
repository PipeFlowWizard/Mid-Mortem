using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEnemyState : State
{
   
    public DeadEnemyState(Enemy enemy,EnemyStateMachine stateMachine) : base(enemy,stateMachine)
    {
    }

    // Enemy runs away from Player
    public override void Action()
    {
        
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    
    public override void Decision()
    {
        base.Decision();
        
    }
}
