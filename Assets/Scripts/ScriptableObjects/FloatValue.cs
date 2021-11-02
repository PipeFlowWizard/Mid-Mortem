using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// See MisterTaftCreates
// https://youtu.be/madv_VkYQno?list=PL4vbr3u7UKWp0iM1WIfRjCDTI03u43Zfu

[CreateAssetMenu]
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
{
    public float initialValue;

    [HideInInspector] public float runTimeValue;

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        runTimeValue = initialValue;
    }
}

