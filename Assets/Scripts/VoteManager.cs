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

	private bool coru1HasRun;

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
				if (!coru1HasRun)
				{
					StartCoroutine(WaitToUIAndScore());
					coru1HasRun = true;
				}
				//tempTimer -= Time.deltaTime;
				//if (tempTimer <= 0 && !uiChangedBool)
				//{
				//	RpcUIChange();
				//	RpcSendSetFinalScores();
				//	uiChangedBool = true;
				//}
			}

			if (uiChangedBool)
			{
				if (!loadNextHasRun)
				{
					StartCoroutine(WaitToLoad());
					//tempSecondTimer -= Time.deltaTime;
					//if (tempSecondTimer <= 0)
					//{
					//	LoadNextGame();
					//	loadNextHasRun = true;
					//}
					loadNextHasRun = true;
				}
			}
		}


	}
	//inputer has sent vote
	public void VoteRecieved(int suspect)
	{
		CmdVoteAdd(suspect);
	}
	//tell server recieved vote info
	[Command] //ERROR: CLIENTS CANT VOTE
	void CmdVoteAdd(int suspectedPlayer)
	{
		//if (isServer)
		//{
		RpcSendVoteInfo(suspectedPlayer);
		//}

	}
	//tell clients what was the vote
	[ClientRpc]
	void RpcSendVoteInfo(int sus)
	{
		voteTotalArray[sus] += 1;
		voteText[sus].text = voteTotalArray[sus].ToString();
		totalInputs += 1;
	}
	//has everyone voted?
	[ClientRpc]
	private void RpcVotingComplete()
	{
		print("VOTING COMPLETE");
		sabotagerIndicatorArray[sabPlayerNum].enabled = true;

		

		votingComplete = true;
		voteDoneRunBool = true;
	}

	//load next scene
	void LoadNextGame()
	{
        if (saver.gamesPlayed < saver.maxGames)//is < in votemanager because otherwise it would load another game
        {

            int rnd = Random.Range(0, 4);
            RpcSendNewSabNum(rnd);
			//delay load as issues arise if imidiate load
            Invoke("DoLoad", 1);

        }
        else
        {
            //HERE GOES "FINAL SCREEN" Load. From there, go to menu
            networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene("Winner Scene");
        }

	}
	//load
	void DoLoad()
	{
		networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(saver.levelNames[saver.levelLoadArray[saver.gamesPlayed]]);
	}
	//change ui
	[ClientRpc]
	void RpcUIChange()
	{
		votePanel.gameObject.SetActive(false);
		scorePanel.gameObject.SetActive(true);

	}
	//check bonus point possibilities, 
	void CalcPointStats()
	{
		if (voteTotalArray[sabPlayerNum] == 3) //Whole Team Correct
		{
			wholeTeamCorrect = true;
		}

		if (voteTotalArray[sabPlayerNum] == 0) //Whole Team Incorrect
		{
			wholeTeamWrong = true;
		}
		//what saboteur gains from incorrect votes
		sabPointGain = 0;
		foreach (int point in voteTotalArray)
		{
			sabPointGain += point;
		}

		sabPointGain -= voteTotalArray[sabPlayerNum] + 1; //the + 1 gets rid of the sab player's vote
		//did innocents or saboteur win
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
	//display points
    public void DisplayPoints(int pNum, int gainPoint, int totalPoint)
    {
        CmdDisplayPoints(pNum, gainPoint, totalPoint);
    }
	//tell server to display
    [Command]
    public void CmdDisplayPoints(int pNum, int gainPoint, int totalPoint) //this called from inputer, maybe do it other way //make void then call cmd
    {
		//RpcDebugText("Run DisplayPoints for pNum = " + pNum);
		print("Run DisplayPoints for pNum = " + pNum);
		RpcSendDisplayPoints(pNum, gainPoint, totalPoint);

    }
	//tell clients to display
    [ClientRpc]
    void RpcSendDisplayPoints(int pNum, int gainPoint, int totalPoint) 
    {
        print("TP: " + totalPoint + "GP: " + gainPoint);

        currentPoints[pNum].text = totalPoint.ToString();
        gainedPoints[pNum].text = gainPoint.ToString();
    }
	//send new sab num
    [ClientRpc]
    void RpcSendNewSabNum(int num)
    {
        FindObjectOfType<IDSaver>().sabNum = num;
    }
	//tell clients new sab num
    [ClientRpc]
    void RpcSendSetFinalScores()
    {
        uiChangedBool = true;
    }

	[ClientRpc]
	void RpcDebugText(string s)
	{
		print(s);
	}
	//wait to display score totals screen
	IEnumerator WaitToUIAndScore()
	{
		yield return new WaitForSecondsRealtime(4f);
		RpcUIChange();
		RpcSendSetFinalScores();
		uiChangedBool = true;
	}
	//wait to load next scene
	IEnumerator WaitToLoad()
	{
		yield return new WaitForSecondsRealtime(4f);
		LoadNextGame();

	}
}
