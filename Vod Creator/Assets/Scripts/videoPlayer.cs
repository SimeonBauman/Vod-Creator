using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Video;
public class videoPlayer : MonoBehaviour
{
    public GameObject VP;  // Reference to the VideoPlayer component

    public VideoClip[] clips;
    public List<GameObject> videoPlayers = new List<GameObject>();
    public int index = 0;
    private void Start()
    {
        
        for (int i = 0; i < clips.Length; i++)
        {
            GameObject g = Instantiate(VP);
           
            videoPlayers.Add(g);
            g.GetComponent<VideoPlayer>().clip = clips[i];
            g.SetActive(false);
        }
        videoPlayers[0].SetActive(true);
    }
    void Update()
    {
        // Check for the "P" key to pause the video
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Pause the video if it's playing
            if (videoPlayers[index].GetComponent<VideoPlayer>().isPlaying)
            {
                videoPlayers[index].GetComponent<VideoPlayer>().Pause();
                Debug.Log("Video paused.");
            }
            else
            {
                videoPlayers[index].GetComponent<VideoPlayer>().Play();  // Optional: if you want to resume playing on pressing P again
                Debug.Log("Video playing.");
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {

            StartCoroutine(switchVideos());
        }

        
    }

    IEnumerator switchVideos()
    {
        videoPlayers[index + 1].SetActive(true);
        videoPlayers[index + 1].GetComponent<VideoPlayer>().Play();
        while(!videoPlayers[index + 1].GetComponent<VideoPlayer>().isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        videoPlayers[index].GetComponent<VideoPlayer>().Pause();
        videoPlayers[index].SetActive(false);
        index = 1;
    }
}
