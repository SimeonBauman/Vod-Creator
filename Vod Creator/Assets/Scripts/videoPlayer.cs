using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class videoPlayer : MonoBehaviour
{
    public GameObject VP;  // Reference to the VideoPlayer component

    public VideoClip[] clips;
    public List<GameObject> videoPlayers = new List<GameObject>();
    public int index = 0;

    public Camera renderCamera; // Camera to capture video frame
    public Texture2D texture2D;

    private void Start()
    {

        for (int i = 0; i < clips.Length; i++)
        {
            GameObject g = Instantiate(VP);

            videoPlayers.Add(g);
            g.GetComponent<VideoPlayer>().clip = clips[i];
            g.SetActive(false);
            //g.GetComponent<VideoPlayer>().renderMode = VideoRenderMode.APIOnly; // Render through code, not directly to a screen
            g.GetComponent<VideoPlayer>().prepareCompleted += OnVideoPrepared;
            //g.GetComponent<VideoPlayer>().errorReceived += OnVideoError;
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
        
        if (videoPlayers[index].GetComponent<VideoPlayer>().isPlaying && texture2D != null && (int)videoPlayers[index].GetComponent<VideoPlayer>().frame%5 == 0)
        {
            // Capture the video frame after the camera has rendered it
            CaptureFrameFromCamera();
        }
        
    }

    IEnumerator switchVideos()
    {
        videoPlayers[index + 1].SetActive(true);
        videoPlayers[index + 1].GetComponent<VideoPlayer>().Play();
        while (!videoPlayers[index + 1].GetComponent<VideoPlayer>().isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        videoPlayers[index].GetComponent<VideoPlayer>().Pause();
        videoPlayers[index].SetActive(false);
        index = 1;
    }

    private void CaptureFrameFromCamera()
    {
        StartCoroutine(CaptureFrame());
    }
    
    private IEnumerator CaptureFrame()
    {
        // Wait for the end of the frame to ensure rendering is complete
        yield return new WaitForEndOfFrame();
       

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderCamera.targetTexture;

        // Render the camera
        renderCamera.Render();

        // Ensure the Texture2D size matches the render texture
        OnVideoPrepared(videoPlayers[index].GetComponent<VideoPlayer>());
        

        // Copy pixels from the render texture to the Texture2D
        texture2D.ReadPixels(new Rect(0, 0, 1, 500), 0, 0);
        texture2D.Apply();

        // Log pixel color for debugging
        Color pixelColor = texture2D.GetPixel(0, 0);
        Debug.Log("Pixel color at (0,0): " + pixelColor);

        // Restore the previous render texture
        RenderTexture.active = currentRT;
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        // Initialize texture to the resolution of the video
        texture2D = new Texture2D((int)videoPlayers[index].GetComponent<VideoPlayer>().width, (int)videoPlayers[index].GetComponent<VideoPlayer>().height, TextureFormat.RGB24, false);
    }
}
