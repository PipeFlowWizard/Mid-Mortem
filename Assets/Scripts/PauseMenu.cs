using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PauseMenu : MonoBehaviour
{
    public GameObject player;
    public GameObject input;
    [SerializeField] GameObject videoPlayer;
    private VideoPlayer video;

    void Start()
    {
        gameObject.SetActive(false);
        video = videoPlayer.GetComponent<VideoPlayer>();
    }

    private void Awake()
    {
        // Doesn't Show UI Elements of the Pause Menu
        gameObject.transform.GetChild(4).gameObject.SetActive(false);
    }

    private void Update()
    {
        if (video.frame > 75)
        {
            showInfo();
        }
        else
        {

        }
    }

    public void pauseGame()
    {
        print(video.frame);
        // TODO: Only Show UI When Video Stopped Playing
        player.GetComponent<PlayerController>().enabled ^= true;
        input.SetActive(!input.activeSelf);
        Time.timeScale = (Time.timeScale == 1.0f ? 0.0f : 1.0f);
        gameObject.SetActive(!gameObject.activeSelf);
        
    }

    void showInfo()
    {
        print("In Coroutine");
        gameObject.transform.GetChild(4).gameObject.SetActive(true);
        print("Done Coroutine");
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
