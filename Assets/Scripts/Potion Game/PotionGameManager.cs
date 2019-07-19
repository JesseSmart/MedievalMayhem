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
    private IDSaver saver;

    // Start is called before the first frame update
    void Start()
    {
        networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
        if (isServer)
        {
            PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed") + 1);
            saver = FindObjectOfType<IDSaver>();
            saver.gamesPlayed++;

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
            CmdSetWinState(true);
            GameEnd();
            gameTimer = 100;
        }
    }

    public void CauldronBlew() //make sure only runs once
    {
        CmdSetWinState(false);
        GameEnd();
    }

    public void GameEnd()
    {
        if (isServer)
        {
            if (saver.gamesPlayed <= saver.maxGames) //might be <=
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

    [Command]
    void CmdSetWinState(bool b)
    {
        RpcSendOutWinState(b);
    }

    [ClientRpc]
    void RpcSendOutWinState(bool b)
    {
        FindObjectOfType<IDSaver>().lastGameTeamWon = b;
    }
}
