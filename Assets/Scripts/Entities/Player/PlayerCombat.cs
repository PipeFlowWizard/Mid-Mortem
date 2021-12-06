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

    public int healthRecoverAmount = 10;
    public int soulRecoverAmount = 15;
    public float reapTimer = 3;
    public Animator anim;
    private float lastMeleeTime = 0;
    
    
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
            Spell nSpell = Instantiate(spell, shootOut.position, shootOut.rotation);
            nSpell.Initialize(spellForward, attack);
        }
        
    }
    
    public void MeleeAttack()
    {
        if(Time.time - lastMeleeTime >= 1f/_player.entityStats.meleeAttackSpeed)
        {
            // Debug.Log("Melee Attempt");
            _player.playerAttackEvent.Raise();
            var force = (transform.forward * 20) - _player.Rigidbody.velocity;
            _player.Rigidbody.AddForce(force, ForceMode.VelocityChange);
            anim.Play("MeleeAttack");
            lastMeleeTime = Time.time;
        }
    }

    public void FirstAbility()
    {
        if (_player.FirstAbility && _player.HasFirstAbility)
        {
            StartCoroutine(_player.AbilityCo(_player.FirstAbility.duration, _player.FirstAbility));
        }
        
    }
    public void SecondAbility()
    {
        if (_player.SecondAbility && _player.HasSecondAbility)
        {
            StartCoroutine(_player.AbilityCo(_player.SecondAbility.duration, _player.SecondAbility));
        }
    }
    
    public void ChargedMeleeAttack()
    {

    }

    public void ChargedRangedAttack()
    {
    }

    public void OnReapEvent()
    {
        StartCoroutine(ReapTimerCo());
        print($"health before: {_player.CurrentHealth.ToString()}");
        print($"souls before: {_player.soulCount.runTimeValue.ToString()}");
        _player.CurrentHealth += healthRecoverAmount;
        if (_player.CurrentHealth >= _player.entityStats.maxHealth)
        {
            _player.CurrentHealth = _player.entityStats.maxHealth;
        }

        _player.soulCount.runTimeValue += soulRecoverAmount;
        if (_player.soulCount.runTimeValue >= _player.soulCount.initialValue)
        {
            _player.soulCount.runTimeValue = _player.soulCount.initialValue;
        }
        print($"health after: {_player.CurrentHealth.ToString()}");
        print($"souls after: {_player.soulCount.runTimeValue.ToString()}");
    }
    
    private IEnumerator ReapTimerCo()
    {
        _player.Movement.MoveSpeed = _player.Movement.MoveSpeed / 2;
        yield return new WaitForSeconds(reapTimer);
        _player.Movement.MoveSpeed = _player.entityStats.speed;
    }
    
}
