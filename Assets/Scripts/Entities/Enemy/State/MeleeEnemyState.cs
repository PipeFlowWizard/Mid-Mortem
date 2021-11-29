using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyState : State
{
    
    // ChaseEnemyState takes an Enenmy Object in constructor
    public MeleeEnemyState(Enemy enemy,EnemyStateMachine stateMachine) : base(enemy,stateMachine)
    {

    }

    // ChaseEnemyState can perform different actions based on distance to Enemy and maxHealth
    public override void Action()
    {
        // Melee Attack is handled by EnemyHurtBox.cs
        enemy.Combat.MeleeAttack();
        Decision();
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        enemy.VFX.ChangeColor();
        enemy.Combat.MeleeAttack();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        enemy.VFX.ChangeColor();
    }

    // If Enemy is in MELEE state, can change to IDLE if Player no longer in scene or outside detectionRange, or
    // CHASE if Player is within detectionRange but outside Melee Range of 1, or
    // RUN if Enemy maxHealth falls below 25%
    public override void Decision()
    {
        base.Decision();
        
        // If Player is no longer in scene, or outside detectionRange, then switch to IDLE
        if(enemy.target == null || _stateMachine.GetPlayerDistance() > enemy.entityStats.detectionRange)
        {
            _stateMachine.SetState(_stateMachine.IdleState);
            
        }
        // If Player is outside Melee Range (1) but within detectionRange, then switch to CHASE
        else if (_stateMachine.GetPlayerDistance() > 2 && _stateMachine.GetPlayerDistance() <= enemy.entityStats.detectionRange)
        {
            _stateMachine.SetState(_stateMachine.ChaseState);
            
        }
        // Else, if Enemy has less than 25% maxHealth and Player in Scene (within Enemy meleeRange), it runs away from Player
        /*else if (_stateMachine.GetPlayerDistance() <= enemy.entityStats.meleeRange && enemy.CurrentHealthState() >= 2)
        {
            _stateMachine.SetState(_stateMachine.RunState);
            
        }*/
        
    }
}
