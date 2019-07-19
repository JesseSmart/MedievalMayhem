using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class WinnerCharacterController : NetworkBehaviour
{
	public int playerNum;
	private GameObject minigameManager;

	public bool pointSent;
	// Start is called before the first frame update
	void Start()
    {
		minigameManager = GameObject.FindGameObjectWithTag("MinigameManager");

		if (hasAuthority || FindObjectsOfType<PlayerConnectedObject>().Length == 1)
		{
			playerNum = FindObjectOfType<IDSaver>().savedID - 1;


			int myPoints = FindObjectOfType<IDSaver>().points;
			CmdSendPointTotal(playerNum, myPoints);
		}
	}

	// Update is called once per frame
	void Update()
    {
		if (hasAuthority)
		{

		}
    }

	[Command]
	void CmdSendPointTotal(int pNum, int point)
	{
		minigameManager.GetComponent<WinnerManager>().playerPoints[pNum] = point;
		//minigameManager.GetComponent<WinnerManager>().winnerNumTexts[pNum].text = (pNum + 1).ToString();
		//minigameManager.GetComponent<WinnerManager>().playerPointsText[pNum].text = point.ToString();
		RpcSendOutPoint(pNum, point);
	}

	[ClientRpc]
	void RpcSendOutPoint(int pNum, int point)
	{
		minigameManager.GetComponent<WinnerManager>().playerPoints[pNum] = point;
		//minigameManager.GetComponent<WinnerManager>().winnerNumTexts[pNum].text = (pNum + 1).ToString();
		//minigameManager.GetComponent<WinnerManager>().playerPointsText[pNum].text = point.ToString();
		pointSent = true;

	}
}
