using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyStateController is responsible for switching between Enemy States
public class EnemyStateController
{
    // Reference to Enemy
    private Enemy enemy;
    // Different States of Enemy
    public enum EnemyState 
    {
        IDLE,
        RANGE_ATTACK,
        CHASE,
        RUN,
        REAP,
        DEAD,
        MELEE
    };
    // Current State of Enemy
    private EnemyState enemyState;

    public EnemyStateController(Enemy enemy)
    {
        this.enemy = enemy;
        // Enemies start out in IdleEnemyState
        enemy.SetState(new IdleEnemyState(enemy));
        enemyState = EnemyState.IDLE;
    }

    // UpdateEnemyState handles any transitions between Enemy States
    public void UpdateEnemyState()
    {
        // No matter which State Enemy is in, they can always enter DeadEnemyState, or start ReapEnemyState
        // If Enemy Health is 0, then Enemy is Dead
        if (enemy.CurrentHealthState() == 3 && !enemy.isDead)
        {
            enemy.Combat.KillEnemy();
            enemyState = EnemyState.DEAD;
        }
        // If Enemy health is below 25% and it can be reaped, then Enemy is Reapable
        else if(enemy.CurrentHealthState() == 2 && enemy.canReap && !enemy.waitingForReap)
        {
            enemy.Movement.StopEnemy();
            enemy.waitingForReap = true;
            enemy.Combat.ReapEnemyTimer();
            enemyState = EnemyState.REAP;
        }
        // Else, the Enemy behavior depends on its current state
        
        // If Enemy is in IDLE state, it can switch to CHASE state to go after Player (if SPEED or ATTACK type Enemy), or
        // RANGE_ATTACK to attack from Range (if DEFENSE or ATTACK type Enemy), or
        // RUN state to escape Player
        else if(enemyState == EnemyState.IDLE && enemy.target != null)
        {
            // ATTACK type Enemy has a 50/50 chance of attacks from RANGE or CHASING
            int randomNumber = UnityEngine.Random.Range(1, 3);
            // If randomNumber is 1, and Enemy is ATTACK, or if Enemy is SPEED, then they chase Player
            // Boss Enemies can't chase after Player
            if ((randomNumber == 1 && enemy.entityStats.entityType == EntityStats.EntityType.ATTACK) || enemy.entityStats.entityType == EntityStats.EntityType.SPEED && !enemy.isBossEnemy)
            {
                // If Player is in Scene (within maxRange), and Enemy still has more than 25% health, it chases after Player
                if (GetPlayerDistance() <= enemy.entityStats.maxRange && enemy.CurrentHealthState() < 2)
                {
                    enemy.SetState(new ChaseEnemyState(enemy));
                    enemyState = EnemyState.CHASE;
                }
                // Else, if Enemy has less than 25% health and Player in Scene (within Enemy chaseRange), it runs away from Player, if not boss
                else if (GetPlayerDistance() <= enemy.entityStats.chaseRange && enemy.CurrentHealthState() >= 2)
                {
                    enemy.SetState(new RunEnemyState(enemy));
                    enemyState = EnemyState.RUN;
                }
            }
            // If randomNumber is 2, and Enemy is ATTACK, or if Enemy is DEFENSE or BossEnemy, then they attack from Range
            else if ((randomNumber == 2 && enemy.entityStats.entityType == EntityStats.EntityType.ATTACK) || enemy.entityStats.entityType == EntityStats.EntityType.DEFENSE || enemy.isBossEnemy)
            {
                // If Player is in Scene (within maxRange), and Enemy still has more than 25% health, it attacks Player from Range
                if (GetPlayerDistance() <= enemy.entityStats.maxRange && enemy.CurrentHealthState() < 2)
                { 
                    enemy.SetState(new RangedEnemyState(enemy));
                    enemyState = EnemyState.RANGE_ATTACK;
                }
                // Else, if Enemy has less than 25% health and Player in Scene (within Enemy chaseRange), it runs away from Player
                else if (GetPlayerDistance() <= enemy.entityStats.chaseRange && enemy.CurrentHealthState() >= 2 && !enemy.isBossEnemy)
                {
                    enemy.SetState(new RunEnemyState(enemy));
                    enemyState = EnemyState.RUN;
                }
            }
        }
        
        // If Enemy in RANGE_ATTACK state, can change to IDLE if Player no longer in Scene or further than maxRange or,
        // RUN if Enemy health falls below 25%
        // Boss Enemies can switch to Chase Player if they are within chaseRange
        else if(enemyState == EnemyState.RANGE_ATTACK)
        {
            // If Player no longer in scene, or further than maxRange then switch to IDLE state
            if (enemy.target == null || GetPlayerDistance() > enemy.entityStats.maxRange)
            {
                enemy.SetState(new IdleEnemyState(enemy));
                enemyState = EnemyState.IDLE;
            }
            // If Enemy is boss and Player is within chaseRange, the Boss attacks
            else if (GetPlayerDistance() <= enemy.entityStats.chaseRange && enemy.CurrentHealthState() < 2 && enemy.isBossEnemy)
            {
                enemy.SetState(new ChaseEnemyState(enemy));
                enemyState = EnemyState.CHASE;
            }
            // If Enemy health is less than 25% it runs away from Player
            else if(GetPlayerDistance() <= enemy.entityStats.chaseRange && enemy.CurrentHealthState() >= 2 && !enemy.isBossEnemy)
            {
                enemy.SetState(new RunEnemyState(enemy));
                enemyState = EnemyState.RUN;
            }
        }
        // If Enemy is in CHASE state, can change to IDLE if Player no longer in Scene or,
        // RANGE_ATTACK if Player is outside chaseRange and a Boss
        // RUN if Enemy health falls below 25%
        else if (enemyState == EnemyState.CHASE)
        {
            // If Player no longer in scene, then switch to IDLE state
            if (enemy.target == null)
            {
                enemy.SetState(new IdleEnemyState(enemy));
                enemyState = EnemyState.IDLE;
            }
            // Melee attack if Player is right in front of Enemy (in range of 1)
            else if (GetPlayerDistance() < 1)
            {
                enemy.SetState(new MeleeEnemyState(enemy));
                enemyState = EnemyState.MELEE;
            }
            // If Enemy is boss and Player is within maxRange but not chaseRange, the Boss attacks from Range
            else if (GetPlayerDistance() > enemy.entityStats.chaseRange && GetPlayerDistance() <= enemy.entityStats.maxRange && enemy.CurrentHealthState() < 2 && enemy.isBossEnemy)
            {
                enemy.SetState(new RangedEnemyState(enemy));
                enemyState = EnemyState.RANGE_ATTACK;
            }
            // If Enemy health is less than 25% it runs away from Player, if not performing suicide run
            else if (enemy.CurrentHealthState() >= 2 && !enemy.isBossEnemy && !enemy.Movement.suicide)
            {
                enemy.SetState(new RunEnemyState(enemy));
                enemyState = EnemyState.RUN;
            }
        }
        // If Enemy is in REAP state and no longer waiting for reap, can change to
        // RUN state when Player within chaseRange and not Boss enemy, or
        // RANGE_ATTACK if Enemy is Boss and Player further than chaseRange, or
        // CHASE if Enemy is Boss and Player is within chaseRange
        // IDLE state if Player no longer in scene, or Player is outside chaseRange
        else if (enemyState == EnemyState.REAP && !enemy.waitingForReap)
        {
            // If Player no longer in scene or outisde chaseRange, switch to IDLE
            if(enemy.target == null || (!enemy.isBossEnemy && GetPlayerDistance() > enemy.entityStats.chaseRange) || (enemy.isBossEnemy && GetPlayerDistance() > enemy.entityStats.maxRange))
            {
                enemy.SetState(new IdleEnemyState(enemy));
                enemyState = EnemyState.IDLE;
            }
            // If Enemy is boss and Player is within chaseRange, the Boss attacks
            else if (GetPlayerDistance() <= enemy.entityStats.chaseRange && enemy.isBossEnemy)
            {
                enemy.SetState(new ChaseEnemyState(enemy));
                enemyState = EnemyState.CHASE;
            }
            // If Enemy is boss and Player is within maxRange but not chaseRange, the Boss attacks from Range
            else if (GetPlayerDistance() > enemy.entityStats.chaseRange && GetPlayerDistance() <= enemy.entityStats.maxRange && enemy.isBossEnemy)
            {
                enemy.SetState(new RangedEnemyState(enemy));
                enemyState = EnemyState.RANGE_ATTACK;
            }
            // If Player still in scene and within chaseRange, it runs away
            else if(GetPlayerDistance() <= enemy.entityStats.chaseRange && !enemy.isBossEnemy)
            {
                enemy.SetState(new RunEnemyState(enemy));
                enemyState = EnemyState.RUN;
            }
        }
        // If Enemy is in RUN state, can change to IDLE state if Player no longer in scene, or outisde chaseRange, or
        // CHASE if Enemy perfoms suicide run
        else if(enemyState == EnemyState.RUN)
        {
            // If Player no longer in scene or outside chaseRange, switch to IDLE
            if(enemy.target == null || GetPlayerDistance() > enemy.entityStats.chaseRange)
            {
                enemy.SetState(new IdleEnemyState(enemy));
                enemyState = EnemyState.IDLE;
            }
            else if (enemy.Movement.suicide)
            {
                enemy.SetState(new ChaseEnemyState(enemy));
                enemyState = EnemyState.CHASE;
            }
        }
        // If Enemy is in MELEE state, can change to IDLE if Player no longer in scene or outside maxRange, or
        // CHASE if Player is within maxRange but outside Melee Range of 1, or
        // RUN if Enemy health falls below 25%
        else if (enemyState == EnemyState.MELEE)
        {
            // If Player is no longer in scene, or outside maxRange, then switch to IDLE
            if(enemy.target == null || GetPlayerDistance() > enemy.entityStats.maxRange)
            {
                enemy.SetState(new IdleEnemyState(enemy));
                enemyState = EnemyState.IDLE;
            }
            // If Player is outside Melee Range (1) but within maxRange, then switch to CHASE
            else if (GetPlayerDistance() > 1 && GetPlayerDistance() <= enemy.entityStats.maxRange)
            {
                enemy.SetState(new ChaseEnemyState(enemy));
                enemyState = EnemyState.CHASE;
            }
            // Else, if Enemy has less than 25% health and Player in Scene (within Enemy chaseRange), it runs away from Player
            else if (GetPlayerDistance() <= enemy.entityStats.chaseRange && enemy.CurrentHealthState() >= 2)
            {
                enemy.SetState(new RunEnemyState(enemy));
                enemyState = EnemyState.RUN;
            }
        }
    }

    // GetPlayerDistance returns distance to Player object
    private float GetPlayerDistance()
    {
        return Vector3.Distance(enemy.transform.position, enemy.target.position);
    }
}
