using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private List<GameObject> _spawnables;
    [SerializeField]
    private List<GameObject> _forest;
    public List<GameObject> Spawnables
    {
        get => _spawnables;
        set => Spawnables = value;
    }
    public List<GameObject> ForestObstacles
    {
        get => _forest;
        set => ForestObstacles = value;
    }
}

