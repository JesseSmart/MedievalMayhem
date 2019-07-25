using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerConnectedObject : NetworkBehaviour
{
    public GameObject playerUnitPrefab;

    private GameObject boatObj;
    //private GameObject[] networkPlayerObjs;

    private GameObject minigameManagerObj;
    public GameObject[] minigameCharacterControllersObjs;
    private int contArrayOffset; //the total of non minigame scenes

	public bool isMe;

    [SyncVar]
	public int playerID;

	public bool isReadyLobby;
	public bool isReadyGame;

	private bool gameHasStarted;

	public bool hasSpawned = false;

    //private PlayerConnectedObject[] pConObjs;
    // Start is called before the first frame update
    void Start()
    {

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {

            if (isLocalPlayer)
            {
                playerID = FindObjectsOfType<PlayerConnectedObject>().Length;
                FindObjectOfType<IDSaver>().savedID = playerID;                
                CmdSetPlayerNumber(playerID);
            }
        }
        else
        {
            if (isServer)
            {
                RpcGameSetPlayerNum();
            }
        }


		minigameManagerObj = GameObject.FindGameObjectWithTag("MinigameManager");

        if (!isLocalPlayer)
		{
			return;
		}


		isMe = true;


        //CheckMinigame();

    }

    // Update is called once per frame
    void Update()
    {
		if (!isLocalPlayer)
		{
			return;
		}

		//lobby
		if (!FindObjectOfType<MinigameInherit>())
		{
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonUp("P1AButton"))
			{

                isReadyLobby = true;
                CmdLobbyReady();
			}
		}
		else if (!gameHasStarted && FindObjectOfType<MinigameInherit>())
		{
			print("CHECKMINIGAME");
			CheckMinigame();
		}

	}

	[Command]
	void CmdLobbyReady()
	{
        isReadyLobby = true;

        if (isServer) //Is necassary but worked before so maybe cause error in future
        {
		    RpcLobbyReady();

        }
	}

	[ClientRpc]
	void RpcLobbyReady()
	{
		isReadyLobby = true;
	}



    void CheckMinigame()
    {
        int sceneNum = SceneManager.GetActiveScene().buildIndex;

        switch (sceneNum)
        {
            case 0:
                //main menu
                break;
            case 1:
                //character select
                //JoinLobbySetup();
                break;
            case 2:
                //minigame 1
                //MGOneSetup();
                GeneralGameSetup(1);
                break;
            case 3:
				//MGTwoSetup();
                GeneralGameSetup(2);
                break;
            case 4:
				//MGThreeSetup();
                GeneralGameSetup(3);
                break;
            case 5:
                //VotingSceneSetup();
                GeneralGameSetup(4);
                break;
            case 6:
				//WinnerSceneSetup();
                GeneralGameSetup(5);
                break;

        }
    }

    void JoinLobbySetup()
    {
        //obselete
    }


    void MGOneSetup()
    {

		if (isLocalPlayer && !hasSpawned) //used to be isClient, have not yet check for errors
		{
			ClientReady(1);
			hasSpawned = true;
            CmdSendSpawned();
		}


        //if (isServer) THIS SEEMS LIKE BIG CHANGE BUT PROGRAM SEEMS INDIFERENT BUT KEEP EYE ON IT
        //{
        PlayerConnectedObject[] players = FindObjectsOfType<PlayerConnectedObject>();
        int ind = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].hasSpawned)
            {
                ind++;
            }
        }

        if (ind >= 4)
        {
            gameHasStarted = true;
        }
        else
        {
            ind = 0;
        }

        //if (players.Length >= 3) //do better spawning
        //{
        //	gameHasStarted = true;
        //}
        //}
    }

	void MGTwoSetup()
	{
		if (isLocalPlayer && !hasSpawned) //used to be isClient, have not yet check for errors
        {
			ClientReady(2);
			hasSpawned = true;
            CmdSendSpawned();

        }

        PlayerConnectedObject[] players = FindObjectsOfType<PlayerConnectedObject>();
        int ind = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].hasSpawned)
            {
                ind++;
            }
        }

        if (ind >= 4)
        {
            gameHasStarted = true;
        }
        else
        {
            ind = 0;
        }
        //if (isServer)
        //{
        //PotionCharacterController[] players = FindObjectsOfType<PotionCharacterController>();
        //if (players.Length >= 3) //do better spawning
        //{
        //	gameHasStarted = true;
        //}
        //}
    }

	void MGThreeSetup()
	{

		if (isLocalPlayer && !hasSpawned) //used to be isClient, have not yet check for errors
        {
			ClientReady(3);
			hasSpawned = true;
            CmdSendSpawned();

        }

        PlayerConnectedObject[] players = FindObjectsOfType<PlayerConnectedObject>();
        int ind = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].hasSpawned)
            {
                ind++;
            }
        }

        if (ind >= 4)
        {
            gameHasStarted = true;
        }
        else
        {
            ind = 0;
        }
        //if (isServer)
        ////{
        //	ChickenPlayerController[] players = FindObjectsOfType<ChickenPlayerController>();
        //	if (players.Length >= 3) //do better spawning
        //	{
        //		gameHasStarted = true;
        //	}
        //}
    }


    void VotingSceneSetup()
    {
        if (isLocalPlayer && !hasSpawned) //used to be isClient, have not yet check for errors
        {
            ClientReady(4);
            hasSpawned = true;
            CmdSendSpawned();

        }

        PlayerConnectedObject[] players = FindObjectsOfType<PlayerConnectedObject>();
        int ind = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].hasSpawned)
            {
                ind++;
            }
        }

        if (ind >= 4)
        {
            gameHasStarted = true;
        }
        else
        {
            ind = 0;
        }
        // if (isServer)
        ////{
        //    VoteInputer[] players = FindObjectsOfType<VoteInputer>();
        //    if (players.Length >= 3) //do better spawning
        //    {
        //        gameHasStarted = true;
        //    }
        //}
    }


	void WinnerSceneSetup()
	{
		if (isLocalPlayer && !hasSpawned) //used to be isClient, have not yet check for errors
		{
			ClientReady(5);
			hasSpawned = true;
			CmdSendSpawned();

		}

		PlayerConnectedObject[] players = FindObjectsOfType<PlayerConnectedObject>();
		int ind = 0;
		for (int i = 0; i < players.Length; i++)
		{
			if (players[i].hasSpawned)
			{
				ind++;
			}
		}

		if (ind >= 4)
		{
			gameHasStarted = true;
		}
		else
		{
			ind = 0;
		}

	}

    void GeneralGameSetup(int gNum)
    {
        if (isLocalPlayer && !hasSpawned) //used to be isClient, have not yet check for errors
        {
            ClientReady(gNum);
            hasSpawned = true;
            CmdSendSpawned();

        }

        PlayerConnectedObject[] players = FindObjectsOfType<PlayerConnectedObject>();
        int ind = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].hasSpawned)
            {
                ind++;
            }
        }

        if (ind >= 4)
        {
            gameHasStarted = true;
        }
        else
        {
            ind = 0;
        }

    }


    void ClientReady(int gameNum)
	{
		isReadyGame = true;
        CmdSendGameReady();
		if (isLocalPlayer)
		{
			CmdSpawnMyUnit(gameNum);
		}
	}

	[Command] //Spawning error could be coming from here. Needs to be delayed, or check whe nall spawned
	void CmdSpawnMyUnit(int charContNum)
	{
		//networkPlayerObjs = GameObject.FindGameObjectsWithTag("NetworkPlayerObject");
		GameObject go = Instantiate(minigameCharacterControllersObjs[charContNum], transform.position, transform.rotation);

		NetworkServer.SpawnWithClientAuthority(go, gameObject); 
	}


    [Command]
    void CmdSetPlayerNumber(int id) 
    {
        playerID = id;
        RpcSendOutMyNum(id);
    }

    [ClientRpc] 
    void RpcSendOutMyNum(int id) //this may be entirley unneccassary becoz it worked fine prior to this
    {
        playerID = id;
    }

    [ClientRpc]
    void RpcGameSetPlayerNum()
    {
        playerID = FindObjectOfType<IDSaver>().savedID;
        CmdSetPlayerNumber(playerID);
    }


    [Command]
    void CmdSendGameReady()
    {
        isReadyGame = true;
    }

    [ClientRpc]
    void RpcSendOutGameReady()
    {
        isReadyGame = true;

    }


    [Command]
    void CmdSendSpawned()
    {
        hasSpawned = true;
        RpcSendOutSpawned();
    }

    [ClientRpc]
    void RpcSendOutSpawned()
    {
        hasSpawned = true;

    }
}
