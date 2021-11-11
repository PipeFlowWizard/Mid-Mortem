using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    
    public void OnMeleeInput()
    {
        _player.Combat.MeleeAttack();
    }

    public void OnMeleeChargeInput()
    {
        _player.Combat.ChargedMeleeAttack();
    }

    public void OnRangedInput()
    {
        _player.Combat.Cast(_player.Movement.mesh.transform.forward, _player.entityStats.attack);
    }
    
    public void OnRangedChargeInput()
    {
    }

}
