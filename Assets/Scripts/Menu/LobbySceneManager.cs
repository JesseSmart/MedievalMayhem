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

    private IDSaver saver;
    private bool hasDoneLoad;
    [SyncVar]
    private string loadSceneString;
    private float loadDelay = 1;
    private bool startTimerLoad;

    void Start()
    {
		//lobbyTimer = lobbyDelay;
		networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
        PlayerPrefs.DeleteAll(); //WARNING< MAKE SURE IT DOES EFFECT MENU PREFABS IF USED IN FUTURE. Might move to menu play press
        if (isServer)
        {
            saver = FindObjectOfType<IDSaver>();
            saver.levelNames = minigameSceneNames;
            RandomizeArray(levelLoadOrder);
            SetLevelNames();
            SetLevelOrder();

            //int rndSab = Random.Range(0, 3);
            //saver.sabNum = rndSab;
            //RpcTellClientsPrefs(rndSab, saver.levelLoadArray, saver.levelNames, minigameSceneNames.Length); //maybe make all things set in this saver first then send througgh
        }	

		spawnedPlayers = new GameObject[4];
    }

    // Update is called once per frame
    void Update()
    {
        
		PlayerConnectedObject[] players = FindObjectsOfType<PlayerConnectedObject>();
        if (isServer)
        {
		    int tempInt = 0;
		    foreach (PlayerConnectedObject p in players)
		    {
			    if (p.isReadyLobby)
			    {
                    //readyTextArray[p.playerID - 1].GetComponent<TextMeshProUGUI>().enabled = true;
                    //if (isServer)
                    //{
				        CmdSendReadyUp(p.playerID);

                    //}
				    tempInt++;
			    }

		    }
		    if (tempInt == 4)
		    {
                if (!hasDoneLoad)
                {
                    loadSceneString = saver.levelNames[saver.levelLoadArray[saver.gamesPlayed]];
                    RpcTellSceneName(loadSceneString);
                    hasDoneLoad = true;

                    int rndSab = Random.Range(0, 3);
                    saver.sabNum = rndSab;
                    RpcTellClientsPrefs(rndSab, saver.levelLoadArray, saver.levelNames, minigameSceneNames.Length);
                    //Invoke("DoLoad", 1f);
                    startTimerLoad = true;

                }
                else if (startTimerLoad == true)
                {
                    loadDelay -= Time.deltaTime;
                    if (loadDelay <= 0)
                    {
                        networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(loadSceneString);
						loadDelay = 10;
                    }
                }



            }
		    else
		    {
                tempInt = 0;
		    }

        }

        void DoLoad()
        {
            networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(loadSceneString);

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
    void CmdSendReadyUp(int id)
    {
        readyTextArray[id - 1].GetComponent<TextMeshProUGUI>().enabled = true;
        RpcReadyUp(id);
    }

    [ClientRpc]
	void RpcReadyUp(int id)
	{
		readyTextArray[id - 1].GetComponent<TextMeshProUGUI>().enabled = true;
	}

    //[Command]
    //void CmdChangeScene()
    //{
    //    loadSceneString = PlayerPrefs.GetString("LevelName" + PlayerPrefs.GetInt("LevelLoad" + PlayerPrefs.GetInt("GamesPlayed")));
    //    print(loadSceneString);
    //    networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(loadSceneString);
    //}


    void RandomizeArray(int[] array) //randomises load order array
    {
        for (int i = array.Length - 1; i > 1; i--)
        {
            int rnd = Random.Range(0, i);
            int temp = array[i];
            array[i] = array[rnd];
            array[rnd] = temp;
        }
        levelLoadOrder = array;
        saver.levelLoadArray = array;
    }

    void SetLevelNames() //converts list of level names into playerpref
    {
        for (int i = 0; i < minigameSceneNames.Length; i++)
        {
            //PlayerPrefs.SetString("LevelName" + i, minigameSceneNames[i]);
            saver.levelNames[i] = minigameSceneNames[i];
        }
    }

    void SetLevelOrder() // sets randomised level load sequence to playpref
    {
        for (int i = 0; i < levelLoadOrder.Length; i++)
        {
            //PlayerPrefs.SetInt("LevelLoad" + i, levelLoadOrder[i]);
            saver.levelLoadArray[i] = levelLoadOrder[i];
        }
    }


    [ClientRpc]
    void RpcTellClientsPrefs(int sab, int[] loadOrd, string[] lvlNames, int maxGam) 
    {
        //SetPrefs(rnd);
        IDSaver mySaver = FindObjectOfType<IDSaver>();
        mySaver.sabNum = sab;
        mySaver.levelLoadArray = loadOrd;
        mySaver.levelNames = lvlNames;
        mySaver.maxGames = maxGam;



        //PlayerPrefs.SetInt("SabPlayerNumber", sab);
        //PlayerPrefs.SetInt("GamesPlayed", 0);
        //PlayerPrefs.SetInt("MaxGames", minigameSceneNames.Length);
    }

    void SetPrefs(int sab, int[] loadOrd, string[] lvlNames, int maxGam)
    {
 
    }


    [ClientRpc]
    void RpcDebugText(string s)
    {
        print(s);
    }

    [ClientRpc]
    void RpcTellSceneName(string name)
    {
        loadSceneString = name;

    }
}
