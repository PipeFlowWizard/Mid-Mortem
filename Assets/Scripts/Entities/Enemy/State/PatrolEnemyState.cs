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
        Decision();
        // If Enemy is not Boss, it Patrols around room
        if (!enemy.isBossEnemy)
        {
            PatrolEnemy();
            enemy.GetPlayer();
        }
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
        if (patrolPointSet && !enemy.waitingForReap)
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
        if (enemy.CurrentHealthState() == 3)
        {
            _stateMachine.SetState(_stateMachine.DeadState);
        }
        if( enemy.target != null)
        {
            int randomNumber = UnityEngine.Random.Range(1, 3);
            if (enemy.isBossEnemy)
            {
                if (_stateMachine.GetPlayerDistance() <= enemy.entityStats.detectionRange && enemy.CurrentHealthState() < 2)
                { 
                    _stateMachine.SetState(_stateMachine.RangedState);
                }
            }
            else
            {
                if (enemy.entityStats.entityType == EntityStats.EntityType.ATTACK)
                {
                    if (randomNumber == 1)
                    {
                        if (_stateMachine.GetPlayerDistance() <= enemy.entityStats.detectionRange && enemy.CurrentHealthState() < 2)
                        {
                            _stateMachine.SetState(_stateMachine.ChaseState);
                        }
                    }

                    if (randomNumber == 2)
                    {
                        if (_stateMachine.GetPlayerDistance() <= enemy.entityStats.detectionRange && enemy.CurrentHealthState() < 2)
                        { 
                            _stateMachine.SetState(_stateMachine.RangedState);
                        }
                    }
                    
                }
                else if (enemy.entityStats.entityType == EntityStats.EntityType.DEFENSE)
                {
                    if (_stateMachine.GetPlayerDistance() <= enemy.entityStats.detectionRange && enemy.CurrentHealthState() < 2)
                    { 
                        _stateMachine.SetState(_stateMachine.RangedState);
                    }
                }
                else if (enemy.entityStats.entityType == EntityStats.EntityType.SPEED)
                {
                    if (_stateMachine.GetPlayerDistance() <= enemy.entityStats.detectionRange && enemy.CurrentHealthState() < 2)
                    {
                        _stateMachine.SetState(_stateMachine.ChaseState);
                    }
                }    
            }
        }
    }
}
