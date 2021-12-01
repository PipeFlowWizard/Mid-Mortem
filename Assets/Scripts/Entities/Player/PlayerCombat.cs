using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Player _player;
    [SerializeField] private Spell spell;
    [SerializeField]  float spellCD = 100f;
    [SerializeField] private Transform shootOut;
    public Animator anim;
    
    private float _nextSpellTime = 0f;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    public void Cast(Vector3 spellForward, int attack)
    {
        if (Time.time > _nextSpellTime)
        {
            //_player.ImpulseSource.GenerateImpulse();
            _player.playerSpellEvent.Raise();
            _nextSpellTime = Time.time + spellCD / 1000;
            Spell nSpell = Instantiate(spell, shootOut.position, Quaternion.identity);
            nSpell.Initialize(spellForward, attack);
        }
        
    }
    
    public void MeleeAttack()
    {
        // Debug.Log("Melee Attempt");
        _player.playerAttackEvent.Raise();
        var force = (transform.forward * 20) - _player.Rigidbody.velocity;
        _player.Rigidbody.AddForce(force,ForceMode.VelocityChange);
        anim.Play("MeleeAttack");
    }
    
    public void ChargedMeleeAttack()
    {
        if (_player.CurrentAbility)
        {
            StartCoroutine(_player.AbilityCo(_player.CurrentAbility.duration));
        } 
    }

    public void ChargedRangedAttack()
    {
    }

    public void OnReapEvent()
    {
        // TODO: restore souls and maxHealth
        print("HEALTH:" + _player.CurrentHealth.ToString());
        _player.CurrentHealth += 15;
        if (_player.CurrentHealth >= _player.entityStats.maxHealth)
        {
            _player.CurrentHealth = _player.entityStats.maxHealth;
        }

        print(_player.CurrentHealth.ToString());
        print("SOULS:" + _player.soulCount.runTimeValue.ToString());
        _player.soulCount.runTimeValue += 20;
        if (_player.soulCount.runTimeValue >= _player.soulCount.initialValue)
        {
            _player.soulCount.runTimeValue = _player.soulCount.initialValue;
        }
        print(_player.soulCount.runTimeValue.ToString());
    }
    
}
