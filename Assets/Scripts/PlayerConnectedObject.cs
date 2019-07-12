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
    private GameObject[] networkPlayerObjs;
    private int netPlayerNum;

    private GameObject minigameManagerObj;
    public GameObject[] minigameCharacterControllersObjs;
    private int contArrayOffset; //the total of non minigame scenes

	public bool isMe;

    [SyncVar]
	public int playerID;

	public bool isReadyLobby;
	public bool isReadyGame;

	private bool gameHasStarted;

	private bool hasSpawned = false;

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
                PlayerPrefs.SetInt("LocalPlayerNum", playerID);
            }
        }
        else
        {
            if (isServer)
            {
                RpcGameSetPlayerNum();
            }
        }



        if (!isLocalPlayer)
		{
			return;
		}


		isMe = true;

		minigameManagerObj = GameObject.FindGameObjectWithTag("MinigameManager");

        CheckMinigame();

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
			if (Input.GetKeyDown(KeyCode.Space))
			{


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
                MGOneSetup();
                break;
            case 3:
				MGTwoSetup();
                break;
            case 4:
				MGThreeSetup();
                break;
            case 5:
                VotingSceneSetup();
                break;
            case 6:
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
		}


		if (isServer) 
		{
			BoatController[] players = FindObjectsOfType<BoatController>();
			if (players.Length >= 3) //do better spawning
			{
				gameHasStarted = true;
			}
		}
	}

	void MGTwoSetup()
	{
		if (isLocalPlayer && !hasSpawned) //used to be isClient, have not yet check for errors
        {
			ClientReady(2);
			hasSpawned = true;
		}


		if (isServer)
		{
			PotionCharacterController[] players = FindObjectsOfType<PotionCharacterController>();
			if (players.Length >= 3) //do better spawning
			{
				gameHasStarted = true;
			}
		}
	}

	void MGThreeSetup()
	{

		if (isLocalPlayer && !hasSpawned) //used to be isClient, have not yet check for errors
        {
			ClientReady(3);
			hasSpawned = true;
		}


		if (isServer)
		{
			ChickenPlayerController[] players = FindObjectsOfType<ChickenPlayerController>();
			if (players.Length >= 3) //do better spawning
			{
				gameHasStarted = true;
			}
		}
	}


    void VotingSceneSetup()
    {
        if (isLocalPlayer && !hasSpawned) //used to be isClient, have not yet check for errors
        {
            ClientReady(4);
            hasSpawned = true;
        }


        if (isServer)
        {
            VoteInputer[] players = FindObjectsOfType<VoteInputer>();
            if (players.Length >= 3) //do better spawning
            {
                gameHasStarted = true;
            }
        }
    }



	void ClientReady(int gameNum)
	{
		isReadyGame = true;
		if (isLocalPlayer)
		{
			CmdSpawnMyUnit(gameNum);
		}
	}

	[Command] //Spawning error could be coming from here. Needs to be delayed, or check whe nall spawned
	void CmdSpawnMyUnit(int charContNum)
	{
		networkPlayerObjs = GameObject.FindGameObjectsWithTag("NetworkPlayerObject");
		netPlayerNum = playerID - 1;
		GameObject go = Instantiate(minigameCharacterControllersObjs[charContNum], transform.position, transform.rotation);

		NetworkServer.SpawnWithClientAuthority(go, gameObject); 
	}


    [Command]
    void CmdSetPlayerNumber(int id) 
    {
        playerID = id;
    }

    [ClientRpc]
    void RpcGameSetPlayerNum()
    {
        playerID = FindObjectOfType<IDSaver>().savedID;
        CmdSetPlayerNumber(playerID);
    }

}
