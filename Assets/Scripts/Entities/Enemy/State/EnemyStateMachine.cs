using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyStateMachine is responsible for switching between Enemy States
public class EnemyStateMachine
{
    // Reference to Enemy
    private Enemy enemy;
    private State _currentState;
    private State _idleState;
    private State _chaseState;
    private State _meleeState;
    private State _rangedState;
    private State _runState;
    private State _deadState;
    private State _reapState;
    private StunEnemyState _stunState;

    public State CurrentState => _currentState;

    public State IdleState => _idleState;

    public State ChaseState => _chaseState;

    public State MeleeState => _meleeState;

    public State RangedState => _rangedState;
    

    public StunEnemyState StunState => _stunState;

    public EnemyStateMachine(Enemy enemy)
    {
        this.enemy = enemy;
        // Enemies start out in PatrolEnemyState

        _idleState = new PatrolEnemyState(enemy,this);
        _chaseState = new ChaseEnemyState(enemy,this);
        _meleeState = new MeleeEnemyState(enemy,this);
        _rangedState = new RangedEnemyState(enemy,this);
        _stunState = new StunEnemyState(enemy,this);
        _deadState = new DeadEnemyState(enemy,this);
        _currentState = _idleState;
    }

    // Tick handles any transitions between Enemy States
    public void Tick()
    {
        // Debug.Log(_currentState.GetType());
        _currentState.Action();
        // No matter which State Enemy is in, they can always enter DeadEnemyState, or start ReapEnemyState
        // If Enemy Health is 0, then Enemy is Dead
        if (enemy.CurrentHealthState() == 3 && !enemy.isDead)
        {
            SetState(_deadState);
            enemy.KillEnemy();
        }
        // If Enemy maxHealth is below 25% and it can be reaped, then Enemy is Reapable
        else if(enemy.CurrentHealthState() == 2 && enemy.canReap && !enemy.waitingForReap)
        {
            enemy.Movement.StopEnemy();
            enemy.waitingForReap = true;
            enemy.ReapEnemyTimer();
        }
        /*else if (!enemy.waitingForReap)
        {
            // If Player no longer in scene or outisde meleeRange, switch to IDLE
            if(enemy.target == null || (!enemy.isBossEnemy && GetPlayerDistance() > enemy.entityStats.meleeRange) || (enemy.isBossEnemy && GetPlayerDistance() > enemy.entityStats.detectionRange))
            {
                SetState(_idleState);
                enemyState = EnemyState.IDLE;
            }
            // If Enemy is boss and Player is within meleeRange, the Boss attacks
            else if (GetPlayerDistance() <= enemy.entityStats.meleeRange && enemy.isBossEnemy)
            {
                SetState(_chaseState);
                enemyState = EnemyState.CHASE;
            }
            // If Enemy is boss and Player is within detectionRange but not meleeRange, the Boss attacks from Range
            else if (GetPlayerDistance() > enemy.entityStats.meleeRange && GetPlayerDistance() <= enemy.entityStats.detectionRange && enemy.isBossEnemy)
            {
                SetState(_rangedState);
                enemyState = EnemyState.RANGE_ATTACK;
            }
           
        }*/
    }

    // GetPlayerDistance returns distance to Player object
    public float GetPlayerDistance()
    {
        return Vector3.Distance(enemy.transform.position, enemy.target.position);
    }
    
    public void SetState(State state)
    {
        if (!CurrentState.HasTimeSinceEnteredPassed(1))
            return;
        
        //Debug.Log("Changing state");
        // If currentState is already assigned, then call OnStateExit for that State
        if (_currentState != null)
        {
            _currentState.OnStateExit();
        }

        // Set currentState to new state
        _currentState = state;
        // If currentState is now not null, call OnStateEnter for that State
        if (_currentState != null)
        {
            _currentState.OnStateEnter();
        }
    }
}
