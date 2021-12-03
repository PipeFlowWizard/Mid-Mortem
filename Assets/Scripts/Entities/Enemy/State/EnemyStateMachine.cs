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

    public State DeadState => _deadState;

    public State ReapState => _reapState;

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
        _reapState = new ReapEnemyState(enemy,this);
        _currentState = _idleState;
    }

    // Tick handles any transitions between Enemy States
    public void Tick()
    {
        //Debug.Log(_currentState.GetType());
        _currentState.Action();
        // No matter which State Enemy is in, they can always enter DeadEnemyState, or start ReapEnemyState
        // If Enemy Health is 0, then Enemy is Dead
        // If Enemy maxHealth is below 25% and it can be reaped, then Enemy is Reapable
        if(enemy.CurrentHealthState() == 2 && enemy.canReap && !enemy.waitingForReap)
        {
            enemy.Movement.StopEnemy();
            enemy.waitingForReap = true;
            enemy.ReapEnemyTimer();
        }
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
