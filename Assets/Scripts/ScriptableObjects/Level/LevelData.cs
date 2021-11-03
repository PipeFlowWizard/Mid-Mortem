using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private List<GameObject> _spawnables;
    public List<GameObject> Spawnables
    {
        get => _spawnables;
        set => Spawnables = value;
    }
}

