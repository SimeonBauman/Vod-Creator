using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject controller;

    videoPlayer vp;

    public GameObject[] menu;

    float lastMouseMove;
    Vector3 lastMousePosition;

    public GameObject pauseIcon;
    public GameObject playIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vp = controller.GetComponent<videoPlayer>();
        this.hideMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (!videoPlayer.preping)
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

            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

            if (mouseDelta != Vector3.zero)
            {
                this.showMenu();
                lastMousePosition = Input.mousePosition;
                lastMouseMove = Time.time;
            }

            if (Time.time - lastMouseMove > 1.5f)
            {
                this.hideMenu();
            }
        }

    }

    public void showMenu()
    {
        for(int i = 0; i < menu.Length; i++)
        {
            menu[i].SetActive(true);
        }
    }

    public void hideMenu()
    {
        for (int i = 0; i < menu.Length; i++)
        {
            menu[i].SetActive(false);
        }
    }

    public void pause()
    {
        bool playing = false;
        if (vp.videoPlayers[0].GetComponent<VideoPlayer>().isPlaying) playing = true;

        for (int i = 0; i < vp.videoPlayers.Count; i++)
        {
            if (playing)
            {
                vp.videoPlayers[i].GetComponent<VideoPlayer>().Pause();
                pauseIcon.SetActive(false);
                playIcon.SetActive(true);

            }
            else
            {
                vp.videoPlayers[i].GetComponent<VideoPlayer>().Play();
                
                pauseIcon.SetActive(true);
                playIcon.SetActive(false);
            }
        }
        
        this.showMenu();
        lastMouseMove = Time.time;

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
        
        for (int i = 0; i < vp.videoPlayers.Count; i++)
        {
            vp.videoPlayers[i].GetComponent<VideoPlayer>().time = vp.videoPlayers[i].GetComponent<VideoPlayer>().time + 10;
        }
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
