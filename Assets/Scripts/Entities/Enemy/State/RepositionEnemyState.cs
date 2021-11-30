using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionEnemyState : State
{
    private Vector3 runAwayPoint;                           // Point Enemy is moving towards while running away from Player
    private bool runAwayPointSet;                           // Determines if RunAway point is set
    public float runAwayDistance;                           // How far away from meleeRange enemy should be
    // PatrolEnemyState takes an Enenmy Object in constructor
    
    public RepositionEnemyState(Enemy enemy,EnemyStateMachine stateMachine) : base(enemy,stateMachine)
    {
        runAwayDistance = 5.0f;
    }

    // Enemy runs away from Player
    public override void Action()
    {
        RunAway();
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
    
    // RunAway Moves Enemy away from Player
    public void RunAway()
    {
        // If Run Away Point not set, then search for a RunAwayPoint
        if (!runAwayPointSet)
        {
            SearchRunAwayPoint();
        }

        // If runAwayPointSet is now true, then use NavMeshAgent to move towards runAwayPoint
        if (runAwayPointSet && !enemy.isDead && !enemy.waitingForReap)
        {
            enemy.Movement.TurnEnemy(runAwayPoint);
            enemy.Movement.MoveEnemy(runAwayPoint);
        }
        // Get distance to runAwayPoint
        Vector3 distanceToPatrolPoint = enemy.transform.position - runAwayPoint;
        // Once the runAwayPoint is reached, runAwayPointSet is false
        if (distanceToPatrolPoint.magnitude < 0.5f)
        {
            runAwayPointSet = false;
        }
    }

    // Function to check for a Run Away Point
    public void SearchRunAwayPoint()
    {
        // Get Distance between Enemy and Player
        float distance = Vector3.Distance(enemy.transform.position, enemy.target.position);
        // Get difference of detectionRange and Enemy-Player distance
        float difference = (enemy.entityStats.meleeRange + runAwayDistance) - distance;
        // Get vector pointing towards Player
        Vector3 direction = -1 * (enemy.target.position - enemy.transform.position);
        direction.y = 0;
        direction = direction.normalized;
        // Set new RunAwayPoint away from Enemy
        runAwayPoint = enemy.transform.position + (direction * difference);
        // If runAwayPoint still on ground then runAwayPointSet is true
        if (Physics.Raycast(runAwayPoint + Vector3.up, Vector3.down, 3.0f, enemy.Movement.groundLayer))
        {
            // If it is, then patrolPointSet is true
            runAwayPointSet = true;
        }
        // Else, Enemy performs suicide run to Player
        else
        {
            enemy.Movement.suicide = true;
        }
    }

    // If Enemy is in RUN state, can change to IDLE state if Player no longer in scene, or outisde meleeRange, or
    // CHASE if Enemy perfoms suicide run
    public override void Decision()
    {
        base.Decision();
        // If Player no longer in scene or outside meleeRange, switch to IDLE
        if(enemy.target == null || _stateMachine.GetPlayerDistance() > enemy.entityStats.meleeRange)
        {
            _stateMachine.SetState(_stateMachine.IdleState);
           
        }
        else if (enemy.Movement.suicide)
        {
            _stateMachine.SetState(_stateMachine.ChaseState);
        }
    
    }
}