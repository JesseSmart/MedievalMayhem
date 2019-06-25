using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PotionGameManager : MonoBehaviour
{

    public GameObject cauldronObj;

    public Slider potionSlider;

    private GameObject networkManagerObj;

    public float gameDuration;
    private float gameTimer;
    // Start is called before the first frame update
    void Start()
    {
        networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
        PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed") + 1);

        gameTimer = gameDuration;

    }

    // Update is called once per frame
    void Update()
    {
        potionSlider.value = cauldronObj.GetComponent<CauldronTrigger>().colorFloat;


        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0)
        {
            //win
            GameEnd();
        }
    }

    public void GameEnd()
    {
        networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene("Voting Scene");

    }
}
