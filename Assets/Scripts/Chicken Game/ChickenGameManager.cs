using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChickenGameManager : MonoBehaviour
{

    private int gameScore;
    public int targetScore;

    public TextMeshProUGUI tmpCurrentScore;
    public TextMeshProUGUI tmpTargetScore;
    public TextMeshProUGUI tmpTimerText;

    public float matchDuration;
    private float matchTimer;

    private GameObject networkManagerObj;

    // Start is called before the first frame update
    void Start()
    {
        matchTimer = matchDuration;

        networkManagerObj = GameObject.FindGameObjectWithTag("NetworkManager");
        PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed") + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameScore >= targetScore)
        {
            //win game
            GameEnd();
        }

        matchTimer -= Time.deltaTime;
        if (matchTimer <= 0)
        {
            //game lost
            GameEnd();
        }
        else
        {
            tmpTimerText.text = matchTimer.ToString("00:00");
        }

        tmpCurrentScore.text = gameScore.ToString();
        tmpTargetScore.text = targetScore.ToString();
    }

    public void GainPoint()
    {
        gameScore += 1;
    }

    public void GameEnd()
    {
        networkManagerObj.GetComponent<CustomNetworkManager>().LoadGameScene("Voting Scene");

    }
}
