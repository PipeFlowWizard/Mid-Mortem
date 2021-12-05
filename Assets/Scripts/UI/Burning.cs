using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Burning : MonoBehaviour
{
    Material material;
    bool isDissolving = false;
    float fade = 0f;

    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.transform.GetChild(0).GetComponent<Image>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isDissolving)
        {
            fade += Time.deltaTime;
            // print(fade);
            if(fade >=1f)
            {
                fade = 1f;
                isDissolving = false;
                transform.Find("Buttons").gameObject.SetActive(true);
            }
        }

        material.SetFloat("_Fade", fade);


    }

    public void SetBurning(bool isBurning)
    {
        isDissolving = isBurning;
    }
}
