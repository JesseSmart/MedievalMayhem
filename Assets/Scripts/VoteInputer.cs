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
    public bool hasRunPoints;

    public IDSaver mySaver;
    public bool pointsSent;
    // Start is called before the first frame update
    void Start()
    {
        minigameManager = GameObject.FindGameObjectWithTag("MinigameManager");

        if (hasAuthority || FindObjectsOfType<PlayerConnectedObject>().Length == 1)
        {
            mySaver = FindObjectOfType<IDSaver>(); 
            playerNum = mySaver.savedID - 1;
            print("playerNum = " + playerNum);
            CmdServerCharSetup(playerNum);
            sabNum = mySaver.sabNum;

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
            CheckIfPointReady();
        }

    }

    void InputManage(int pNum)
    {

        float horizontalInput = Input.GetAxis("DPadHorizontal");
        float verticalInput = Input.GetAxis("DPadVertical");

        #region //PC Controls
		//If buttin to vote pressed and has not alredy voted and vote is not for self
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

	//tell manager that i have voted
    [Command]
    void CmdSendReady(int i)
    {
        //hasInputted = true;
        minigameManager.GetComponent<VoteManager>().VoteRecieved(i); //maybe make a findobjectoftype
        RpcBackToClientSendReady();
    }
	//tell everyone that this has voted
    [ClientRpc]
    void RpcBackToClientSendReady()
    {
        hasInputted = true;
    }

	//wait until manager says to calculate points
    void CheckIfPointReady()
    {
        if (FindObjectOfType<VoteManager>().readyToRecievePoints)
        {
            if (!hasRunPoints)
            {
                CalcPoints();
                hasRunPoints = true;
            }
        }
    }

	//calculate points gained
    void CalcPoints()
    {
        gainedPoints = 0;
        VoteManager mang = FindObjectOfType<VoteManager>();

        print("Team Did Win: " + mang.teamDidWin);
        if (playerNum != sabNum) //I am not sabotuer
        {
			//objective successfl
            if (mang.teamDidWin)
            {
                gainedPoints++;
            }
			//vote correct
            if (myVote == sabNum)
            {
                gainedPoints++;
            }
			//everyone correct
            if (mang.wholeTeamCorrect)
            {
                gainedPoints++;
            }
        }
        else if (playerNum == sabNum) //is sab
        {
			//objective failed
            if (!(mang.teamDidWin))
            {
                gainedPoints++;
            }
			//not one voted correct
            if (mang.wholeTeamWrong)
            {
                gainedPoints++;
            }

            gainedPoints += mang.sabPointGain;
        }
        else
        {
            print("NOT SAB NOR PLAYER");
        }
       
        FindObjectOfType<IDSaver>().points += gainedPoints; 
        int tP = FindObjectOfType<IDSaver>().points;

		//tell manager info
		FindObjectOfType<VoteManager>().currentPoints[playerNum].text = tP.ToString();
		FindObjectOfType<VoteManager>().gainedPoints[playerNum].text = gainedPoints.ToString();
		CmdSendPointGain(gainedPoints, gainedPoints, tP);
	}
	//tell clients to show points
    [Command]
    void CmdSendPointGain(int p, int gained, int total)
    {

        RpcSendOutPointGain(p, gained, total);
    }
	//show my points info
    [ClientRpc]
    void RpcSendOutPointGain(int p, int gained, int total)
    {

		FindObjectOfType<VoteManager>().currentPoints[playerNum].text = total.ToString();
		FindObjectOfType<VoteManager>().gainedPoints[playerNum].text = gained.ToString();

		gainedPoints = p;
        pointsSent = true;

    }
	//debug
	[Command]
	void CmdDebug(string s)
	{
		print(s);
	}
	//debug
    [ClientRpc]
    void RpcDebugPrint(string s)
    {
        print(s);
    }
}
