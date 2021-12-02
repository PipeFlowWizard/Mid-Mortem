using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulQueue : MonoBehaviour
{
    private int numQueue;
    private GameObject[] souls;
    [SerializeField] private GameObject dashAbility;
    [SerializeField] private GameObject firstAbility;
    [SerializeField] private GameObject secondAbility;

    public void OnDashAbilityGot()
    {
        dashAbility.SetActive(true);
    }
    public void OnFirstAbilityGot()
    {
        firstAbility.SetActive(true);
    }
    public void OnSecondAbilityGot()
    {
        secondAbility.SetActive(true);
    }

    // Start is called before the first frame update
    // void Start()
    // {
    //     numQueue = 0;
    //
    //     souls = new GameObject[3];
    //     for (int i = 0; i < 3; i++)
    //         souls[i] = transform.Find("Soul " + (i + 1).ToString()).gameObject;
    // }
    //
    // void Update()
    // {
    //     for (int i = 0; i < souls.Length; i++)
    //         if (i < numQueue)
    //             souls[i].transform.Find("Outer Ring").transform.Find("Dot").GetComponent<Image>().enabled = true;
    //         else
    //             souls[i].transform.Find("Outer Ring").transform.Find("Dot").GetComponent<Image>().enabled = false;
    // }
    //
    // public void UpdateQueue(int value)
    // {
    //     numQueue += value;
    //
    //     if (numQueue > 3)
    //         numQueue = 3;
    //     else if (numQueue < 0)
    //         numQueue = 0;
    //
    //     
    // }
    
    
}
