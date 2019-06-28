using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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

    void Start()
    {
		if (!isLocalPlayer)
		{
			return;
		}

		isMe = true;

		print("asd");
		minigameManagerObj = GameObject.FindGameObjectWithTag("MinigameManager");
		transform.position += Vector3.up;

        CheckMinigame();

    }

    // Update is called once per frame
    void Update()
    {

    }

    [Command]
    void CmdSpawnMyUnit(int charContNum)
    {

        networkPlayerObjs = GameObject.FindGameObjectsWithTag("NetworkPlayerObject");
        netPlayerNum = networkPlayerObjs.Length - 1; //player 1 = int 0, just so you know
        GameObject go = Instantiate(minigameCharacterControllersObjs[charContNum], spArray[netPlayerNum].transform.position, spArray[netPlayerNum].transform.rotation);

		NetworkServer.SpawnWithClientAuthority(go, gameObject);
		//CustomNetworkManager.Instantiate(minigameCharacterControllersObjs[charContNum], spArray[netPlayerNum].transform.position, spArray[netPlayerNum].transform.rotation);
		//NetworkServer.Spawn(go); 
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

        spArray = minigameManagerObj.GetComponent<LobbySceneManager>().playerSpawnPoints;
		//CmdSpawnMyUnit(0);
		//RpcSpawnMyUnit(0);

    }


    void MGOneSetup()
    {
		//if client
		//set me as ready

		//if host - update
		//find all players
		//see whos ready
		//if == total players
		//rpc - time =1, spawn players, for loop
        spArray = minigameManagerObj.GetComponent<BoatGameManager>().playerSpawnPoints;
		//CmdSpawnMyUnit(1);

		//RpcSpawnMyUnit(1);

	}


}
