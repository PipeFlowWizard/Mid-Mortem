using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyState : State
{
    private Vector3 patrolPoint = Vector3.zero; // Point Enemy is moving towards while Patrolling
    public bool patrolPointSet = false;         // Determines if Patrol Point is set for Enemy
    public float patrolPointRange;              // Range in which Enemy patrols around
    
    // PatrolEnemyState takes an Enenmy Object in constructor
    public PatrolEnemyState(Enemy enemy,EnemyStateMachine stateMachine) : base(enemy,stateMachine)
    {
        patrolPointRange = 10.0f;
        patrolPointSet = false;
    }

    // Enemy performs no Action while in Idle state, except looking for Player
    public override void Action()
    {
        // If Enemy is not Boss, it Patrols around room
        if (!enemy.isBossEnemy)
        {
            PatrolEnemy();
            enemy.GetPlayer();
        }
        Decision();
    }

    // OnStateEnter Stop Enemy Movement
    public override void OnStateExit()
    {
        base.OnStateExit();
    }
    
    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }
    
    // Function for Enemy patrolling
    public void PatrolEnemy()
    {
        // If Patrol Point not set, then search for a patrol point
        if (!patrolPointSet)
        {
            SearchPatrolPoint();
        }

        // If patrolPointSet is now true, then use NavMeshAgent to move towards patrolPoint
        if (patrolPointSet && !enemy.isDead && !enemy.waitingForReap)
        {
            enemy.Movement.TurnEnemy(patrolPoint);
            enemy.Movement._navMeshAgent.SetDestination(patrolPoint);
        }
        // Get distance to patrolPoint
        Vector3 distanceToPatrolPoint = enemy.transform.position - patrolPoint;
        // Once the patrolPoint is reached, patrolPointSet is false
        if(distanceToPatrolPoint.magnitude < 0.5f)
        {
            patrolPointSet = false;
        }
    }

    // Function to check for a Patrol Point
    public void SearchPatrolPoint()
    {
        // Calculate random patrol point in patrolPointRange
        float randomZ = Random.Range(-patrolPointRange, patrolPointRange);
        float randomX = Random.Range(-patrolPointRange, patrolPointRange);
        // Set new Patrol Point
        patrolPoint = new Vector3(enemy.transform.position.x + randomX, enemy.transform.position.y, enemy.transform.position.z + randomZ);
        // Check that patrolPoint still on ground
        if (Physics.Raycast(patrolPoint + Vector3.up, Vector3.down, 3.0f, enemy.Movement.groundLayer))
        {
            // If it is, then patrolPointSet is true
            patrolPointSet = true;
        }
    }

    // Else, the Enemy behavior depends on its current state
    // If Enemy is in IDLE state, it can switch to CHASE state to go after Player (if SPEED or ATTACK type Enemy), or
    // RANGE_ATTACK to attackDamage from Range (if DEFENSE or ATTACK type Enemy), or
    // RUN state to escape Player
    public override void Decision()
    {
        base.Decision();
        if( enemy.target != null)
        {
            // ATTACK type Enemy has a 50/50 chance of attacks from RANGE or CHASING
            int randomNumber = UnityEngine.Random.Range(1, 3);
            // If randomNumber is 1, and Enemy is ATTACK, or if Enemy is SPEED, then they chase Player
            // Boss Enemies can't chase after Player
            if ((randomNumber == 1 && enemy.entityStats.entityType == EntityStats.EntityType.ATTACK) || enemy.entityStats.entityType == EntityStats.EntityType.SPEED && !enemy.isBossEnemy)
            {
                // If Player is in Scene (within detectionRange), and Enemy still has more than 25% maxHealth, it chases after Player
                if (_stateMachine.GetPlayerDistance() <= enemy.entityStats.detectionRange && enemy.CurrentHealthState() < 2)
                {
                    _stateMachine.SetState(_stateMachine.ChaseState);
                }
                // Else, if Enemy has less than 25% maxHealth and Player in Scene (within Enemy meleeRange), it runs away from Player, if not boss
                /*else if (_stateMachine.GetPlayerDistance() <= enemy.entityStats.meleeRange && enemy.CurrentHealthState() >= 2)
                {
                    _stateMachine.SetState(_stateMachine.RunState);
                }*/
            }
            // If randomNumber is 2, and Enemy is ATTACK, or if Enemy is DEFENSE or BossEnemy, then they attackDamage from Range
            else if ((randomNumber == 2 && enemy.entityStats.entityType == EntityStats.EntityType.ATTACK) || enemy.entityStats.entityType == EntityStats.EntityType.DEFENSE || enemy.isBossEnemy)
            {
                // If Player is in Scene (within detectionRange), and Enemy still has more than 25% maxHealth, it attacks Player from Range
                if (_stateMachine.GetPlayerDistance() <= enemy.entityStats.detectionRange && enemy.CurrentHealthState() < 2)
                { 
                    _stateMachine.SetState(_stateMachine.RangedState);
                }
                // Else, if Enemy has less than 25% maxHealth and Player in Scene (within Enemy meleeRange), it runs away from Player
                /*else if (_stateMachine.GetPlayerDistance() <= enemy.entityStats.meleeRange && enemy.CurrentHealthState() >= 2 && !enemy.isBossEnemy)
                {
                    _stateMachine.SetState(_stateMachine.RunState);
                }*/
            }
        }
    }
}
