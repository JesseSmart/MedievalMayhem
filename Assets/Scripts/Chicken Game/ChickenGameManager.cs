using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
public class ChickenGameManager : MinigameInherit
{

    private int gameScore;
    public int targetScore;

    public TextMeshProUGUI tmpCurrentScore;
    public TextMeshProUGUI tmpTargetScore;
    public TextMeshProUGUI tmpTimerText;

    public float matchDuration;
    private float matchTimer;

    private GameObject networkManagerObj;

    private IDSaver saver;
    // Start is called before the first frame update
    void Start()
    {
        matchTimer = matchDuration;

        networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");

        if (isServer)
        {
            saver = FindObjectOfType<IDSaver>();
            PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed") + 1);
            saver.gamesPlayed++;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (gameScore >= targetScore)
        {
            //win game
            GameEnd();
        }

        matchTimer -= Time.deltaTime;
        if (matchTimer <= 0)
        {
            //game lost
            GameEnd();
            matchTimer = 100;
        }
        else
        {
            tmpTimerText.text = matchTimer.ToString("00:00");
        }

        tmpCurrentScore.text = gameScore.ToString();
        tmpTargetScore.text = targetScore.ToString();
    }

    public void GainPoint()
    {
        gameScore += 1;
    }

    public void GameEnd()
    {
        if (isServer)
        {
            if (saver.gamesPlayed < saver.maxGames) //might be <=
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
