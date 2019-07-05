using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerConnectedObject : NetworkBehaviour
{
    //public static int playerTotal;
    public GameObject playerUnitPrefab;
    //GameObject myPlayerUnit;

    private GameObject boatObj;
    private GameObject[] networkPlayerObjs;
    private int netPlayerNum;
    private GameObject[] spArray;

    private GameObject minigameManagerObj;
    public GameObject[] minigameCharacterControllersObjs;
    private int contArrayOffset; //the total of non minigame scenes
								 // Start is called before the first frame update
	public bool isMe;
	public int playerID;

	public bool isReadyLobby;
	public bool isReadyGame;

	private bool gameHasStarted;

	private bool hasSpawned = false;

    void Start()
    {
		//Time.timeScale = 00.1f;



		
		playerID = FindObjectsOfType<PlayerConnectedObject>().Length; //ERROR: Makes multiple conobjs same id

		if (!isLocalPlayer)
		{
			return;
		}

		isMe = true;

		//print("asd");
		minigameManagerObj = GameObject.FindGameObjectWithTag("MinigameManager");
		//transform.position += Vector3.up;

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
		//minigame
		//else if(gameStart == false)
		//{ 
		//}
	}

	[Command]
	void CmdLobbyReady()
	{
		RpcLobbyReady();
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
                JoinLobbySetup();
                break;
            case 2:
                //minigame 1
                MGOneSetup();
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;

        }
    }

    void JoinLobbySetup()
    {
		print("RUNNIG LOBBY SHIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII");
        spArray = minigameManagerObj.GetComponent<LobbySceneManager>().playerSpawnPoints;
		//CmdSpawnMyUnit(0);
		//RpcSpawnMyUnit(0);

    }


    void MGOneSetup()
    {
		spArray = minigameManagerObj.GetComponent<BoatGameManager>().playerSpawnPoints;

		if (isClient && !hasSpawned)
		{
			CmdClientReady(1);
			hasSpawned = true;
		}
		//if client
		//set me as ready

		if (isServer) 
		{
			BoatController[] players = FindObjectsOfType<BoatController>();
			if (players.Length >= 3) //do better spawning
			{
				gameHasStarted = true;
			}
			//int readyInt = 0;
			//foreach (BoatGameManager player in players)
			//{
			//	if (players.Length >= 4)
			//	{
			//		gameHasStarted = true;
			//	}
			//}

			//if (readyInt == 4)
			//{
			//	print("RPC READY UP");
				

			//	//RpcSpawnMyUnit(1); //HERE
			//}
			//else
			//{
			//	print("ELSE readyint= " + readyInt );
			//	readyInt = 0;
			//}


		}
		//if host - update
		//find all players
		//see whos ready
		//if == total players
		//rpc - time =1, spawn players, for loop
        
		//CmdSpawnMyUnit(1);

		//RpcSpawnMyUnit(1);

	}

	[Command]
	void CmdClientReady(int gameNum)
	{
		isReadyGame = true;
		if (isLocalPlayer)
		{
			CmdSpawnMyUnit(gameNum);
		}
	}

	//[ClientRpc]
	//void RpcReadyUp(int gameNum, GameObject[] spawns)
	//{
	//	//print("MGOneSetup: " + spArray.Length + " name " + spArray[0].name);

	//	//Time.timeScale = 1;
	//	RpcSpawnMyUnit(gameNum, spawns);
	//	gameHasStarted = true;
	//}

	[Command]
	void CmdSpawnMyUnit(int charContNum)
	{
		//gameHasStarted = true;
		print("SPAWNMYUNITS");
		networkPlayerObjs = GameObject.FindGameObjectsWithTag("NetworkPlayerObject");
		netPlayerNum = playerID - 1;
		//print("CharCont " + charContNum + " | sp " + spawnPoints[netPlayerNum].gameObject.name);
		//GameObject go = Instantiate(minigameCharacterControllersObjs[charContNum], spawnPoints[netPlayerNum].transform.position, spawnPoints[netPlayerNum].transform.rotation);
		GameObject go = Instantiate(minigameCharacterControllersObjs[charContNum], transform.position, transform.rotation);

		NetworkServer.SpawnWithClientAuthority(go, gameObject);
		//CustomNetworkManager.Instantiate(minigameCharacterControllersObjs[charContNum], spArray[netPlayerNum].transform.position, spArray[netPlayerNum].transform.rotation);
		//NetworkServer.Spawn(go); 
	}

}
