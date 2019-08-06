using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class BoatController : NetworkBehaviour
{
    public int playerNum;
    private GameObject boatObj;
    private Rigidbody rbody;
    public int currentSpeed;


    private GameObject[] networkPlayerObjs;
    private GameObject[] spArray;


    public Animator anim;

	private GameObject sceneManager;

    public Material[] playerColours;

    public GameObject playerModelObj;

    private GameObject saboteurIdentifier;
    private GameObject innocentIdentifier;

    [SyncVar]
    private int sabPNum;

	public PlayerCustoms playerCustom;

    // Start is called before the first frame update
    void Start()
    {
            boatObj = GameObject.FindGameObjectWithTag("Boat");
            rbody = boatObj.GetComponent<Rigidbody>();

            sceneManager = GameObject.FindGameObjectWithTag("MinigameManager");
        if (hasAuthority || FindObjectsOfType<PlayerConnectedObject>().Length == 1 )
        {
            playerNum = FindObjectOfType<IDSaver>().savedID - 1;
            CmdCharacterSetup(playerNum);



            //SAB Stuff
            sabPNum = FindObjectOfType<IDSaver>().sabNum;
            saboteurIdentifier = GameObject.FindGameObjectWithTag("SaboteurIdentifier");
            innocentIdentifier = GameObject.FindGameObjectWithTag("InnocentIdentifier");
            if (playerNum == sabPNum)
            {
                //saboteurIdentifier.GetComponent<Image>().color = Color.red;
				saboteurIdentifier.GetComponent<Image>().enabled = true;

            }
            else
            {
                //saboteurIdentifier.GetComponent<Image>().color = Color.green;
				innocentIdentifier.GetComponent<Image>().enabled = true;

			}

			doTheLoad();
        }
	}

	//Load customisation options
	private void doTheLoad()
	{
		PlayerData data = SaveSystem.LoadPlayer();
		if (data == null)
		{
			data = new PlayerData();
		}
		CmdLoadCos(data.cust1Unlocked, data.cust2Unlocked, data.custom1Enabled, data.custom2Enabled);

	}
	//send cosmetic states to server
	[Command]
	void CmdLoadCos(bool b1, bool b2, bool equip1, bool equip2)
	{
		RpcLoadCos(b1, b2, equip1, equip2);
	}

	//tell clients customisation states
	[ClientRpc]
	void RpcLoadCos(bool b1, bool b2, bool equip1, bool equip2)
	{
		playerCustom.custom1Unlocked = b1;
		playerCustom.custom2Unlocked = b2;
		playerCustom.custom1Enabled = equip1;
		playerCustom.custom2Enabled = equip2;
	}

	//Tell server player id and set up colour and position
	[Command]
    void CmdCharacterSetup(int id) //this needs to be sent back to all clients (RPC)
    {
        playerNum = id;
        transform.SetParent(boatObj.GetComponent<BoatStats>().spawnPointArray[playerNum].transform);
        transform.localPosition = new Vector3(0, 0, 0);
        playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[playerNum];
        RpcBackToClientSetup(id);
    }

	//Tell clients player id and set up colour and position
	[ClientRpc]
    void RpcBackToClientSetup(int id)
    {
        playerNum = id;
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
    }

	//Inputes for movement
    void InputBoat(int pNum)
    {

        if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("P1LeftBumper"))
        {
            CmdMoveBoat(-1);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("P1RightBumper"))
        {
            CmdMoveBoat(1);
        }
    }

	//add force
    [Command]
    void CmdMoveBoat(int dir)
    {
        anim.SetTrigger("Row");
        rbody.AddForce(dir * 10, 0, 0);
		RpcSendAnimOut();
    }

	//Animate on clients
	[ClientRpc]
	void RpcSendAnimOut()
	{
		anim.SetTrigger("Row");

	}

}
