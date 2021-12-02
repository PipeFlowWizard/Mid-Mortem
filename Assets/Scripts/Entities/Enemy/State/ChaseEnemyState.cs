using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemyState : State
{
    
    // ChaseEnemyState takes an Enenmy Object in constructor
    public ChaseEnemyState(Enemy enemy,EnemyStateMachine stateMachine) : base(enemy,stateMachine)
    {

    }

    // ChaseEnemyState can perform different actions based on distance to Enemy and maxHealth
    public override void Action()
    {
        if(enemy.target != null)
        {
            int randomNumber = UnityEngine.Random.Range(1, 1001);
            // If randomNumber is between 2 and 4 then Enemy can try and Dash to Player, if Enemy is SPEED
            if ((randomNumber <= 10 && enemy.entityStats.entityType == EntityStats.EntityType.SPEED && !enemy.isBossEnemy))
            {
                enemy.Movement.TurnEnemy(enemy.target.position);
                enemy.Movement.TestDash(enemy.target.position);
            }
            else
            {
                enemy.Movement.MoveEnemy(enemy.target.position);
            }
        }
        Decision();
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    // If Enemy is in CHASE state, can change to IDLE if Player no longer in Scene or,
    // RANGE_ATTACK if Player is outside meleeRange and a Boss
    // RUN if Enemy maxHealth falls below 25%
    public override void Decision()
    {
        base.Decision();
        
        // If Player no longer in scene, then switch to IDLE state
        if (enemy.target == null)
        {
            _stateMachine.SetState(_stateMachine.IdleState);
        }
        // Melee attack if Player is right in front of Enemy (in range of 1)
        else if (_stateMachine.GetPlayerDistance() < enemy.entityStats.meleeRange)
        {
            _stateMachine.SetState(_stateMachine.MeleeState);
        }
        // If Enemy is boss and Player is within detectionRange but not meleeRange, the Boss attacks from Range
        else if (_stateMachine.GetPlayerDistance() > enemy.entityStats.meleeRange && _stateMachine.GetPlayerDistance() <= enemy.entityStats.detectionRange)
        {
            _stateMachine.SetState(_stateMachine.RangedState);
        }
    }
}
