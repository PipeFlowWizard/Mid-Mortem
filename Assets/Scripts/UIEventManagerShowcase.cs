using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIEventManagerShowcase : MonoBehaviour
{
    // All for demo purposes, add to real UI when needed
    [SerializeField] private TextMeshProUGUI soulText;
    [SerializeField] private FloatValue soulCounter;
    [SerializeField] private GameObject dedUI;
    [SerializeField] private GameObject normalReapUI;
    [SerializeField] private GameObject bossReapUI;

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

    public void OnNormalEnemyReap()
    {
        normalReapUI.SetActive(true);
        StartCoroutine(TurnOffUI(normalReapUI));
    }

    public void OnBossEnemyReap()
    {
        bossReapUI.SetActive(true);
        StartCoroutine(TurnOffUI(bossReapUI));
    }

    private IEnumerator TurnOffUI(GameObject ui)
    {
        yield return new WaitForSeconds(2);
        ui.SetActive(false);
    }
    public void OnDiedEvent()
    {
        dedUI.SetActive(true);
        
    }
}
