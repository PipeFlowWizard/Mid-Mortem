using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Spell spell;
    [SerializeField]  Transform spellOrigin;
    [SerializeField]  float spellSpeed = 50f;
    [SerializeField]  float spellCD = 100f;


    

    private float _nextSpellTime = 0f;
    public void Cast()
    {
        if (Time.time > _nextSpellTime)
        {
            _nextSpellTime = Time.time + spellCD / 1000;
            Spell nSpell = Instantiate(spell, spellOrigin.position, spellOrigin.rotation);
            nSpell.SetSpeed(spellSpeed);
        }
        
    }

}
