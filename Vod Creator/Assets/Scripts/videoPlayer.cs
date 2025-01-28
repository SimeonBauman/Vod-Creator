using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class videoPlayer : MonoBehaviour
{
    public GameObject VP;  // Reference to the VideoPlayer component
    public GameObject cam;

    public List<VideoClip> clips;
    public List<GameObject> videoPlayers = new List<GameObject>();
    public int index = 0;

    

    public GameObject tempPixel;
    public List<GameObject> pixels = new List<GameObject>();

    public Color32 Yellow;
    public Color32 Green;
    public Color32 Red;

    public static bool preping;
    public GameObject prepScreen;

    private void Start()
    {
        Screen.SetResolution(1920,1080, true);

        JSONReader.readJSON();
        
        preping = true;

        for (int i = 0; i < JSONReader.vid.players.Length; i++)
        {
            GameObject g = Instantiate(VP);



            videoPlayers.Add(g);

            VOD vid = g.GetComponent<VOD>();

            vid.cam = Instantiate(cam);
            vid.cam.SetActive(false);

            vid.clip = JSONReader.vid.players[i].path;
            vid.Green = this.Green;
            vid.Red = this.Red;
            vid.Yellow = this.Yellow;
            vid.creator = this.GetComponent<videoPlayer>();

           if (JSONReader.vid.players[0].startTime != "") vid.startTime = double.Parse(JSONReader.vid.players[i].startTime);


            VideoPlayer vp = g.GetComponent<VideoPlayer>();
            vp.url = JSONReader.vid.players[i].path;
            //vp.targetCamera = vid.cam.GetComponent<Camera>();
            g.SetActive(false);

            
            
        }
        
        if (videoPlayers[0].GetComponent<VOD>().startTime > 0) StartCoroutine(this.startAll());
        else videoPlayers[0].SetActive(true);
        //videoPlayers[1].SetActive(true);

    }
    void Update()
    {
        // Check for the "P" key to pause the video
        if (!preping)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(switchVideos());
            }

            this.switchVideosByNumber();
        }
        prepScreen.SetActive(preping);
        
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

    public IEnumerator startAll()
    {
        //Camera.main.gameObject.GetComponent<Camera>().enabled = false;
        
        for (int i = 0; i < videoPlayers.Count; i++)
        {
            
            
            videoPlayers[i].GetComponent<VideoPlayer>().playbackSpeed = 1;
            videoPlayers[i].GetComponent<VideoPlayer>().Stop();
            
            
        }
        for (int i = 0; i < videoPlayers.Count; i++)
        {
            //videoPlayers[i].GetComponent<VOD>().cam.SetActive(true);
            videoPlayers[i].SetActive(false);
            yield return new WaitForEndOfFrame();
            videoPlayers[i].SetActive(true);
            yield return new WaitForEndOfFrame();

            //try prepare
            videoPlayers[i].GetComponent<VideoPlayer>().Prepare();
            while (!videoPlayers[i].GetComponent<VideoPlayer>().isPrepared) { yield return new WaitForEndOfFrame(); Debug.Log("waiting"); }
            videoPlayers[i].GetComponent<VideoPlayer>().Play(); 
            while(!videoPlayers[i].GetComponent<VideoPlayer>().isPlaying) yield return new WaitForEndOfFrame();

            videoPlayers[i].GetComponent<VideoPlayer>().time = videoPlayers[i].GetComponent<VOD>().startTime;
            videoPlayers[i].GetComponent<VideoPlayer>().targetCamera = videoPlayers[i].GetComponent<VOD>().cam.GetComponent<Camera>();
            Debug.Log(videoPlayers[i].GetComponent<VOD>().cam.name);

        }
        videoPlayers[0].GetComponent<VOD>().cam.SetActive(true);
        yield return null;
        preping = false;
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
            JSONReader.writeToJSON("", JSONReader.vid.players);
            StartCoroutine(this.startAll());
            
        }
    }
    
    public void switchVideosByNumber()
    {
        int i = 10000;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            i = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            i = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            i = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            i = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            i = 4;
        }

        if (i < videoPlayers.Count )
        {
            videoPlayers[i].GetComponent<VOD>().cam.SetActive(true);
            if(index != i) videoPlayers[index].GetComponent<VOD>().cam.SetActive(false);
            index = i;
        }
    }

    
}
