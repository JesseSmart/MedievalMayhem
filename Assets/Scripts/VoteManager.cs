using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class VoteManager : MinigameInherit
{

    public TextMeshProUGUI[] voteText;
    public int[] voteTotalArray = new int[4] { 0,0,0,0};
    private int totalInputs;

    public Image[] sabotagerIndicatorArray;

    private float tempTimer = 5; //debug
    private GameObject networkManagerObj;
    private bool votingComplete;
    // Start is called before the first frame update
    void Start()
    {
        networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");


    }

    // Update is called once per frame
    void Update()
    {
        if (totalInputs >= 4)
        {
            VotingComplete();
        }

        if (votingComplete)
        {
            tempTimer -= Time.deltaTime;
            if (tempTimer <= 0)
            {
                LoadNextGame();
            }
        }

    }

    [Command] //ERROR: CLIENTS CANT VOTE
    public void CmdVoteAdd(int suspectedPlayer)
    {
        if (isServer)
        {
            RpcSendVoteInfo(suspectedPlayer);
        }

    }

    [ClientRpc]
    void RpcSendVoteInfo(int sus)
    {
        voteTotalArray[sus] += 1;
        voteText[sus].text = voteTotalArray[sus].ToString();
        totalInputs += 1;
    }


    private void VotingComplete()
    {
        print("VOTING COMPLETE");
        sabotagerIndicatorArray[PlayerPrefs.GetInt("SabPlayerNumber")].enabled = true;
        votingComplete = true;
    }


    void LoadNextGame()
    {
        PlayerPrefs.SetInt("SabPlayerNum", Random.Range(0, 3));
        networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(PlayerPrefs.GetString("LevelName" + PlayerPrefs.GetInt("LevelLoad" + PlayerPrefs.GetInt("GamesPlayed"))));

    }


}
