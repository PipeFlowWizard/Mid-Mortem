using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarScript : MonoBehaviour
{
    [SerializeField] private float hp = 100;
    [SerializeField] private float maxHp = 100;
    [SerializeField] private Image hpFill;
    [SerializeField] private GameObject sadBorder;
    [SerializeField] private GameObject happyBorder;
    [SerializeField] private GameObject medBorder;
    [SerializeField] private GameObject deadBorder;
    [SerializeField] private Transform Player; // Get Player HP from Player Transform

    // Testing: 
    private float dmgCounter = 0f;
    private float dmgTime = 1f;

    // Update is called once per frame
    void Update()
    {
        dmgCounter += Time.deltaTime;
        if(dmgCounter > dmgTime)
        {
            hp -= 15f;
            dmgCounter = 0;
        }
        UpdateHP();
        
    }
    private void UpdateHP()
    {
        
        float ratio = hp / maxHp;
        hpFill.fillAmount = ratio;
        if (ratio > 0.65f)
        {
            sadBorder.gameObject.SetActive(false);
            happyBorder.gameObject.SetActive(true);
            medBorder.gameObject.SetActive(false);
            deadBorder.gameObject.SetActive(false);
  
        }
        else if(ratio < 0.33 && ratio > 0)
        {
            sadBorder.gameObject.SetActive(true);
            happyBorder.gameObject.SetActive(false);
            medBorder.gameObject.SetActive(false);
            deadBorder.gameObject.SetActive(false);
        }
        else if(ratio <= 0){
            sadBorder.gameObject.SetActive(false);
            happyBorder.gameObject.SetActive(false);
            medBorder.gameObject.SetActive(false);
            deadBorder.gameObject.SetActive(true);
        }
        else
        {
            sadBorder.gameObject.SetActive(false);
            happyBorder.gameObject.SetActive(false);
            medBorder.gameObject.SetActive(true);
            deadBorder.gameObject.SetActive(false);
        }
        
    }
}
