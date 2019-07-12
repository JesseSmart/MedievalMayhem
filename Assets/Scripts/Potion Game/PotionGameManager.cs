using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
public class PotionGameManager : MinigameInherit
{

    public GameObject cauldronObj;

    public Slider potionSlider;
    public TextMeshProUGUI tmpTimerText;

    private GameObject networkManagerObj;

    public float gameDuration;
    private float gameTimer;
    // Start is called before the first frame update
    void Start()
    {
        networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
        if (isServer)
        {
            PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed") + 1);

        }

        gameTimer = gameDuration;

    }

    // Update is called once per frame
    void Update()
    {
        potionSlider.value = cauldronObj.GetComponent<CauldronTrigger>().colorFloat;


        gameTimer -= Time.deltaTime;
        tmpTimerText.text = gameTimer.ToString("00:00");
        if (gameTimer <= 0)
        {
            //win
            GameEnd();
        }
    }

    public void GameEnd()
    {
        if (isServer)
        {
            if (PlayerPrefs.GetInt("GamesPlayed") < PlayerPrefs.GetInt("MaxGames")) //might be <=
            {
                networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene("Voting Scene");

            }
            else
            {
                //all games played, end game scene or whatever
                networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene("Menu");
            }

        }

    }
}
