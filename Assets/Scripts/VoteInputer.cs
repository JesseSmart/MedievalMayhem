using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteInputer : MonoBehaviour
{
    public int playerNumber;
    private bool[] hasInputted = new bool[4];
    public GameObject parentObject;

    private string[] aButtonArray = new string[4] { "P1AButton", "P2AButton", "P3AButton", "P4AButton" };
    private string[] bButtonArray = new string[4] { "P1BButton", "P2BButton", "P3BButton", "P4BButton" };
    private string[] xButtonArray = new string[4] { "P1XButton", "P2XButton", "P3XButton", "P4XButton" };
    private string[] yButtonArray = new string[4] { "P1YButton", "P2YButton", "P3YButton", "P4YButton" };



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InputManage(playerNumber);

    }

    void InputManage(int pNum)
    {
        #region //PC Controls
        if (Input.GetKeyDown(KeyCode.Alpha1) && pNum == 0 && hasInputted[pNum] == false)
        {
            print("Player " + pNum + ": Pressed 1");
            parentObject.GetComponent<VoteManager>().VoteAdd(0);
            hasInputted[pNum] = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && pNum == 0 && hasInputted[pNum] == false)
        {
            print("Player " + pNum + ": Pressed 2");
            parentObject.GetComponent<VoteManager>().VoteAdd(1);
            hasInputted[pNum] = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && pNum == 0 && hasInputted[pNum] == false)
        {
            print("Player " + pNum + ": Pressed 3");
            parentObject.GetComponent<VoteManager>().VoteAdd(2);
            hasInputted[pNum] = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && pNum == 0 && hasInputted[pNum] == false)
        {
            print("Player " + pNum + ": Pressed 4");
            parentObject.GetComponent<VoteManager>().VoteAdd(3);
            hasInputted[pNum] = true;
        }
        #endregion



        if (Input.GetButtonDown(aButtonArray[pNum]) && hasInputted[pNum] == false)
        {
            print("Player " + pNum + ": Pressed A");
            parentObject.GetComponent<VoteManager>().VoteAdd(0);
            hasInputted[pNum] = true;

        }

        if (Input.GetButtonDown(xButtonArray[pNum]) && hasInputted[pNum] == false)
        {
            print("Player " + pNum + ": Pressed X");
            parentObject.GetComponent<VoteManager>().VoteAdd(1);
            hasInputted[pNum] = true;


        }

        if (Input.GetButtonDown(yButtonArray[pNum]) && hasInputted[pNum] == false)
        {
            print("Player " + pNum + ": Pressed Y");
            parentObject.GetComponent<VoteManager>().VoteAdd(2);
            hasInputted[pNum] = true;


        }

        if (Input.GetButtonDown(bButtonArray[pNum]) && hasInputted[pNum] == false)
        {
            print("Player " + pNum + ": Pressed B");
            parentObject.GetComponent<VoteManager>().VoteAdd(3);
            hasInputted[pNum] = true;


        }
    }


}
