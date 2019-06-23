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
    //debug 
    private float timr = 8;

    private int totalReadys = 0;
    // Start is called before the first frame update
    void Start()
    {
        networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
    }

    // Update is called once per frame
    void Update()
    {
        //timr -= Time.deltaTime;
        //if (timr <= 0)
        //{
        //    LoadNextGame();
        //    timr = 10;
        //}
    }

	[Command]
    public void CmdPlayersHaveReadyUp()
    {
		networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(minigameSceneNames[0]);
		//totalReadys += 1;

  //      if (totalReadys >= 2) //make 4 later
  //      {
  //          LoadNextGame();
  //      }
    }

    public void LoadNextGame() //or maybe voting scene
    {
        
        //networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(minigameSceneNames[0]);
        
        
        
        //if host
        //networkManagerObj.GetComponent<CustomNetworkManager>().onlineScene = minigameScenes[0].ToString();

        //if (isServer)
        //{
        //    networkManagerObj.GetComponent<CustomNetworkManager>().StartUpHost();

        //}
        //else if (isLocalPlayer)        
        //{
        //    networkManagerObj.GetComponent<CustomNetworkManager>().JoinGame();

        //}


        //else


    }
}
