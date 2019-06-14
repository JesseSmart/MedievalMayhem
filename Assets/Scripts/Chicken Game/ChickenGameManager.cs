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
    // Start is called before the first frame update
    void Start()
    {
        matchTimer = matchDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameScore >= targetScore)
        {
            //win game
        }

        matchTimer -= Time.deltaTime;
        if (matchTimer <= 0)
        {
            //game lost
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
}
