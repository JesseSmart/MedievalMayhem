using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VoteInputer : NetworkBehaviour
{
	public int playerNum;
	public bool hasInputted;
	public GameObject minigameManager;
	private int myVote;
	private int gainedPoints;



	// Start is called before the first frame update
	void Start()
	{
		minigameManager = GameObject.FindGameObjectWithTag("MinigameManager");

		if (hasAuthority)
		{
			playerNum = FindObjectOfType<IDSaver>().savedID - 1;
			CmdServerCharSetup(playerNum);
		}
	}

	[Command]
	void CmdServerCharSetup(int id)
	{
		playerNum = id;
		RpcBackToClientSetup(id);
	}

	[ClientRpc]
	void RpcBackToClientSetup(int id)
	{
		playerNum = id;
	}



	// Update is called once per frame
	void Update()
	{
		if (hasAuthority)
		{
			InputManage(playerNum);
		}

	}

	void InputManage(int pNum)
	{
		#region //PC Controls
		if (Input.GetKeyDown(KeyCode.Alpha1) && hasInputted == false && FindObjectOfType<IDSaver>().savedID != 1)
		{
			print("Player " + pNum + ": Pressed 1");
			hasInputted = true;
			myVote = 0;
			CmdSendReady(0);

		}

		if (Input.GetKeyDown(KeyCode.Alpha2) && hasInputted == false && FindObjectOfType<IDSaver>().savedID != 2)
		{
			print("Player " + pNum + ": Pressed 2");
			hasInputted = true;
			myVote = 1;
			CmdSendReady(1);

		}
		if (Input.GetKeyDown(KeyCode.Alpha3) && hasInputted == false && FindObjectOfType<IDSaver>().savedID != 3)
		{

			print("Player " + pNum + ": Pressed 3");
			hasInputted = true;
			myVote = 2;
			CmdSendReady(2);

		}

		if (Input.GetKeyDown(KeyCode.Alpha4) && hasInputted == false && FindObjectOfType<IDSaver>().savedID != 4)
		{
			print("Player " + pNum + ": Pressed 4");
			hasInputted = true;
			myVote = 3;
			CmdSendReady(3);
		}
		#endregion



		//if (Input.GetButtonDown(aButtonArray[pNum]) && hasInputted[pNum] == false)
		//{
		//    print("Player " + pNum + ": Pressed A");
		//    parentObject.GetComponent<VoteManager>().VoteAdd(0);
		//    hasInputted[pNum] = true;

		//}

		//if (Input.GetButtonDown(xButtonArray[pNum]) && hasInputted[pNum] == false)
		//{
		//    print("Player " + pNum + ": Pressed X");
		//    parentObject.GetComponent<VoteManager>().VoteAdd(1);
		//    hasInputted[pNum] = true;


		//}

		//if (Input.GetButtonDown(yButtonArray[pNum]) && hasInputted[pNum] == false)
		//{
		//    print("Player " + pNum + ": Pressed Y");
		//    parentObject.GetComponent<VoteManager>().VoteAdd(2);
		//    hasInputted[pNum] = true;


		//}

		//if (Input.GetButtonDown(bButtonArray[pNum]) && hasInputted[pNum] == false)
		//{
		//    print("Player " + pNum + ": Pressed B");
		//    parentObject.GetComponent<VoteManager>().VoteAdd(3);
		//    hasInputted[pNum] = true;


		//}
	}

	[Command]
	void CmdSendReady(int i)
	{
		hasInputted = true;
		minigameManager.GetComponent<VoteManager>().VoteRecieved(i);
		RpcBackToClientSendReady();
	}

	[ClientRpc]
	void RpcBackToClientSendReady()
	{
		hasInputted = true;
	}


	public void RecieveWholeTeamBonus(int p, int sabNum) //hasAuthority?
	{
		if (FindObjectOfType<IDSaver>().savedID != (sabNum - 1))
		{
			gainedPoints += p;

		}
	}

	public void RecieveGreatSabBonus(int p, int sabNum)
	{
		if (FindObjectOfType<IDSaver>().savedID == (sabNum - 1))
		{
			gainedPoints += p;

		}
	}

	public void RecieveBasePoints(int p, int sabNum)
	{
		if (myVote == (sabNum - 1))
		{
			gainedPoints += p;
		}


	}

	public void RecieveSabPoints(int p, int sabNum)
	{
		if (FindObjectOfType<IDSaver>().savedID == (sabNum - 1))
		{
			gainedPoints += p;
		}
	}

	public void SetTotalPoints()
	{
		FindObjectOfType<IDSaver>().points += gainedPoints;
		minigameManager.GetComponent<VoteManager>().CmdDisplayPoints(FindObjectOfType<IDSaver>().savedID, gainedPoints, FindObjectOfType<IDSaver>().points);
	}

}
