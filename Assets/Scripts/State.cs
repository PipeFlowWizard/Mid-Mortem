using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    // Reference to Enemy Object for Enemy states
    protected Enemy enemy;
    // Reference to Boss Object
    protected Boss boss;

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
        boss = null;
    }

    // State constructor can take a Boss reference as well
    public State(Boss boss)
    {
        this.boss = boss;
        enemy = null;
    }
}
