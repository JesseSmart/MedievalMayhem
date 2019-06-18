using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class LobbySceneManager : NetworkBehaviour
{

    public GameObject[] playerSpawnPoints;
    private GameObject networkManagerObj;

    public Scene[] minigameScenes;


    // Start is called before the first frame update
    void Start()
    {
        networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextGame() //or maybe voting scene
    {

        //if host
        networkManagerObj.GetComponent<CustomNetworkManager>().onlineScene = minigameScenes[0].ToString();

        if (isServer)
        {
            networkManagerObj.GetComponent<CustomNetworkManager>().StartUpHost();

        }
        else if (isLocalPlayer)        
        {
            networkManagerObj.GetComponent<CustomNetworkManager>().JoinGame();

        }


        //else


    }
}
