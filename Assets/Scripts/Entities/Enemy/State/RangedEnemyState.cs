using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// RangedEnemyState is when Enemies fire ranged attacks
public class RangedEnemyState : State
{ 
    // RangedEnemyState takes an Enenmy Object in constructor
    public RangedEnemyState(Enemy enemy,EnemyStateMachine stateMachine) : base(enemy,stateMachine)
    {

    }

    private float lastAttackTime = 0;
    // On Entering this State, Enemy Stops moving then turns
    public override void OnStateEnter()
    {
        enemy.Movement.StopEnemy();
    }

    // On Exiting this State, rangeAttack is set to false
    public override void OnStateExit()
    {
        enemy.Movement._navMeshAgent.enabled = true;
    }

    // RangedEnemyState can perform different actions based on distance to Enemy and maxHealth
    public override void Action()
    {
        // Turn enemy toward Player
        if(enemy.target)
            enemy.Movement.TurnEnemy(enemy.target.position);
        
        // If Enemy is Boss, then it can use Boss Attacks
        if (enemy.isBossEnemy)
        {
            BossAttack();
        }
        // If Enemy is normal enemy, it can use abilities
        else
        {
            Debug.Log("Enemy Attack");
            EnemyAttack();
        }
        Decision();
    }

    private void EnemyAttack()
    {
        int randomNumber = UnityEngine.Random.Range(1, 11);
        if (Time.time - lastAttackTime < 1f/enemy.entityStats.rangedAttackSpeed)
        {
            return;
        }
        else
        {
            // If randomNumber is between 1 & 5 and Enemy is DEFENSE, then use Invincible
            if (randomNumber >= 1 && randomNumber <= 5)
            {
                if (enemy.entityStats.entityType == EntityStats.EntityType.DEFENSE)
                {
                    enemy.Combat.Invincible();
                }
                else if (enemy.entityStats.entityType == EntityStats.EntityType.ATTACK)
                {
                    enemy.Combat.TripleRangedAttack();
                }
            }
            else
            {
                enemy.Combat.RangedAttack();
            }

            lastAttackTime = Time.time;
        }
    }
    
    private void BossAttack()
    {
        int randomNumber = UnityEngine.Random.Range(1, 11);
        if (Time.time - lastAttackTime < 1f/enemy.entityStats.rangedAttackSpeed)
        {
            return;
        }
    
        // If randomNumber is between 1 & 8 and current level is greater than or equal to 1, then Boss can use SuperRangedAttack
        if (randomNumber >= 2 && randomNumber <= 3 && enemy.currentLevel >= 1)
        {
            Debug.Log("X attack");
            enemy.Combat.SuperRangedAttack();
            
        }
        //If randomNumber is between 9 and 16 and current level is greater than or equal to 2, then Boss can use MeteorFall
        else if (randomNumber >= 4 && randomNumber <= 6 && enemy.currentLevel >= 2)
        {
            enemy.Combat.MeteorFall();
            Debug.Log("Meteor");
        }
        // If randomNumer is between 17 and 24 and current level is greater than or equal to 3, then Boss can use HeatSeeker
        else if (randomNumber >= 7 && randomNumber <= 9 && enemy.currentLevel >= 3)
        {
            enemy.Combat.HeatSeeker();
            Debug.Log("Heat");
        }
        // Else, just normal Ranged Attack
        else
        {
            enemy.Combat.RangedAttack();
            Debug.Log("normal");
        }
        

        lastAttackTime = Time.time;
    
    }

    public override void Decision()
    {
        
        base.Decision();
        if (enemy.CurrentHealthState() == 3)
        {
            _stateMachine.SetState(_stateMachine.DeadState);
        }
        // If Player no longer in scene, or further than detectionRange then switch to IDLE state
        if (enemy.target == null || _stateMachine.GetPlayerDistance() > enemy.entityStats.detectionRange)
        {
            _stateMachine.SetState(_stateMachine.IdleState);
        }
        // If Enemy is boss and Player is within meleeRange, the Boss attacks
        else if (_stateMachine.GetPlayerDistance() <= enemy.entityStats.meleeRange)
        {
            _stateMachine.SetState(_stateMachine.ChaseState);
        }
    }
}
