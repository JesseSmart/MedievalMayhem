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
					SortArray();

					for (int i = 0; i < players.Length; i++)
					{
                        RpcSetArrays(i);

					}
					tempHasRun = true;
				}
			}
			else
			{
				tempInt =  0;
			}

            //if all players have PointSet = true
            //Sort playerPoints[]
            // Rearrange pWinOrder[] in same order
            //Displayer the two arrays in their corresponding text arrays







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
    void RpcSetArrays(int index)
    {
        winnerNumTexts[index].text = "Player " + pWinOrder[index].ToString();
        playerPointsText[index].text = playerPoints[index].ToString() + "P";
    }
}
