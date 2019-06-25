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

	private GameObject[] playerObjs;
	private GameObject sceneManager;

    public Material[] playerColours;

    public GameObject playerModelObj;


    // Start is called before the first frame update
    void Start()
    {
        boatObj = GameObject.FindGameObjectWithTag("Boat");
        rbody = boatObj.GetComponent<Rigidbody>();

		sceneManager = GameObject.FindGameObjectWithTag("MinigameManager");
		playerObjs = GameObject.FindGameObjectsWithTag("Player");

		playerNum = playerObjs.Length - 1;

		transform.SetParent(boatObj.GetComponent<BoatStats>().spawnPointArray[playerNum].transform);
		transform.localPosition = new Vector3(0, 0, 0);


		playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[playerNum];


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
            CmdMoveBoat(-1);
        }

        if (Input.GetButtonDown(rightBumperArray[pNum]))
        {
            CmdMoveBoat(1);
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            CmdMoveBoat(-1);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
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
