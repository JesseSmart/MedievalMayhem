using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;


public class BoatGameManager : NetworkBehaviour
{


    public float gameDuration;
    public float gameTimer;

    public int loadScene;
    public int nextGameInt;

    public GameObject[] playerSpawnPoints;


    public TextMeshProUGUI uiTimer;

    public Image identifierImageObject;
    public Sprite innocentIdentifierImage;
    public Sprite sabotagerIdentifierImage;

    private GameObject boatObj;

    private GameObject networkManagerObj;

    // Start is called before the first frame update
    void Start()
    {
        networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
        if (isServer)
        {
            PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed") + 1);

        }

        gameTimer = gameDuration;
    }

    // Update is called once per frame
    void Update()
    {
        uiTimer.text = gameTimer.ToString("00:00");


        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0)
        {
            EndGame();
        }

    }

    private void EndGame()
    {
        //networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(PlayerPrefs.GetString("LevelName" + PlayerPrefs.GetInt("LevelLoad" + PlayerPrefs.GetInt("GamesPlayed"))));
        if (isServer)
        {
            if (PlayerPrefs.GetInt("GamesPlayed") < PlayerPrefs.GetInt("MaxGames")) //might be <=
            {
                networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene("Voting Scene");

            }
            else
            {
                //all games played, end game scene or whatever
                networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene("Menu");
            }

        }

    }

    //private void OnlineSetter()
    //{
    //    identifierImageObject.enabled = false;
    //}

    //private void OfflineSetter()
    //{
    //    identifierImageObject.enabled = true;



    //}
}
