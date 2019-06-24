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
    void Start()
    {
		//if (!isLocalPlayer)
		//{
		//	return;
		//}
        minigameManagerObj = GameObject.FindGameObjectWithTag("MinigameManager");

        CheckMinigame();

    }

    // Update is called once per frame
    void Update()
    {

    }

    [ClientRpc]
    void RpcSpawnMyUnit(int charContNum)
    {

        networkPlayerObjs = GameObject.FindGameObjectsWithTag("NetworkPlayerObject");
        netPlayerNum = networkPlayerObjs.Length - 1; //player 1 = int 0, just so you know
        GameObject go = Instantiate(minigameCharacterControllersObjs[charContNum], spArray[netPlayerNum].transform.position, spArray[netPlayerNum].transform.rotation);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        
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
        RpcSpawnMyUnit(0);

    }


    void MGOneSetup()
    {

        spArray = minigameManagerObj.GetComponent<BoatGameManager>().playerSpawnPoints;
        RpcSpawnMyUnit(1);

    }


}
