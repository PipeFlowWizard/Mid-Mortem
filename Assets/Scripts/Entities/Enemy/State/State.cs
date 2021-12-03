using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    // Reference to Enemy Object for Enemy states
    protected Enemy enemy;
    protected EnemyStateMachine _stateMachine;
    private float stateEnterTime = 0;

    // Action() will make the referenced GameObject perform the desired
    // action based on State
    public abstract void Action();

    // OnStateEnter is called when GameObject enters new State
    public virtual void OnStateEnter()
    {
        // Debug.Log(this.GetType());
        stateEnterTime = Time.time;
    }

    // OnStateExit is called when GameObject exits current State
    public virtual void OnStateExit() { }

    // State constructor takes Enemy reference
    public State(Enemy enemy, EnemyStateMachine stateMachine)
    {
        this.enemy = enemy;
        this._stateMachine = stateMachine;
    }

    public bool HasTimeSinceEnteredPassed(float seconds)
    {
        return (Time.time - stateEnterTime) > seconds;
    }
    public virtual void Decision() { }
    
    
}
