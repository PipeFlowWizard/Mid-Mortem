using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoulManager : MonoBehaviour
{
    // All for demo purposes, add to real UI when needed
    [SerializeField] private TextMeshProUGUI soulText;
    [SerializeField] private FloatValue soulCounter;
    [SerializeField] private GameObject dedUI;

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

    public void OnDiedEvent()
    {
        dedUI.SetActive(true);
        
    }
}
