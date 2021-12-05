using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle;
    public TextMeshProUGUI fullscreenLabel;

    [SerializeField] private Slider volumeSlider;
    private AudioSource[] gameMusic;

    private void Awake()
    {
        SetFullScreen();
    }

    public void SetVolume()
    {
        gameMusic = FindObjectsOfType<AudioSource>();
        foreach (AudioSource sound in gameMusic)
            sound.volume = volumeSlider.value;
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;

        if (fullscreenToggle.isOn)
        {
            fullscreenLabel.text = "On";
            fullscreenLabel.color = new Color(109f / 255f, 254f / 255f, 136f / 255f);
        }
        else
        {
            fullscreenLabel.text = "Off";
            fullscreenLabel.color = new Color(254f / 255f, 97f / 255f, 97f / 255f);
        }
    }

    public void resume()
    {
        gameObject.SetActive(false);
    }
}