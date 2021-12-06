using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEnemyState : State
{
    private float enterTime = 0;

    private float stunDuration = 0;
    // ChaseEnemyState takes an Enenmy Object in constructor
    public StunEnemyState(Enemy enemy,EnemyStateMachine stateMachine) : base(enemy,stateMachine)
    {
    }

    // ChaseEnemyState can perform different actions based on distance to Enemy and maxHealth
    public override void Action()
    {
        Decision();
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        enterTime = Time.time;
        enemy.Movement.NavMeshAgent.enabled = false;

    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        enemy.Movement.NavMeshAgent.enabled = true;
    }

    // If Enemy is in MELEE state, can change to IDLE if Player no longer in scene or outside detectionRange, or
    // CHASE if Player is within detectionRange but outside Melee Range of 1, or
    // RUN if Enemy maxHealth falls below 25%
    public override void Decision()
    {
        base.Decision();
        if (enemy.CurrentHealthState() == 3)
        {
            _stateMachine.SetState(_stateMachine.DeadState);
        }
        if (Time.time - enterTime > stunDuration)
        {
            _stateMachine.SetState(_stateMachine.ChaseState);
        }
    }

    public StunEnemyState SetDuration(float duration)
    {
        stunDuration = duration;
        return this;
    }
}
