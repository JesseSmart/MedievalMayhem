using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class VoteManager : MinigameInherit
{
    public GameObject votePanel;
    public GameObject scorePanel;

    public TextMeshProUGUI[] voteText;
    public TextMeshProUGUI[] currentPoints;
    public TextMeshProUGUI[] gainedPoints;
    public int[] voteTotalArray = new int[4] { 0, 0, 0, 0 };
    private int totalInputs;

    public Image[] sabotagerIndicatorArray;

    private float tempTimer = 5; //debug
    private float tempSecondTimer = 5;
    private GameObject networkManagerObj;

    private bool votingComplete;
    private bool voteDoneRunBool;
    private bool uiChangedBool;

    private int sabPlayerNum;

    // Start is called before the first frame update
    void Start()
    {
        networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
        if (isServer)
        {
            sabPlayerNum = FindObjectOfType<IDSaver>().sabNum;
        }
        sabPlayerNum = FindObjectOfType<IDSaver>().sabNum;

    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            if (totalInputs >= 4)
            {
                if (voteDoneRunBool == false)
                {
                    RpcVotingComplete();
                    RpcPointCalculate();

                    voteDoneRunBool = true;
                }
            }

            if (votingComplete)
            {
                tempTimer -= Time.deltaTime;
                if (tempTimer <= 0 && !uiChangedBool)
                {
                    RpcUIChange();
                    RpcSendSetFinalScores();


                    uiChangedBool = true;
                }
            }

            if (uiChangedBool)
            {
                tempSecondTimer -= Time.deltaTime;
                if (tempSecondTimer <= 0)
                {
                    LoadNextGame();
                }
            }
        }
        

    }

    public void VoteRecieved(int suspect)
    {
        CmdVoteAdd(suspect);
    }

    [Command] //ERROR: CLIENTS CANT VOTE
    void CmdVoteAdd(int suspectedPlayer)
    {
        //if (isServer)
        //{
        RpcSendVoteInfo(suspectedPlayer);
        //}

    }

    [ClientRpc]
    void RpcSendVoteInfo(int sus)
    {
        voteTotalArray[sus] += 1;
        voteText[sus].text = voteTotalArray[sus].ToString();
        totalInputs += 1;
    }

    [ClientRpc]
    private void RpcVotingComplete()
    {
        print("VOTING COMPLETE");
        sabotagerIndicatorArray[sabPlayerNum].enabled = true;
        votingComplete = true;
        voteDoneRunBool = true;
    }


    void LoadNextGame()
    {
        int rnd = Random.Range(0, 3);
        PlayerPrefs.SetInt("SabPlayerNumber", rnd);
        RpcSendNewSabNum(rnd);
        networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(PlayerPrefs.GetString("LevelName" + PlayerPrefs.GetInt("LevelLoad" + PlayerPrefs.GetInt("GamesPlayed"))));

    }

    [ClientRpc]
    void RpcUIChange()
    {
        votePanel.gameObject.SetActive(false);
        scorePanel.gameObject.SetActive(true);

    }


    //rpc maybe
    [ClientRpc]
    void RpcPointCalculate()
    {
        VoteInputer[] inputers = FindObjectsOfType<VoteInputer>();


        if (voteTotalArray[sabPlayerNum] == 3) //Whole Team Correct
        {
            foreach (VoteInputer inp in inputers)
            {
                inp.RecieveWholeTeamBonus(1);
            }
        }
        else if (voteTotalArray[sabPlayerNum] == 0) //No One Correct
        {
            foreach (VoteInputer inp in inputers)
            {
                inp.RecieveGreatSabBonus(1);
            }
        }

        //base
        foreach (VoteInputer inp in inputers)
        {
            inp.RecieveBasePoints(1);
        }

        //base SAB
        foreach (VoteInputer inp in inputers)
        {
            //change points to non correct votes
            int pointGain = 0;

            foreach (int point in voteTotalArray)
            {
                pointGain += point;
            }

            pointGain -= voteTotalArray[sabPlayerNum];

            inp.RecieveBasePoints(pointGain);
        }

        //if win

        //if lose

    }

    [Command]
    public void CmdDisplayPoints(int pNum, int totalPoint, int gainPoint)
    {
        RpcSendDisplayPoints(pNum, totalPoint, gainPoint);

    }

    [ClientRpc]
    void RpcSendDisplayPoints(int pNum, int totalPoint, int gainPoint)
    {
        currentPoints[pNum].text = totalPoint.ToString();
        gainedPoints[pNum].text = gainPoint.ToString();
    }

    [ClientRpc]
    void RpcSendNewSabNum(int num)
    {
        FindObjectOfType<IDSaver>().sabNum = num;
        PlayerPrefs.SetInt("SabPlayerNumber", num);

    }

    [ClientRpc]
    void RpcSendSetFinalScores()
    {
        foreach (VoteInputer inp in FindObjectsOfType<VoteInputer>())
        {
            inp.SetTotalPoints();
        }
        uiChangedBool = true;
    }

}
