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
    [SerializeField]
    private List<GameObject> _forest_grass;
    
    [SerializeField]
    private List<GameObject> _snow;
    [SerializeField]
    private List<GameObject> _sand;
    [SerializeField]
    private List<GameObject> _sand_grass;
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
    public List<GameObject> ForestGrass
    {
        get => _forest_grass;
        set => ForestGrass = value;
    }
    public List<GameObject> SandGrass
    {
        get => _sand_grass;
        set => SandGrass = value;
    }
    public List<GameObject> SnowObstacles
    {
        get => _snow;
        set => SnowObstacles = value;
    }
    public List<GameObject> SandObstacles
    {
        get => _sand;
        set => SandObstacles = value;
    }
}

