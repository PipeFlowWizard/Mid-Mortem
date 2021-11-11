using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    // All HP References
    [SerializeField] private float hp;
    [SerializeField] private GameObject Player;
    [SerializeField] private float maxHp;
    [SerializeField] private Image hpFill;
    
    public Sprite happySprite;
    public Sprite medSprite;
    public Sprite sadSprite;
    public Sprite deadSprite;

    private Entity playerHealth;

    [SerializeField] private Image image;

    private float medThreshold = 0.6f;
    private float sadThreshold = 0.3f;

    // Soul Stuff
    private int numQueue;
    private GameObject[] souls;

    //Mana Stuff


    // Testing: 
    private float dmgCounter = 0f;
    private float dmgTime = 1f;

    private void Start()
    {
        //HP Stuff
        
        playerHealth = Player.GetComponent<Entity>();

        //SOUL Stuff
        /*
        numQueue = 0;
        souls = new GameObject[3];
        for (int i = 0; i < 3; i++)
            souls[i] = transform.Find("Soul " + (i + 1).ToString()).gameObject;
        */
        //MANA Stuff
        InitializeSouls();  //From UIEventManagerShowcase
    }

    // Update is called once per frame
    void Update()
    {
        //HP Stuff
        UpdateHpUI();

        //Souls Stuff
        /*
        for (int i = 0; i < souls.Length; i++)
            if (i < numQueue)
                souls[i].transform.Find("Outer Ring").transform.Find("Dot").GetComponent<Image>().enabled = true;
            else
                souls[i].transform.Find("Outer Ring").transform.Find("Dot").GetComponent<Image>().enabled = false;
        */
        //Mana Stuff

    }

    public void UpdateQueue(int value)
    {
        numQueue += value;

        if (numQueue > 3)
            numQueue = 3;
        else if (numQueue < 0)
            numQueue = 0;
    }

    public void UpdateHP(float value)
    {
        hp += value;
        if (hp > maxHp)
            hp = maxHp;
        else if (hp < 0)
            hp = 0;
    }

    public void UpdateHpUI()
    {
        hp = playerHealth.CurrentHealth;
        float ratio = hp / maxHp;
        hpFill.fillAmount = ratio;

        if (ratio > medThreshold)
            image.sprite = happySprite;
        else if (ratio > sadThreshold)
            image.sprite = medSprite;
        else if (ratio > 0)
            image.sprite = sadSprite;
        else
            image.sprite = deadSprite;
    }

    //IMPORTING UIEVENTMANAGER SHOWCASE
    [SerializeField] private TextMeshProUGUI soulText;
    [SerializeField] private FloatValue soulCounter;
    [SerializeField] private GameObject dedUI;
    [SerializeField] private GameObject normalReapUI;
    [SerializeField] private GameObject bossReapUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private Image soulFill;

    private void InitializeSouls()
    {
        soulText.text = soulCounter.initialValue.ToString();
    }
    public void OnSoulCountUpdate()
    {
        //print("OnSoulCountUpdate()");
        soulText.text = soulCounter.runTimeValue.ToString();
        float ratio = soulCounter.runTimeValue / 100f;
        soulFill.fillAmount = ratio;
    }

    public void OnPaused()
    {
        //print("OnPause()");
        pauseUI.SetActive(true);
        StartCoroutine(TurnOffUI(bossReapUI));
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
