using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField] float yPos;
    [SerializeField] float range = 8;
    [SerializeField] float exclusionRange = 4;
    
    void Awake()
    {
        // -8....-4 XXX 4....8
        float x = Random.Range(-range + exclusionRange, range - exclusionRange);
        if (x < 0 && x > -exclusionRange)
        {
            x -= exclusionRange;
        }
        else if (x >= 0 && x < exclusionRange)
        {
            x += exclusionRange;
        }
        
        float z = Random.Range(-range, range);
        if (z < 0 && z > -exclusionRange)
        {
            z -= exclusionRange;
        }
        else if (z >= 0 && z < exclusionRange)
        {
            z += exclusionRange;
        }
        
        print($"PUZZLE X: {x} Z: {z}");
        transform.position = new Vector3(x, yPos, z);

    }
}
