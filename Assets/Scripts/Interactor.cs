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
    public Color _colorNormal, _colorActive;
    
    private void Start()
    {
        _visual = GetComponentInChildren<MeshRenderer>();
        _collider = GetComponent<SphereCollider>();
        _colorActive = _visual.material.color;
        _colorNormal = _visual.material.color;

        _colorActive.a = 0.4f;
        _colorNormal.a = 0.1f;
    }
    


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided with " + other.name);
        if(other.CompareTag("Player"))
        {
            numColliders++;
            _visual.material.color = _colorActive;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            numColliders--;
            if (numColliders == 0)
                _visual.material.color = _colorNormal;
        }
    }
}
