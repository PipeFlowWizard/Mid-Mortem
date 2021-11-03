using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject player;
    public GameObject input;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void pauseGame()
    {
        player.GetComponent<PlayerController>().enabled ^= true;
        input.SetActive(!input.activeSelf);
        Time.timeScale = (Time.timeScale == 1.0f ? 0.0f : 1.0f);
        gameObject.SetActive(!gameObject.activeSelf);
        
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
