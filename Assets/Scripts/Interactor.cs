using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;



[RequireComponent(typeof(SphereCollider))]
public class Interactor : MonoBehaviour
{
    private Collider _collider;
    private MeshRenderer _visual;
    public int numColliders = 0;
    
    private void Start()
    {
        _visual = GetComponentInChildren<MeshRenderer>();
        _collider = GetComponent<SphereCollider>();
    }
    


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided with " + other.name);
        if(other.CompareTag("Player"))
        {
            numColliders++;
            _visual.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            numColliders--;
            if (numColliders == 0)
                _visual.enabled = false;
        }
    }
}
