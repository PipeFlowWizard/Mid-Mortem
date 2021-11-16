using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public GameObject player;
    public FloatValue soulCounter;
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

    [SerializeField] private GameObject playerDeathUI;
    [SerializeField] private GameObject enemyReapedUI;
    [SerializeField] private GameObject BossReapedUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject hurtUI;

    private void InitializeStats()
    {
        statTextHealth.text = "Health: " + playerStats.CurrentHealth.ToString();
        // statTextSouls.text = "Souls: " + playerStats.CurrentSouls.ToString();
        statTextSouls.text = soulCounter.initialValue.ToString();
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
                hurtUI.SetActive(true);
                break;

            // case "Souls":
            //     statTextSouls.text = "Souls: " + playerStats.CurrentSouls.ToString();
            //     break;

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

    // Souls are separate from Stats, for now they're represented by the skull in Pause Menu
    public void OnSoulCountUpdate()
    {
        ////print("OnSoulCountUpdate()");
        statTextSouls.text = soulCounter.runTimeValue.ToString();
        // float ratio = soulCounter.runTimeValue / 100f;
        // soulFill.fillAmount = ratio;
    }

    // TODO: FIX THIS
    // For some reason, we have to toggle pause twice before the pauseUI gets active
    private void Resume()
    {
        _isPaused = false;
        pauseUI.GetComponent<PauseMenu>().resume();
        //pauseUI.SetActive(false);
        Time.timeScale = 1;
    }
    private void Pause()
    {
        _isPaused = true;
        pauseUI.SetActive(true);
        pauseUI.GetComponent<PauseMenu>().playVideo();
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

    public void goToOptions()
    {
        pauseUI.SetActive(false);
        optionsUI.SetActive(true);
    }

    public void returnToPause()
    {
        pauseUI.SetActive(true);
        optionsUI.SetActive(false);
    }

    public void OnPlayerDeath()
    {
        //print("OnDiedEvent()");
        playerDeathUI.SetActive(true);
        playerDeathUI.GetComponent<Burning>().SetBurning(true);

    }

    public void OnEnemyReaped()
    {
        //print("OnNormalEnemyReap()");
        enemyReapedUI.SetActive(true);
        StartCoroutine(TurnOffUI(enemyReapedUI));
    }

    public void OnBossReaped()
    {
        //print("OnBossEnemyReap()");
        BossReapedUI.SetActive(true);
        StartCoroutine(TurnOffUI(BossReapedUI));
    }

    private IEnumerator TurnOffUI(GameObject ui)
    {
        yield return new WaitForSeconds(2);
        ui.SetActive(false);
    }
}
