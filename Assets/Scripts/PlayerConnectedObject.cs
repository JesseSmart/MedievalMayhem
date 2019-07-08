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
        //playerID = FindObjectsOfType<PlayerConnectedObject>().Length; //ERROR: Makes multiple conobjs same id

        //pConObjs = FindObjectsOfType<PlayerConnectedObject>();



        if (SceneManager.GetActiveScene().buildIndex == 1)
        {

            if (isLocalPlayer)
            {
                playerID = FindObjectsOfType<PlayerConnectedObject>().Length; 
                PlayerPrefs.SetInt("OrigID", playerID);
                PlayerPrefs.SetInt("PlayerNum" + PlayerPrefs.GetInt("OrigID"), playerID);

                
                CmdSetPlayerNumber();
                PlayerPrefs.SetInt("LocalPlayerNum", playerID);

            }
        }
        else
        {
            if (isLocalPlayer)
            {

                CmdGameSetPlayerNum();
            }
        }
        print("My Net ID: " + PlayerPrefs.GetInt("OrigID") + " | My PlPr: " + PlayerPrefs.GetInt("PlayerNum" + PlayerPrefs.GetInt("OrigID")));


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
                break;
            case 4:
				MGTwoSetup();
                break;
            case 5:
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

		if (isClient && !hasSpawned)
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

		if (isClient && !hasSpawned)
		{
			ClientReady(2);
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

	void ClientReady(int gameNum)
	{
		isReadyGame = true;
		if (isLocalPlayer)
		{
			CmdSpawnMyUnit(gameNum);
		}
	}

	[Command]
	void CmdSpawnMyUnit(int charContNum)
	{
		networkPlayerObjs = GameObject.FindGameObjectsWithTag("NetworkPlayerObject");
		netPlayerNum = playerID - 1;
		GameObject go = Instantiate(minigameCharacterControllersObjs[charContNum], transform.position, transform.rotation);

        //maybe send netword id with a go.getcomp().myNetID = netID

		NetworkServer.SpawnWithClientAuthority(go, gameObject); 
	}


    [Command]
    void CmdSetPlayerNumber() //ISSUE IS SOMEWHERE HERE
    {
        playerID = FindObjectsOfType<PlayerConnectedObject>().Length;
//        PlayerPrefs.SetInt("PlayerNum"+ PlayerPrefs.GetInt("OrigID"), playerID);

        //maybe Rpc for set num aswell so it sets on clients
    }

    [Command]
    void CmdGameSetPlayerNum()
    {
        playerID = PlayerPrefs.GetInt("PlayerNum" + PlayerPrefs.GetInt("OrigID"));
        //playerID = PlayerPrefs.GetInt("OrigID");
    }

    //Clients are all player 4 for some reason
}
