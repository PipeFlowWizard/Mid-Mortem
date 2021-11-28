using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    private AudioSource audioSource;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    public void UpdateMasterVolume()
    {
        audioSource.volume = masterVolumeSlider.normalizedValue;
    }
}
