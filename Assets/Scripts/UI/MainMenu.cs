using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string playSceneName;
    [SerializeField] private string optionSceneName;

    public void play()
    {
        SceneManager.LoadScene(playSceneName, LoadSceneMode.Single);
    }

    public void options()
    {
        SceneManager.LoadScene(optionSceneName, LoadSceneMode.Single);
    }

    public void quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
}