using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    public Toggle tog;
    private AudioSource[] gameMusic;
    public TextMeshProUGUI label;

    //===========================================
    // Basic Menu
    //===========================================
    private void Awake()
    {
        //gameObject.SetActive(false);
        tog = GetComponentInChildren<Toggle>();
    }

    private void Update()
    {
        if (tog.isOn)
        {
            label.text = "On";
            label.color = new Color(109f / 255f, 254f / 255f, 136f / 255f);
        }
        else
        {
            label.text = "Off";
            label.color = new Color(254f / 255f, 97f / 255f, 97f / 255f);
        }
    }

    public void SetVolume(float vol)
    {
        gameMusic = FindObjectsOfType<AudioSource>();
        foreach(AudioSource sound in gameMusic)
        {
            sound.volume = vol;
        }
        
    }
    public void resume()
    {
        gameObject.SetActive(false);
    }


    public void SetFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }




    
}
