using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
public class videoPlayer : MonoBehaviour
{
    public GameObject VP;  // Reference to the VideoPlayer component

    public VideoClip[] clips;
    public List<GameObject> videoPlayers = new List<GameObject>();
    public int index = 0;

    

    public GameObject tempPixel;
    public List<GameObject> pixels = new List<GameObject>();

    public Color32 Yellow;
    public Color32 Green;
    public Color32 Red;

    private void Start()
    {

        for (int i = 0; i < clips.Length; i++)
        {
            GameObject g = Instantiate(VP);

            videoPlayers.Add(g);

            VOD vid = g.GetComponent<VOD>();
            vid.clip = clips[i];
            vid.Green = this.Green;
            vid.Red = this.Red;
            vid.Yellow = this.Yellow;
            vid.creator = this.GetComponent<videoPlayer>();

            VideoPlayer vp = g.GetComponent<VideoPlayer>();
            vp.clip = clips[i];
            g.SetActive(false);

            
            
        }
        videoPlayers[0].SetActive(true);
        //videoPlayers[1].SetActive(true);

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
    void createPixels()
    {
        pixels.Add(Instantiate(tempPixel, new Vector2(0,0), transform.rotation));
        pixels.Add(Instantiate(tempPixel, new Vector2(0,0), transform.rotation));
        /*for (int j = 0; j < 200; j++)
        {
            for (int i = 0; i < 500; i++)
            {
                pixels.Add(Instantiate(tempPixel, new Vector2(j, i), transform.rotation));

            }
        }*/
    }
    IEnumerator switchVideos()
    {

        int currIndex = index;
        int nextIndex = 0;
        if (currIndex >= videoPlayers.Count - 1) nextIndex = 0;
        else nextIndex = currIndex + 1;
        
        videoPlayers[nextIndex].SetActive(true);
        
        videoPlayers[nextIndex].GetComponent<VideoPlayer>().time = videoPlayers[nextIndex].GetComponent<VOD>().startTime + (videoPlayers[currIndex].GetComponent<VideoPlayer>().time - videoPlayers[currIndex].GetComponent<VOD>().startTime);
        videoPlayers[nextIndex].GetComponent<VideoPlayer>().Prepare();

        while (!videoPlayers[nextIndex].GetComponent<VideoPlayer>().isPrepared)
        {
            videoPlayers[currIndex].GetComponent<VideoPlayer>().Play();
            yield return null;
            
        }

        videoPlayers[nextIndex].GetComponent<VideoPlayer>().Play();

    
        
         yield return new WaitForEndOfFrame();
         videoPlayers[currIndex].GetComponent<VideoPlayer>().Stop();
         videoPlayers[currIndex].SetActive(false);
        index++;
        if (index >= videoPlayers.Count)
        {
            index = 0;
        }

        
    }

    
    public void renderNext()
    {
        videoPlayers[index].SetActive(false);
        videoPlayers[index].GetComponent<VideoPlayer>().playbackSpeed = 1;
        if(index <  videoPlayers.Count - 1) {
            index++;
            videoPlayers[index].SetActive(true);
        }
        else
        {
            index = 0;
            videoPlayers[index].SetActive(true);
            videoPlayers[index].GetComponent<VideoPlayer>().time = videoPlayers[index].GetComponent<VOD>().startTime;
        }
    }
    
}
