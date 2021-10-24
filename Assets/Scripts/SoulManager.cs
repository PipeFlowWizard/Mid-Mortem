using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoulManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI soulText;
    [SerializeField] private FloatValue soulCounter;

    private void Start()
    {
        InitializeSouls();
    }

    private void InitializeSouls()
    {
        soulText.text = soulCounter.initialValue.ToString();
    }
    public void OnSoulCountUpdate()
    {
        soulText.text = soulCounter.runTimeValue.ToString();
    }
}
