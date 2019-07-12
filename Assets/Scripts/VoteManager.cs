using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class VoteManager : MinigameInherit
{

	public TextMeshProUGUI[] voteText;
	public int[] voteTotalArray = new int[4] { 0, 0, 0, 0 };
	private int totalInputs;

	public Image[] sabotagerIndicatorArray;

	private float tempTimer = 5; //debug
	private GameObject networkManagerObj;
	private bool votingComplete;
	private bool voteDoneRunBool;
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
			if (voteDoneRunBool == false)
			{
				VotingComplete();
				voteDoneRunBool = true;
			}
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
		sabotagerIndicatorArray[PlayerPrefs.GetInt("SabPlayerNumber")].enabled = true;
		PointCalculate();
		votingComplete = true;
	}


	void LoadNextGame()
	{
		PlayerPrefs.SetInt("SabPlayerNumber", Random.Range(0, 3));
		networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(PlayerPrefs.GetString("LevelName" + PlayerPrefs.GetInt("LevelLoad" + PlayerPrefs.GetInt("GamesPlayed"))));

	}


	//rpc maybe
	void PointCalculate()
	{
		VoteInputer[] inputers = FindObjectsOfType<VoteInputer>();
		int sabPlayerNum = PlayerPrefs.GetInt("SabPlayerNumber");

		if (voteTotalArray[sabPlayerNum] == 3) //Whole Team Correct
		{
			foreach (VoteInputer inp in inputers)
			{
				inp.RecieveWholeTeamBonus(1, sabPlayerNum);
			}
		}
		else if (voteTotalArray[PlayerPrefs.GetInt("SabPlayerNumber")] == 0) //No One Correct
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



}
