using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    // Reference to Enemy Object for Enemy states
    protected Enemy enemy;
    // Reference to Player GameObject for Player states

    // Action() will make the referenced GameObject perform the desired
    // action based on State
    public abstract void Action();

    // OnStateEnter is called when GameObject enters new State
    public virtual void OnStateEnter() { }

    // OnStateExit is called when GameObject exits current State
    public virtual void OnStateExit() { }

    // State constructor takes Enemy reference
    public State(Enemy enemy)
    {
        this.enemy = enemy;
    }
}
