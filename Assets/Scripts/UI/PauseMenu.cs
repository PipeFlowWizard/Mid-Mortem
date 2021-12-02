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

    private void Awake()
    {
        showInfo(false);

        video = videoPlayer.GetComponent<VideoPlayer>();

        //gameObject.SetActive(false);
    }

    public void playVideo()
    {
        showInfo(false);
        videoPlayer.SetActive(true);
    }

    public void showInfo(bool status)
    {
        soulInfo.SetActive(status);
        actionsInfo.SetActive(status);
        playerStatsInfo.SetActive(status);
    }

    public void resume()
    {
        showInfo(false);
        video.Pause();
        gameObject.SetActive(false);
    }

    public void quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}
