using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;
using UnityEngine.UI;
using System;

public class MenuController : MonoBehaviour
{
    public GameObject controller;

    videoPlayer vp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vp = controller.GetComponent<videoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.pause();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            this.backTen();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            this.jumpTen();
        }
    }

    public void pause()
    {
        bool playing = false;
        if (vp.videoPlayers[0].GetComponent<VideoPlayer>().isPlaying) playing = true;

        for (int i = 0; i < vp.videoPlayers.Count; i++)
        {
            if (playing) vp.videoPlayers[i].GetComponent<VideoPlayer>().Pause();
            else vp.videoPlayers[i].GetComponent<VideoPlayer>().Play();
        }
        
            
    }
    public void backTen()
    {
        bool startTime = false;
        for(int i = 0;i < vp.videoPlayers.Count;i++)
        {
            if(vp.videoPlayers[i].GetComponent<VideoPlayer>().time - 10 < vp.videoPlayers[i].GetComponent<VOD>().startTime) startTime = true;

            if(startTime)
            {
                vp.videoPlayers[i].GetComponent<VideoPlayer>().time = vp.videoPlayers[i].GetComponent<VOD>().startTime;
            }
            else
            {
                vp.videoPlayers[i].GetComponent<VideoPlayer>().time = vp.videoPlayers[i].GetComponent<VideoPlayer>().time - 10;
            }
        }
    }

    public void jumpTen()
    {
        bool startTime = false;
        for (int i = 0; i < vp.videoPlayers.Count; i++)
        {
            vp.videoPlayers[i].GetComponent<VideoPlayer>().time = vp.videoPlayers[i].GetComponent<VideoPlayer>().time + 10;
        }
    }
}
