using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public GameObject player;
    private Entity playerStats;

    public GameEvent togglePauseEvent;

    private bool _isPaused;

    private void Start()
    {
        playerStats = player.GetComponent<Entity>();
        InitializeStats();
    }
    
    [SerializeField] private TextMeshProUGUI statTextHealth;
    [SerializeField] private TextMeshProUGUI statTextSouls;
    [SerializeField] private TextMeshProUGUI statTextAttack;
    [SerializeField] private TextMeshProUGUI statTextSpeed;
    [SerializeField] private TextMeshProUGUI statTextDefense;

    [SerializeField] private GameObject dedUI;
    [SerializeField] private GameObject normalReapUI;
    [SerializeField] private GameObject bossReapUI;
    [SerializeField] private GameObject pauseUI;

    private void InitializeStats()
    {
        statTextHealth.text = "Health: " + playerStats.CurrentHealth.ToString();
        statTextSouls.text = "Souls: " + playerStats.CurrentSouls.ToString();
        statTextAttack.text = "Attack: " + playerStats.CurrentAttack.ToString();
        statTextSpeed.text = "Speed: " + playerStats.CurrentSpeed.ToString();
        statTextDefense.text = "Defense: " + playerStats.CurrentDefense.ToString();
    }

    public void UpdateStat(string stat)
    {
        switch (stat)
        {
            case "Health":
                statTextHealth.text = "Health: " + playerStats.CurrentHealth.ToString();
                GetComponentInChildren<HpBar>().UpdateHP();
                break;

            case "Souls":
                statTextSouls.text = "Souls: " + playerStats.CurrentSouls.ToString();
                break;

            case "Attack":
                statTextAttack.text = "Attack: " + playerStats.CurrentAttack.ToString();
                break;

            case "Speed":
                statTextSpeed.text = "Speed: " + playerStats.CurrentSpeed.ToString();
                break;

            case "Defense":
                statTextDefense.text = "Defense: " + playerStats.CurrentDefense.ToString();
                break;
        }
    }

    public void OnSoulCountUpdate()
    {
        ////print("OnSoulCountUpdate()");
        //soulText.text = soulCounter.runTimeValue.ToString();
        //float ratio = soulCounter.runTimeValue / 100f;
        //soulFill.fillAmount = ratio;
    }

    // TODO: FIX THIS
    // For some reason, we have to toggle pause twice before the pauseUI gets active
    private void Resume()
    {
        _isPaused = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1;
    }
    private void Pause()
    {
        _isPaused = true;
        pauseUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnPauseInput()
    {
        togglePauseEvent.Raise();
        if (_isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    

    public void OnNormalEnemyReap()
    {
        //print("OnNormalEnemyReap()");
        normalReapUI.SetActive(true);
        StartCoroutine(TurnOffUI(normalReapUI));
    }

    public void OnBossEnemyReap()
    {
        //print("OnBossEnemyReap()");
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
        //print("OnDiedEvent()");
        dedUI.SetActive(true);
        dedUI.GetComponent<Burning>().SetBurning(true);

    }
}
