using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BoatGameManager : MonoBehaviour
{


    public float gameDuration;
    public float gameTimer;

    public int loadScene;
    public int nextGameInt;

    public TextMeshProUGUI uiTimer;

    public Image identifierImageObject;
    public Sprite innocentIdentifierImage;
    public Sprite sabotagerIdentifierImage;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("isOnline") == 1)
        {
            OnlineSetter();
        }
        else
        {
            OfflineSetter();
        }


        gameTimer = gameDuration;
    }

    // Update is called once per frame
    void Update()
    {
        uiTimer.text = gameTimer.ToString("00:00");


        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0)
        {
            EndGame();
        }

    }

    private void EndGame()
    {
        SceneManager.LoadScene(loadScene);
        PlayerPrefs.SetInt("NextScene", nextGameInt);
    }

    private void OnlineSetter()
    {
        identifierImageObject.enabled = false;
    }

    private void OfflineSetter()
    {
        identifierImageObject.enabled = true;



    }
}
