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

    [Header("Pause Elements")]
    [SerializeField] private TextMeshProUGUI statTextMaxHealth;
    [SerializeField] private TextMeshProUGUI statTextCurrentHealth;
    [SerializeField] private TextMeshProUGUI statTextMaxSouls;
    [SerializeField] private TextMeshProUGUI statTextCurrentSouls;
    [SerializeField] private TextMeshProUGUI statTextAttack;
    [SerializeField] private TextMeshProUGUI statTextSpeed;
    [SerializeField] private TextMeshProUGUI statTextDefense;

    [Header("UI Elements")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject pixelationUI;
    [SerializeField] private GameObject hudUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject infoUI;
    [SerializeField] private GameObject hurtUI;
    [SerializeField] private GameObject enemyReapedUI;
    [SerializeField] private GameObject BossReapedUI;
    [SerializeField] private GameObject playerDeathUI;

    private void Start()
    {
        playerStats = player.GetComponent<Entity>();
        Time.timeScale = 0;

        InitializeStats();
    }

    private void InitializeStats()
    {
        statTextMaxHealth.text = "Max Health: " + playerStats.MaxHealth.ToString();
        statTextMaxHealth.text = "Current Health: " + playerStats.CurrentHealth.ToString();
        statTextMaxSouls.text = "Max Souls: 100";
        statTextCurrentSouls.text = "Current Souls: " + soulCounter.initialValue.ToString();
        statTextAttack.text = "Attack: " + playerStats.CurrentAttack.ToString();
        statTextSpeed.text = "Speed: " + playerStats.CurrentSpeed.ToString();
        statTextDefense.text = "Defense: " + playerStats.CurrentDefense.ToString();
    }

    public void UpdateStat(string stat)
    {
        switch (stat)
        {
            case "Max Health":
                statTextMaxHealth.text = "Max Health: " + playerStats.MaxHealth.ToString();
                GetComponentInChildren<HpBar>().UpdateHP();
                break;

            case "Current Health":
                statTextCurrentHealth.text = "Current Health: " + playerStats.CurrentHealth.ToString();
                GetComponentInChildren<HpBar>().UpdateHP();
                hurtUI.SetActive(true);
                break;

            case "Current Souls":
                statTextCurrentSouls.text = "Current Souls: " + soulCounter.runTimeValue.ToString();
                GetComponentInChildren<SoulBar>().UpdateSouls();
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

    private void Resume()
    {
        _isPaused = false;
        pauseUI.GetComponent<PauseMenu>().resume();
        infoUI.GetComponent<Options>().resume();
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
        infoUI.SetActive(true);
    }

    public void returnToPause()
    {
        pauseUI.SetActive(true);
        infoUI.SetActive(false);
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

    public void Play()
    {
        pixelationUI.SetActive(true);
        hudUI.SetActive(true);
        mainMenuUI.SetActive(false);

        Time.timeScale = 1;
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
