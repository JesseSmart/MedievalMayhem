using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VoteManager : MonoBehaviour
{

    public TextMeshProUGUI[] voteText;
    public int[] voteTotalArray = new int[4] { 0,0,0,0};
    private int totalInputs;

    public Image[] sabotagerIndicatorArray;

    private float tempTimer = 5; //debug
    private GameObject networkManagerObj;

    // Start is called before the first frame update
    void Start()
    {
        networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");


    }

    // Update is called once per frame
    void Update()
    {
        //if (totalInputs >= 4)
        //{
        //    VotingComplete();
        //}

        tempTimer -= Time.deltaTime;
        if (tempTimer <= 0)
        {
            LoadNextGame();
        }

    }

    //public void VoteAdd(int suspectedPlayer)
    //{
    //    voteTotalArray[suspectedPlayer] += 1;
    //    voteText[suspectedPlayer].text = voteTotalArray[suspectedPlayer].ToString();
    //    totalInputs += 1;
    //}

    //private void VotingComplete()
    //{
    //    print("VOTING COMPLETE");
    //    sabotagerIndicatorArray[Random.Range(0, 3)].enabled = true;
    //}
    void LoadNextGame()
    {
        PlayerPrefs.SetInt("SabPlayer", Random.Range(0, 3));
        networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene(PlayerPrefs.GetString("LevelName" + PlayerPrefs.GetInt("LevelLoad" + PlayerPrefs.GetInt("GamesPlayed"))));

    }


}
