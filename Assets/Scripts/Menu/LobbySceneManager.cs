using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class LobbySceneManager : NetworkBehaviour
{

    public GameObject[] playerSpawnPoints;
    private GameObject networkManagerObj;
    public GameObject[] readyTextArray;

    public string[] minigameSceneNames;

    private int totalReadys = 0;

    private int[] levelLoadOrder = new int[3] { 0,1,2}; //make sure is length of possible levels loadable. Could maybe make void 
    // Start is called before the first frame update
    void Start()
    {
        networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
        if (isServer)
        {
            PlayerPrefs.DeleteAll(); //WARNING< MAKE SURE IT DOES EFFECT MENU PREFABS IF USED IN FUTURE. Might move to menu play press
            RandomizeArray(levelLoadOrder);
            SetLevelNames();
            SetLevelOrder();
            PlayerPrefs.SetInt("SabPlayerNumber", Random.Range(0, 3));

        }
        PlayerPrefs.SetInt("GamesPlayed", 0);
        PlayerPrefs.SetInt("MaxGames", minigameSceneNames.Length);
    }

    // Update is called once per frame
    void Update()
    {

    }

	[Command]
    public void CmdPlayersHaveReadyUp()
    {
		//networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(minigameSceneNames[0]);
        networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(PlayerPrefs.GetString("LevelName" + PlayerPrefs.GetInt("LevelLoad" + PlayerPrefs.GetInt("GamesPlayed"))));

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
}
