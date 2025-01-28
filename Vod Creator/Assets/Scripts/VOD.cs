using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;
public class VOD : MonoBehaviour
{
    public VideoPlayer player;

    public double startTime = 0;

    public string clip;

    public Camera renderCamera; // Camera to capture video frame
    public Texture2D texture2D;

    public Color32 Yellow;
    public Color32 Green;
    public Color32 Red;

    public videoPlayer creator;

    public GameObject cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player.Stop();
        
        
        player.prepareCompleted += OnVideoPrepared;
        if (startTime == 0)
        {
            StartCoroutine(FindStartTime());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isPlaying && texture2D != null && (int)player.frame % 5 == 0)
        {
            // Capture the video frame after the camera has rendered it
            //CaptureFrameFromCamera();
        }
    }

    private void CaptureFrameFromCamera()
    {
        StartCoroutine(CaptureFrame());
    }

    private IEnumerator FindStartTime()
    {
        //player.renderMode = VideoRenderMode.APIOnly;
        player.playbackSpeed = 2.5f;
        player.Play();

        int numOfFrames = 0;

        while (!player.isPlaying || texture2D == null)
        {
            yield return new WaitForEndOfFrame();
        }

        
        yield return new WaitForEndOfFrame();

        while (startTime == 0)
        {
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = renderCamera.targetTexture;

            
            // Render the camera
            renderCamera.Render();

            // Ensure the Texture2D size matches the render texture
            OnVideoPrepared(player);


            // Copy pixels from the render texture to the Texture2D
            texture2D.ReadPixels(new Rect(1725, 540, 1, 540), 0, 0);
            texture2D.Apply();


            for (int i = 0; i < 540; i++)
            {
               
                Color32 color = texture2D.GetPixel(0, i);
                if(Input.GetKey(KeyCode.K)) Debug.Log(color);
                if (matches(color,Yellow) || matches(color, Green) || matches(color, Red))
                {
                    Debug.Log(color);
                    if (numOfFrames == 1)
                    {
                        this.startTime = player.time - 20;
                        Debug.Log(startTime);
                        Debug.Log(color);
                        Debug.Log(i);
                        player.Pause();
                        break;
                    }
                    else
                    {
                        numOfFrames++;
                    }
                }
                else
                {
                    numOfFrames = 0;
                }
            }
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }

        JSONReader.vid.players[creator.index].startTime = this.startTime.ToString();

        creator.renderNext();

    }

    bool matches(Color32 color, Color32 test)
    {
        int error = 4;
        if (color.g >= test.g -error && color.g <= test.g + error)
        {
            if(color.b >= test.b - error && color.b <= test.b + error)
            {
                if((color.r >= test.r - error && color.r <= test.r + error))
                {
                    return true;
                }
            }
        }
        return false;


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
        OnVideoPrepared(player);


        // Copy pixels from the render texture to the Texture2D
        texture2D.ReadPixels(new Rect(1600, 540, 1, 540), 0, 0);
        texture2D.Apply();
        

        for (int i = 0; i < 540; i++)
        {
            Color32 color = texture2D.GetPixel(0, i);
            if (color.Equals(Yellow))
            {
                i += 50;
            }
        }


        // Restore the previous render texture
        RenderTexture.active = currentRT;
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        // Initialize texture to the resolution of the video
        Destroy(texture2D);
        texture2D = new Texture2D((int)player.width, (int)player.GetComponent<VideoPlayer>().height, TextureFormat.RGB24, false);
    }
}
