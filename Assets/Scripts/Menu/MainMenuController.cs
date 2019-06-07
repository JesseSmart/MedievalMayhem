﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{

    public int loadScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnlinePlayPressed()
    {
        PlayerPrefs.SetInt("isOnline", 1);

        SceneManager.LoadScene(loadScene);
    }

    public void OfflinePlayPressed()
    {
        PlayerPrefs.SetInt("isOnline", 0);

        SceneManager.LoadScene(loadScene);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}