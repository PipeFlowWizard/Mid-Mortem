using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Key;

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "PuzzleComp") GetComponentInParent<Level>().keyAcquired();
    }
}
