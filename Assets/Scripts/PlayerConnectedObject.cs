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

	public int playerID;

	public bool isReadyLobby;
	public bool isReadyGame;

	private bool gameHasStarted;

	private bool hasSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        //playerID = FindObjectsOfType<PlayerConnectedObject>().Length; //ERROR: Makes multiple conobjs same id

        PlayerConnectedObject[] pConObjs = FindObjectsOfType<PlayerConnectedObject>();
        //if lobby
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //if (isLocalPlayer)
            //{


            //    for (int i = 0; i < pConObjs.Length; i++)
            //    {
            //        pConObjs[i].playerID = i + 1;
            //        PlayerPrefs.SetInt("LocalPlayerNum" + (i + 1), playerID);

            //    }
            //}


            CmdSetPlayerNumber(); //might not be needed


        }
        else
        {


            if (isLocalPlayer)
            {

                playerID = PlayerPrefs.GetInt("LocalPlayerNum");

                //for (int i = 0; i < pConObjs.Length; i++)
                //{
                //    playerID = PlayerPrefs.GetInt("LocalPlayerNum" + (i + 1));


                //}

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

		NetworkServer.SpawnWithClientAuthority(go, gameObject); 
	}


    [Command]
    void CmdSetPlayerNumber()
    {
        playerID = NetworkServer.connections.Count;
        if (isLocalPlayer)
        {
            PlayerPrefs.SetInt("LocalPlayerNum", playerID);

        }
        //maybe Rpc for set num aswell so it sets on clients
    }
}
