using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] private GameEvent videoFinishedEvent;
    private VideoPlayer video;
    private bool startVideo;

    private void Awake()
    {
        video = GetComponent<VideoPlayer>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        video.targetTexture.Release();
        StartCoroutine(playVideo());
    }

    IEnumerator playVideo()
    {
        video.Play();

        while (!video.isPlaying)
            yield return null;

        startVideo = true;
    }

    private void Update()
    {
        if (startVideo && !video.isPlaying)
        {
            startVideo = false;
            videoFinishedEvent.Raise();
            gameObject.SetActive(false);
        }
    }
}
