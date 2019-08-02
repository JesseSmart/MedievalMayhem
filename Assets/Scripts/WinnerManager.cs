using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class WinnerManager : MinigameInherit
{

	public TextMeshProUGUI[] winnerNumTexts;
	public TextMeshProUGUI[] playerPointsText;
	public int[] playerPoints = new int[4] { 0,0,0,0};
	public int[] pWinOrder = new int[4] { 1, 2, 3, 4};


	private GameObject networkManagerObj;

	private bool tempHasRun;
    private bool playerCheckComplete;
	public float sceneTimer = 10;

	//save data
	public int totalWins;
	public int totalLosses;

    // Start is called before the first frame update
    void Start()
    {
		networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");


		if (isServer)
		{

		}
    }

    // Update is called once per frame
    void Update()
    {
		if (isServer)
		{
            
			WinnerCharacterController[] players = FindObjectsOfType<WinnerCharacterController>();
			int tempInt = 0;
			for (int i = 0; i < players.Length; i++)
			{
				if (players[i].pointSent)
				{
					tempInt++;
				}
			}

			if (tempInt >= 4)
			{
				if (!tempHasRun)
				{
					for (int i = 0; i < players.Length; i++)
					{
						RpcSetPointArray(playerPoints[i], i);
					}

					SortArray();

					for (int i = 0; i < players.Length; i++)
					{
						RpcSetNumArrays(i, playerPoints[i], pWinOrder[i]);
					}

					for (int i = 0; i < players.Length; i++)
					{
                        RpcSetTextArrays(i);
					}

					//
					//RpcLoadPlayer();
					//RpcSavePlayer();

					tempHasRun = true;
				}
			}
			else
			{
				tempInt =  0;
			}

            if (tempHasRun)
            {
			    sceneTimer -= Time.deltaTime;
			    if (sceneTimer <= 0)
			    {
				    networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene("Menu");
				    sceneTimer = 100;
			    }

            }
		}
    }

	void SortArray()
	{

		for (int j = 0; j < playerPoints.Length - 1; j++)
		{
			for (int i = 0; i < playerPoints.Length - j - 1; i++)
			{
				if (playerPoints[i] < playerPoints[i + 1]) //was <
				{
					//RpcSwap(i);
					int tempInt;
					tempInt = playerPoints[i + 1];
					playerPoints[i + 1] = playerPoints[i];
					playerPoints[i] = tempInt;


					//Change player number order in array to match point arrangement
					int otherInt;
					otherInt = pWinOrder[i + 1];
					pWinOrder[i + 1] = pWinOrder[i];
					pWinOrder[i] = otherInt;
				}
			}
		}
	}

	[ClientRpc]
	void RpcSetPointArray(int point, int ind)
	{
		playerPoints[ind] = point;
	}

	[ClientRpc]
	void RpcSwap(int ind)
	{
		int tempInt;
		tempInt = playerPoints[ind + 1];
		playerPoints[ind + 1] = playerPoints[ind];
		playerPoints[ind] = tempInt;


		//Change player number order in array to match point arrangement
		int otherInt;
		otherInt = pWinOrder[ind + 1];
		pWinOrder[ind + 1] = pWinOrder[ind];
		pWinOrder[ind] = otherInt;

		//int tempInt;
		//tempInt = playerPoints[ind];
		//playerPoints[ind] = playerPoints[ind + 1];
		//playerPoints[ind + 1] = tempInt;


		////Change player number order in array to match point arrangement
		//int otherInt;
		//otherInt = pWinOrder[ind];
		//pWinOrder[ind] = pWinOrder[ind + 1];
		//pWinOrder[ind + 1] = otherInt;
	}

	[ClientRpc]
	void RpcSetNumArrays(int index, int pPoint, int wOrd)
	{
		playerPoints[index] = pPoint;
		pWinOrder[index] = wOrd;
	}


	[ClientRpc]
    void RpcSetTextArrays(int index)
    {
        winnerNumTexts[index].text = "Player " + pWinOrder[index].ToString();
        playerPointsText[index].text = playerPoints[index].ToString() + "P";
    }

	//[ClientRpc]
	//void RpcSavePlayer()
	//{
	//	if (FindObjectOfType<IDSaver>().savedID - 1 == pWinOrder[0])
	//	{
	//		totalWins++;
	//	}
	//	else
	//	{
	//		totalLosses--;
	//	}
	//	print("Total Wins: " + totalWins + " || Total Losses: " + totalLosses);
	//	SaveSystem.SavePlayer(this);
	//}

	//[ClientRpc]
	//void RpcLoadPlayer()
	//{
	//	PlayerData data = SaveSystem.LoadPlayer();
	//	if (data == null)
	//	{
	//		data = new PlayerData();
	//	}

		
	//	totalWins = data.totalWins;
	//	totalLosses = data.totalLosses;
	//	print("Loaded Wins: " + totalWins + " || Loaded Losses: " + totalLosses);

	//}
}
