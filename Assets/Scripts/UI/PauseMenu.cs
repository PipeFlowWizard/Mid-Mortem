using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject videoPlayer;
    private VideoPlayer video;

    void Start()
    {
        video = videoPlayer.GetComponent<VideoPlayer>();
    }

    private void Awake()
    {
        // Doesn't Show UI Elements of the Pause Menu
        gameObject.transform.GetChild(4).gameObject.SetActive(false);
    }

    private void Update()
    {
       
        if (video.frame > 52)
        {
            showInfo();
        }
        else
            gameObject.transform.GetChild(4).gameObject.SetActive(false);

    }


    void showInfo()
    {
        // print("In Coroutine");
        gameObject.transform.GetChild(4).gameObject.SetActive(true);
        // print("Done Coroutine");
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
