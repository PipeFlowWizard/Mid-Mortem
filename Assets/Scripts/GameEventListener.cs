using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// See Unite Austin 2017 - Game Architecture with Scriptable Objects
// https://www.youtube.com/watch?v=raQ3iHhE_Kk 

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;
    public UnityEvent response;
    public void OnSignalRaised()
    {
        response.Invoke();
    }

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.DeRegisterListener(this);
    }
}

