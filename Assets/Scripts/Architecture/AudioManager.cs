using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource enemySource;
    [SerializeField] private AudioSource playerSource;
    [SerializeField] private AudioSource effectSource;
    
    


    public void OnEnemySoundEvent(AudioClip clip)
    {
        enemySource.clip = clip;
        enemySource.Play();
    }
    public void OnPlayerSoundEvent(AudioClip clip)
    {
        playerSource.clip = clip;
        playerSource.Play();
    }
    public void OnEffectSoundEven(AudioClip clip)
    {
        effectSource.clip = clip;
        effectSource.Play();
    }
    public void OnMusicChangeEvent(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

}
