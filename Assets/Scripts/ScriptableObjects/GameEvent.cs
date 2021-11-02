using System.Collections.Generic;
using UnityEngine;

// From Unite Austin 2017 - Game Architecture with Scriptable Objects
// https://www.youtube.com/watch?v=raQ3iHhE_Kk 

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise()
    {
        // iterating backwards to remove from the end of the list
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnSignalRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void DeRegisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

}

