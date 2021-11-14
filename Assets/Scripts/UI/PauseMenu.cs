using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject videoPlayer;
    [SerializeField] private GameObject soulInfo;
    [SerializeField] private GameObject actionsInfo;
    [SerializeField] private GameObject playerStatsInfo;
    private VideoPlayer video;

    void Start()
    {
        gameObject.SetActive(false);
        video = videoPlayer.GetComponent<VideoPlayer>();
    }

    private void Awake()
    {
        // Doesn't Show UI Elements of the Pause Menu
        soulInfo.SetActive(false);
        actionsInfo.SetActive(false);
        playerStatsInfo.SetActive(false);
    }

    private void Update()
    {
        if (video.frame > 100)
        //if (video.frame > 52)
        {
            showInfo();
        }
        else
        {
            soulInfo.SetActive(false);
            actionsInfo.SetActive(false);
            playerStatsInfo.SetActive(false);
        }

    }


    void showInfo()
    {
        soulInfo.SetActive(true);
        actionsInfo.SetActive(true);
        playerStatsInfo.SetActive(true);
    }

    public void options()
    {

    }

    public void quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
