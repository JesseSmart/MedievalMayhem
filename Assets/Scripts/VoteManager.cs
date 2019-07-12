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
			sabPlayerNum = PlayerPrefs.GetInt("SabPlayerNumber");
		}


	}

	// Update is called once per frame
	void Update()
	{

		if (totalInputs >= 4)
		{
			if (voteDoneRunBool == false)
			{
				VotingComplete();
				voteDoneRunBool = true;
			}
		}

		if (votingComplete)
		{
			tempTimer -= Time.deltaTime;
			if (tempTimer <= 0 && !uiChangedBool)
			{
				UIChange();

				foreach(VoteInputer inp in FindObjectsOfType<VoteInputer>())
				{
					inp.SetTotalPoints();
				}

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


	private void VotingComplete()
	{
		print("VOTING COMPLETE");
		sabotagerIndicatorArray[sabPlayerNum].enabled = true;
		PointCalculate();
		votingComplete = true;
	}


	void LoadNextGame()
	{
		PlayerPrefs.SetInt("SabPlayerNumber", Random.Range(0, 3));
		networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(PlayerPrefs.GetString("LevelName" + PlayerPrefs.GetInt("LevelLoad" + PlayerPrefs.GetInt("GamesPlayed"))));

	}

	void UIChange()
	{
		votePanel.gameObject.SetActive(false);
		scorePanel.gameObject.SetActive(true);

	}


	//rpc maybe
	void PointCalculate()
	{
		VoteInputer[] inputers = FindObjectsOfType<VoteInputer>();
		

		if (voteTotalArray[sabPlayerNum] == 3) //Whole Team Correct
		{
			foreach (VoteInputer inp in inputers)
			{
				inp.RecieveWholeTeamBonus(1, sabPlayerNum);
			}
		}
		else if (voteTotalArray[sabPlayerNum] == 0) //No One Correct
		{
			foreach (VoteInputer inp in inputers)
			{
				inp.RecieveGreatSabBonus(1, sabPlayerNum);
			}
		}

		//base
		foreach (VoteInputer inp in inputers)
		{
			inp.RecieveBasePoints(1, sabPlayerNum);
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

			inp.RecieveBasePoints(pointGain, sabPlayerNum);
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




}
