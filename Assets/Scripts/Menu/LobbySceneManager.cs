using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class LobbySceneManager : NetworkBehaviour
{

    public GameObject[] playerSpawnPoints;
    private GameObject networkManagerObj;
    public GameObject[] readyTextArray;

    public string[] minigameSceneNames;

    private int totalReadys = 0;

    private int[] levelLoadOrder = new int[3] { 0,1,2}; //make sure is length of possible levels loadable. Could maybe make void 
														// Start is called before the first frame update

	GameObject[] spawnedPlayers;

	public Color[] playerColors = new Color[4] { Color.red, Color.blue, Color.green, Color.yellow} ;

	//private float lobbyDelay = 5;
	//private float lobbyTimer;
	int lastPlayers = 0;

    [SyncVar]
    private string loadSceneString;

    void Start()
    {
		//lobbyTimer = lobbyDelay;
		networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
        PlayerPrefs.DeleteAll(); //WARNING< MAKE SURE IT DOES EFFECT MENU PREFABS IF USED IN FUTURE. Might move to menu play press
        if (isServer)
        {

            RandomizeArray(levelLoadOrder);
            SetLevelNames();
            SetLevelOrder();

            int rndInd = Random.Range(0, 3);
            //Might need to be sent to rpc so all clients know
            FindObjectOfType<IDSaver>().sabNum = rndInd;

            PlayerPrefs.SetInt("SabPlayerNumber", rndInd);
            PlayerPrefs.SetInt("GamesPlayed", 0);
            PlayerPrefs.SetInt("MaxGames", minigameSceneNames.Length);

            RpcTellClientsPrefs(rndInd);
        }	

		spawnedPlayers = new GameObject[4];
    }

    // Update is called once per frame
    void Update()
    {
        
		PlayerConnectedObject[] players = FindObjectsOfType<PlayerConnectedObject>();

		int tempInt = 0;
		foreach (PlayerConnectedObject p in players)
		{
			if (p.isReadyLobby)
			{
                //readyTextArray[p.playerID - 1].GetComponent<TextMeshProUGUI>().enabled = true;
                if (isServer)
                {
				    RpcReadyUp(p.playerID);

                }
				tempInt++;
			}

		}
		if (tempInt == 4)
		{
            if (isServer)
            {
                //loadSceneString = PlayerPrefs.GetString(PlayerPrefs.GetString("LevelName" + PlayerPrefs.GetInt("LevelLoad" + PlayerPrefs.GetInt("GamesPlayed"))));
                //networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(loadSceneString);

                networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(PlayerPrefs.GetString("LevelName" + PlayerPrefs.GetInt("LevelLoad" + PlayerPrefs.GetInt("GamesPlayed"))));
                //CmdPlayersHaveReadyUp();

            }
		}
		else
		{
			tempInt = 0;
		}

		if (players.Length != lastPlayers)
		{
			foreach (GameObject obj in spawnedPlayers)
			{
				if (obj != null)
				{
					Destroy(obj);
				}
			}

			for (int x = 0; x < players.Length; ++x)
			{
					spawnedPlayers[x] = (GameObject)Instantiate(Resources.Load("LobbyPlayerVisual"), Vector3.zero + (Vector3.left * 2) + (Vector3.right * x), Quaternion.identity);

					spawnedPlayers[x].GetComponentInChildren<SkinnedMeshRenderer>().material.color = playerColors[x];

			}

		}

		lastPlayers = players.Length;
	}

	[Command]
    public void CmdPlayersHaveReadyUp()
    {
		//networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(minigameSceneNames[0]);
		//*** Maybe this load conobj error
        networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(PlayerPrefs.GetString("LevelName" + PlayerPrefs.GetInt("LevelLoad" + PlayerPrefs.GetInt("GamesPlayed"))));

    }

	[ClientRpc]
	void RpcReadyUp(int id)
	{
		readyTextArray[id - 1].GetComponent<TextMeshProUGUI>().enabled = true;
	}
    

    void RandomizeArray(int[] array) //randomises load order array
    {
        for (int i = array.Length - 1; i > 1; i--)
        {
            int rnd = Random.Range(0, i);
            int temp = array[i];
            array[i] = array[rnd];
            array[rnd] = temp;
        }
    }

    void SetLevelNames() //converts list of level names into playerpref
    {
        for (int i = 0; i < minigameSceneNames.Length; i++)
        {
            PlayerPrefs.SetString("LevelName" + i, minigameSceneNames[i]);
        }
    }

    void SetLevelOrder() // sets randomised level load sequence to playpref
    {
        for (int i = 0; i < levelLoadOrder.Length; i++)
        {
            PlayerPrefs.SetInt("LevelLoad" + i, levelLoadOrder[i]);
        }
    }


    [ClientRpc]
    void RpcTellClientsPrefs(int rnd)
    {
        SetPrefs(rnd);
    }

    void SetPrefs(int rnd)
    {
        FindObjectOfType<IDSaver>().sabNum = rnd;
        PlayerPrefs.SetInt("SabPlayerNumber", rnd);
        PlayerPrefs.SetInt("GamesPlayed", 0);
        PlayerPrefs.SetInt("MaxGames", minigameSceneNames.Length);
    }
}
