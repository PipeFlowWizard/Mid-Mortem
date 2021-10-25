using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private Spell spell;
    [SerializeField]  float spellCD = 100f;


    private float _nextSpellTime = 0f;
    public void Cast(Vector3 spellOrigin, Vector3 spellForward)
    {
        if (Time.time > _nextSpellTime)
        {
            _nextSpellTime = Time.time + spellCD / 1000;
            Spell nSpell = Instantiate(spell, spellOrigin, Quaternion.identity);
            nSpell.Initialize(spellForward);
        }
        
    }


}
