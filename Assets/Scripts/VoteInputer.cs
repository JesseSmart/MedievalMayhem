using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class VoteInputer : NetworkBehaviour
{
    public int playerNum;
    public bool hasInputted;
    public GameObject minigameManager;





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
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasInputted == false)
        {
            print("Player " + pNum + ": Pressed 1");
            minigameManager.GetComponent<VoteManager>().CmdVoteAdd(0);
            hasInputted = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && hasInputted == false)
        {
            print("Player " + pNum + ": Pressed 2");
            minigameManager.GetComponent<VoteManager>().CmdVoteAdd(1);
            hasInputted = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasInputted == false)
        {
            print("Player " + pNum + ": Pressed 3");
            minigameManager.GetComponent<VoteManager>().CmdVoteAdd(2);
            hasInputted = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && hasInputted == false)
        {
            print("Player " + pNum + ": Pressed 4");
            minigameManager.GetComponent<VoteManager>().CmdVoteAdd(3);
            hasInputted = true;
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


}
