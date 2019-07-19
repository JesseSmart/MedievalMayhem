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
	private bool loadNextHasRun;

	private int sabPlayerNum;
	private IDSaver saver;

	//SCORE STUFF
    [SyncVar]
	public bool readyToRecievePoints;
	public bool wholeTeamCorrect;
	public bool wholeTeamWrong;
	public int sabPointGain;
    public bool teamDidWin;

	// Start is called before the first frame update
	void Start()
	{
		networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
		if (isServer)
		{
			saver = FindObjectOfType<IDSaver>();
			sabPlayerNum = saver.sabNum;
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
					CalcPointStats();

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
				if (!loadNextHasRun)
				{
					tempSecondTimer -= Time.deltaTime;
					if (tempSecondTimer <= 0)
					{
						LoadNextGame();
						loadNextHasRun = true;
					}
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
        if (saver.gamesPlayed < saver.maxGames)//is < in votemanager because otherwise it would load another game
        {

            int rnd = Random.Range(0, 3);
            RpcSendNewSabNum(rnd);
            Invoke("DoLoad", 1);
            //networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(saver.levelNames[saver.levelLoadArray[saver.gamesPlayed]]);

        }
        else
        {
            //HERE GOES "FINAL SCREEN" Load. From there, go to menu
            networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene("Menu");
        }

	}
	void DoLoad()
	{
		networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(saver.levelNames[saver.levelLoadArray[saver.gamesPlayed]]);
	}

	[ClientRpc]
	void RpcUIChange()
	{
		votePanel.gameObject.SetActive(false);
		scorePanel.gameObject.SetActive(true);

	}

	void CalcPointStats()
	{
		if (voteTotalArray[sabPlayerNum] == 3) //Whole Team Correct
		{
			wholeTeamCorrect = true;
		}

		if (voteTotalArray[sabPlayerNum] == 0) //Whole Team Correct
		{
			wholeTeamWrong = true;
		}

		sabPointGain = 0;
		foreach (int point in voteTotalArray)
		{
			sabPointGain += point;
		}

		sabPointGain -= voteTotalArray[sabPlayerNum] + 1; //the + 1 gets rid of the sab player's vote

        teamDidWin = false;
        if (FindObjectOfType<IDSaver>().lastGameTeamWon)
        {
            teamDidWin = true;
        }

		RpcSendPointStats(wholeTeamCorrect, wholeTeamWrong, sabPointGain, teamDidWin);
		//readyToRecievePoints = true;
	}

    //rpc maybe
    [ClientRpc]
    void RpcSendPointStats(bool cor, bool wrong, int sabPoint, bool winBool)
    {
		wholeTeamCorrect = cor;
		wholeTeamWrong = wrong;
		sabPointGain = sabPoint;
        teamDidWin = winBool;
		readyToRecievePoints = true;

		//if win lose

    }

    public void DisplayPoints(int pNum, int gainPoint, int totalPoint)
    {
        CmdDisplayPoints(pNum, gainPoint, totalPoint);
    }
    [Command]
    public void CmdDisplayPoints(int pNum, int gainPoint, int totalPoint) //this called from inputer, maybe do it other way //make void then call cmd
    {
		//currentPoints[pNum].text = totalPoint.ToString();
		//gainedPoints[pNum].text = gainPoint.ToString();
        //print("TP: " + totalPoint + "GP: " + gainPoint);
		RpcSendDisplayPoints(pNum, gainPoint, totalPoint);

    }

    [ClientRpc]
    void RpcSendDisplayPoints(int pNum, int gainPoint, int totalPoint) 
    {
        print("TP: " + totalPoint + "GP: " + gainPoint);

        currentPoints[pNum].text = totalPoint.ToString();
        gainedPoints[pNum].text = gainPoint.ToString();
    }

    [ClientRpc]
    void RpcSendNewSabNum(int num)
    {
        FindObjectOfType<IDSaver>().sabNum = num;
        //PlayerPrefs.SetInt("SabPlayerNumber", num);

    }

    [ClientRpc]
    void RpcSendSetFinalScores()
    {
        //foreach (VoteInputer inp in FindObjectsOfType<VoteInputer>())
        //{
        //    inp.SetTotalPoints();
        //}
        uiChangedBool = true;
    }

}
