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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameScore >= targetScore)
        {
            //win game
        }

        tmpCurrentScore.text = gameScore.ToString();
        tmpTargetScore.text = targetScore.ToString();
    }

    public void GainPoint()
    {
        gameScore += 1;
    }
}
