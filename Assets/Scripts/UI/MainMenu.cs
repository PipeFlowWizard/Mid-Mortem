using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int playSceneIndex;
    [SerializeField] private string optionSceneIndex;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject startButton;

    public void play()
    {
        // SceneManager.LoadScene(playSceneIndex, LoadSceneMode.Single);
        loading.SetActive(true);
        startButton.SetActive(false);
        StartCoroutine(LoadYourAsyncScene());
    }

    public void options()
    {
        SceneManager.LoadScene(optionSceneIndex, LoadSceneMode.Single);
    }

    public void quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }
    
    private IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(playSceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading..");
            yield return null;
        }

    }
}
