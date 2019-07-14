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

    private int sabNum;

	// Start is called before the first frame update
	void Start()
	{
		minigameManager = GameObject.FindGameObjectWithTag("MinigameManager");

		if (hasAuthority)
		{
			playerNum = FindObjectOfType<IDSaver>().savedID - 1;
			CmdServerCharSetup(playerNum);
            sabNum = FindObjectOfType<IDSaver>().sabNum;
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

        float horizontalInput = Input.GetAxis("DPadHorizontal");
        float verticalInput = Input.GetAxis("DPadVertical");

        #region //PC Controls
        if ((Input.GetKeyDown(KeyCode.Alpha1) || verticalInput > 0) && hasInputted == false && FindObjectOfType<IDSaver>().savedID != 1)
		{
			hasInputted = true;
			myVote = 0;
			CmdSendReady(0);

		}

		if ((Input.GetKeyDown(KeyCode.Alpha2) || horizontalInput > 0) && hasInputted == false && FindObjectOfType<IDSaver>().savedID != 2)
		{
			hasInputted = true;
			myVote = 1;
			CmdSendReady(1);

		}
		if ((Input.GetKeyDown(KeyCode.Alpha3) || verticalInput < 0) && hasInputted == false && FindObjectOfType<IDSaver>().savedID != 3)
		{

			hasInputted = true;
			myVote = 2;
			CmdSendReady(2);

		}

		if ((Input.GetKeyDown(KeyCode.Alpha4) || horizontalInput < 0) && hasInputted == false && FindObjectOfType<IDSaver>().savedID != 4)
		{
			hasInputted = true;
			myVote = 3;
			CmdSendReady(3);
		}
		#endregion


	}

    //could have void which calls all voids to do with score. Would have to change alot

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


    
	public void RecieveWholeTeamBonus(int p) //hasAuthority?
	{
        if (hasAuthority)
        {
		    if (FindObjectOfType<IDSaver>().savedID != (sabNum - 1))
		    {
			    gainedPoints += p;
		    }
        }
	}

	public void RecieveGreatSabBonus(int p)
	{
        if (hasAuthority)
        {
		    if (FindObjectOfType<IDSaver>().savedID == (sabNum - 1))
		    {
			    gainedPoints += p;

		    }

        }
	}

	public void RecieveBasePoints(int p)
	{
        if (hasAuthority)
        {
            if (myVote == (sabNum - 1))
		    {
			    gainedPoints += p;
		    }

        }


	}

	public void RecieveSabPoints(int p)
	{
        if (hasAuthority)
        {
            if (FindObjectOfType<IDSaver>().savedID == (sabNum - 1))
		    {
			    gainedPoints += p;
		    }

        }
	}

	public void SetTotalPoints()
	{
        if (hasAuthority)
        {
		    FindObjectOfType<IDSaver>().points += gainedPoints;
		    minigameManager.GetComponent<VoteManager>().CmdDisplayPoints(playerNum, gainedPoints, FindObjectOfType<IDSaver>().points);

        }
	}

}
