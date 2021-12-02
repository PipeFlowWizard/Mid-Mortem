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
    private List<GameObject> _snow;
    [SerializeField]
    private List<GameObject> _sand;
    [SerializeField]
    private List<GameObject> _forestGrass;
    [SerializeField]
    private List<GameObject> _sandGrass;
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
    public List<GameObject> GrassForest
    {
        get => _forestGrass;
        set => GrassForest = value;
    }
    public List<GameObject> GrassSand
    {
        get => _sandGrass;
        set => GrassSand = value;
    }
    public GameEvent desertBiomeEvent;
    public GameEvent snowBiomeEvent;
    public GameEvent forestBiomeEvent;
}

