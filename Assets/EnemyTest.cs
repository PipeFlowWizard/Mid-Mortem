using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TEST CLASS, DELETE LATER
public class EnemyTest : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHurtBox"))
        {
            Destroy(gameObject);
        }
    }
}
