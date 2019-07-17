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

        if (hasAuthority)
        {
            mySaver = FindObjectOfType<IDSaver>(); //server doesnt seem to run this, and maybe all of the stuff. check if it just start or if it the hasAuthority the ting fuggin up
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
        minigameManager.GetComponent<VoteManager>().VoteRecieved(i); //maybe make a findobjectoftype
        RpcBackToClientSendReady();
    }

    [ClientRpc]
    void RpcBackToClientSendReady()
    {
        hasInputted = true;
    }


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
        else
        {
            print("Manger Not Ready");
        }


    }

    void CalcPoints()
    {
        gainedPoints = 0;
        VoteManager mang = FindObjectOfType<VoteManager>();
        if (playerNum != sabNum) //not sab
        {
            print("AM GOOD POINTS");

            if (myVote == sabNum)
            {
                gainedPoints++;
            }

            if (mang.wholeTeamCorrect)
            {
                gainedPoints++;
            }
        }
        else if (playerNum == sabNum) //is sab
        {
            print("AM SAB POINTS");
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
        CmdSendPointGain(gainedPoints);
        pointsSent = true;
        FindObjectOfType<IDSaver>().points += gainedPoints; //BIG issues with mySaver in this area
        int tP = FindObjectOfType<IDSaver>().points;

        FindObjectOfType<VoteManager>().DisplayPoints(playerNum, gainedPoints, tP);

    }

    [Command]
    void CmdSendPointGain(int p)
    {
        gainedPoints = p;
        pointsSent = true;
        RpcSendOutPointGain(p);
    }

    [ClientRpc]
    void RpcSendOutPointGain(int p)
    {
        gainedPoints = p;
        pointsSent = true;

    }


    [ClientRpc]
    void RpcDebugPrint(string s)
    {
        print(s);
    }
}
