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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (totalInputs >= 4)
        {
            VotingComplete();
        }
    }

    public void VoteAdd(int suspectedPlayer)
    {
        voteTotalArray[suspectedPlayer] += 1;
        voteText[suspectedPlayer].text = voteTotalArray[suspectedPlayer].ToString();
        totalInputs += 1;
    }

    private void VotingComplete()
    {
        print("VOTING COMPLETE");
        sabotagerIndicatorArray[Random.Range(0, 3)].enabled = true;
    }


}
