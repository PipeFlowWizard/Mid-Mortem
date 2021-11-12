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
            _nextSpellTime = Time.time + spellCD / 1000;
            Spell nSpell = Instantiate(spell, shootOut.position, Quaternion.identity);
            nSpell.Initialize(spellForward, attack);
        }
        
    }
    
    public void MeleeAttack()
    {
        // Debug.Log("Melee Attempt");
        anim.Play("MeleeAttack");
    }
    
    public void ChargedMeleeAttack()
    {
        if (_player.CurrentAbility)
        {
            StartCoroutine(_player.AbilityCo(_player.CurrentAbility.duration));
        } 
    }
    
    
}
