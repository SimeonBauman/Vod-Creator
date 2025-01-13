using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
public class videoPlayer : MonoBehaviour
{
    public GameObject VP;  // Reference to the VideoPlayer component
    public GameObject cam;

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

            vid.cam = Instantiate(cam);
            vid.cam.SetActive(false);

            vid.clip = clips[i];
            vid.Green = this.Green;
            vid.Red = this.Red;
            vid.Yellow = this.Yellow;
            vid.creator = this.GetComponent<videoPlayer>();

            VideoPlayer vp = g.GetComponent<VideoPlayer>();
            vp.clip = clips[i];
            //vp.targetCamera = vid.cam.GetComponent<Camera>();
            g.SetActive(false);

            
            
        }
        videoPlayers[0].SetActive(true);
        //videoPlayers[1].SetActive(true);

    }
    void Update()
    {
        // Check for the "P" key to pause the video
        
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


        
        /*videoPlayers[nextIndex].SetActive(true);
        
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
         videoPlayers[currIndex].SetActive(false);*/
        videoPlayers[nextIndex].GetComponent<VOD>().cam.SetActive(true);
        videoPlayers[currIndex].GetComponent<VOD>().cam.SetActive(false);
        yield return null;

        index++;
        if (index >= videoPlayers.Count)
        {
            index = 0;
        }

        
    }

    public void startAll()
    {
        for (int i = 0; i < videoPlayers.Count; i++)
        {
            videoPlayers[i].SetActive(true);
            videoPlayers[i].GetComponent<VideoPlayer>().playbackSpeed = 1;
            videoPlayers[i].GetComponent<VideoPlayer>().Stop();
            videoPlayers[i].GetComponent<VideoPlayer>().time = videoPlayers[i].GetComponent<VOD>().startTime;
            videoPlayers[i].GetComponent<VideoPlayer>().targetCamera = videoPlayers[i].GetComponent<VOD>().cam.GetComponent<Camera>();
        }
        for (int i = 0; i < videoPlayers.Count; i++)
        {
            
            videoPlayers[i].GetComponent<VideoPlayer>().Play();
           
        }
        videoPlayers[0].GetComponent<VOD>().cam.SetActive(true);
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
            this.startAll();
        }
    }
    
}
