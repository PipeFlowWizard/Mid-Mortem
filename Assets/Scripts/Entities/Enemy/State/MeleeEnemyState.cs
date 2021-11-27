using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyState : State
{
    
    // ChaseEnemyState takes an Enenmy Object in constructor
    public MeleeEnemyState(Enemy enemy) : base(enemy)
    {

    }

    // ChaseEnemyState can perform different actions based on distance to Enemy and health
    public override void Action()
    {
        // Melee Attack is handled by EnemyHurtBox.cs
        enemy.VFX.MeleeAttack();
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        enemy.VFX.ChangeColor();
        enemy.VFX.MeleeAttack();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        enemy.VFX.ChangeColor();
    }
}
