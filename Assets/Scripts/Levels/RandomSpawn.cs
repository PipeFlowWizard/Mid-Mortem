using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField] float yPos;
    void Awake()
    {
        float x = Random.Range(-8, 8);
        float z = Random.Range(-8, 8);

        transform.position = new Vector3(x, yPos, z);

    }
}
