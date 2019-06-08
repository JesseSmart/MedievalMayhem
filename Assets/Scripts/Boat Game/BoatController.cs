using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class BoatController : NetworkBehaviour
{
    public int playerNum;
    private GameObject boatObj;
    private Rigidbody rbody;
    public int currentSpeed;


    private GameObject[] networkPlayerObjs;
    private int netPlayerNum;
    private GameObject[] spArray;
    //public GameObject testIndicatorObj;

    private string[] horizontalArray = new string[4] { "P1Horizontal", "P2Horizontal", "P3Horizontal", "P4Horizontal" };
    private string[] verticalArray = new string[4] { "P1Vertical", "P2Vertical", "P3Vertical", "P4Vertical" };
    private string[] leftBumperArray = new string[4] { "P1LeftBumper", "P2LeftBumper", "P3LeftBumper", "P4LeftBumper" };
    private string[] rightBumperArray = new string[4] { "P1RightBumper", "P2RightBumper", "P3RightBumper", "P4RightBumper" };

    public Animator anim;
  
    // Start is called before the first frame update
    void Start()
    {
        boatObj = GameObject.FindGameObjectWithTag("Boat");
        rbody = boatObj.GetComponent<Rigidbody>();

        //spArray = boatObj.GetComponent<BoatStats>().spawnPointArray;
        //networkPlayerObjs = GameObject.FindGameObjectsWithTag("NetworkPlayerObject");
        //netPlayerNum = networkPlayerObjs.Length; //player 1 = int 0, just so you know
        //gameObject.transform.parent = spArray[netPlayerNum].transform;
        //gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            InputBoat(playerNum);
        }
        else
        {
            print("aint local");
        }
    }

    void InputBoat(int pNum)
    {
        if (Input.GetButtonDown(leftBumperArray[pNum]))
        {
            print("Player " + pNum + ": Pressed Left Bumper");

            //testIndicatorObj.GetComponent<MeshRenderer>().enabled = true;
            //StartCoroutine(WaitSec(0.5f));

            CmdMoveBoat(-1);
        }

        if (Input.GetButtonDown(rightBumperArray[pNum]))
        {
            print("Player " + pNum + ": Pressed Left Bumper");

            //testIndicatorObj.GetComponent<MeshRenderer>().enabled = true;
            //StartCoroutine(WaitSec(0.5f));

            CmdMoveBoat(1);
        }



        if (Input.GetKeyDown(KeyCode.A) && playerNum == 0)
        {

            print("Player " + pNum + ": Pressed A");

            //testIndicatorObj.GetComponent<MeshRenderer>().enabled = true;
            //StartCoroutine(WaitSec(0.5f));

            CmdMoveBoat(-1);
        }

        if (Input.GetKeyDown(KeyCode.D) && playerNum == 0)
        {

            print("Player " + pNum + ": Pressed A");

            //testIndicatorObj.GetComponent<MeshRenderer>().enabled = true;
            //StartCoroutine(WaitSec(0.5f));

            CmdMoveBoat(1);
        }
    }

    [Command]
    void CmdMoveBoat(int dir)
    {
        anim.SetTrigger("Row");
        rbody.AddForce(dir * 10, 0, 0);
    }



    IEnumerator WaitSec(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //testIndicatorObj.GetComponent<MeshRenderer>().enabled = false;

    }


}
