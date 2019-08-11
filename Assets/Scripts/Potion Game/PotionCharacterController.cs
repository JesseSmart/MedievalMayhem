using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class PotionCharacterController : NetworkBehaviour
{
    public Rigidbody rbody;
    public float speed;

	private bool isHolding;

	public GameObject holdPoint;
	private GameObject myObject;
	public GameObject[] ingredientArray;
	public Animator anim;

    public Material[] playerColours;

    public GameObject playerModelObj;
    public int playerNum;

    private int sabPNum;
    private GameObject saboteurIdentifier;
	private GameObject innocentIdentifier;

	public PlayerCustoms playerCustom;
    // Start is called before the first frame update
    void Start()
    {
        if (hasAuthority || FindObjectOfType<IDSaver>().savedID == 1)
        {
            playerNum = FindObjectOfType<IDSaver>().savedID - 1;
            playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[playerNum];
            CmdServerCharSetup(playerNum);

            //Set Identifier to show whether saboteur or innocent
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

	//Load cosmetics
	private void doTheLoad()
	{
		PlayerData data = SaveSystem.LoadPlayer();
		if (data == null)
		{
			data = new PlayerData();
		}
		CmdLoadCos(data.cust1Unlocked, data.cust2Unlocked, data.custom1Enabled, data.custom2Enabled);

	}

	[Command]
	void CmdLoadCos(bool b1, bool b2, bool equip1, bool equip2)
	{
		RpcLoadCos(b1, b2, equip1, equip2);
	}

	[ClientRpc]
	void RpcLoadCos(bool b1, bool b2, bool equip1, bool equip2)
	{
		playerCustom.custom1Unlocked = b1;
		playerCustom.custom2Unlocked = b2;
		playerCustom.custom1Enabled = equip1;
		playerCustom.custom2Enabled = equip2;
	}

	// Update is called once per frame
	void Update()
    {
		if (hasAuthority)
		{
			ingredientArray = GameObject.FindGameObjectsWithTag("Ingredient");
			SortArray();
			PickupCheck();
			Movement();
            RotationChar();
		}
    }

	//Character movement
    private void Movement()
    {


        Vector3 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput);
        inputVector = Vector3.ClampMagnitude(inputVector, 1); //1 might have to be baseMovementSpeed instead. clamp prevents diagonal movement being faster
        Vector3 movement = inputVector * speed;
        Vector3 newPos = currentPos + movement * Time.fixedDeltaTime;
        //transform.Translate(newPos);
        rbody.MovePosition(newPos);
		anim.SetFloat("mySpeed", Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")));
		CmdRecieveAnim(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	//Tell server to tell clients to animate
	[Command]
	void CmdRecieveAnim(float hor, float ver)
	{
		//anim.SetFloat("mySpeed", Mathf.Abs(hor) + Mathf.Abs(ver));
		RpcSendOutAnim(hor, ver);
	}

	//animate clients
	[ClientRpc]
	void RpcSendOutAnim(float hor, float ver)
	{
		anim.SetFloat("mySpeed", Mathf.Abs(hor) + Mathf.Abs(ver));
	}

	//character rotation
	private void RotationChar()
    {
        Vector3 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("LookHorizontal");
        float verticalInput = Input.GetAxis("LookVertical");


        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 lookPos = currentPos + inputVector;

		//if no input, do not rotate
        if (lookPos != Vector3.zero)
        {
            transform.LookAt(lookPos);

        }




    }

	//Pick up potions
	public void PickupCheck()
	{

		if (ingredientArray.Length > 0)
		{
			float dist = Vector3.Distance(transform.position, ingredientArray[0].transform.position);

			if (dist < 3)
			{
				if (Input.GetKey(KeyCode.Space) || Input.GetButton("P1AButton"))
				{
                    if (!isHolding)
                    {
                        myObject = ingredientArray[0];
                        isHolding = true;
                    }

					//pickupObject.GetComponent<BoxCollider>().enabled = false;
					//myObject.GetComponent<Rigidbody>().useGravity = false;
     //               CmdSetGravityState(myObject, false);
                    print(holdPoint);
                    CmdSetPotionPos(myObject, holdPoint); //THIS WILL NOT WORK COZ U CANT PASS GAME OBJECTS ____________-----------------------------------------------
					//myObject.transform.position = holdPoint.transform.position;
				}

				if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("P1AButton"))
				{

					//pickupObject.GetComponent<BoxCollider>().enabled = false;
					//myObject.GetComponent<Rigidbody>().useGravity = true;
     //               CmdSetGravityState(myObject, true);

                    myObject = null;
                    isHolding = false;

					//ingredientArray[0].transform.position = holdPoint.transform.position;
				}


			}

		}


	}

	//Find closest potion
	void SortArray()
	{

		for (int j = 0; j < ingredientArray.Length - 1; j++)
		{
			for (int i = 0; i < ingredientArray.Length - j - 1; i++)
			{
				float dist = Vector3.Distance(transform.position, ingredientArray[i].transform.position);
				float nextDist = Vector3.Distance(transform.position, ingredientArray[i + 1].transform.position);

				if (nextDist < dist)
				{
					GameObject tempObj;
					tempObj = ingredientArray[i];
					ingredientArray[i] = ingredientArray[i + 1];
					ingredientArray[i + 1] = tempObj;
				}
			}
		}
	}

	//character setup on server
    [Command]
    void CmdServerCharSetup(int id)
    {
        playerNum = id;
        playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[id];
        RpcBackToClientSetup(id);
    }

	//character setup on client
    [ClientRpc]
    void RpcBackToClientSetup(int id)
    {
        playerNum = id;
        playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[id];

    }

	//potion gravity on server
    [Command]
    void CmdSetGravityState(GameObject item, bool b)
    {
        item.GetComponent<Rigidbody>().useGravity = b;
        RpcSendOutGravity(item, b);
    }

	//set gravity on client
    [ClientRpc]
    void RpcSendOutGravity(GameObject item, bool b)
    {
        item.GetComponent<Rigidbody>().useGravity = b;
    }

	//set position of potion to player holder
    [Command]
    void CmdSetPotionPos(GameObject item, GameObject holderEgo)
    {
        print(item.name);
        print(holdPoint.name);
        item.transform.position = holdPoint.transform.position;
        //RpcSendOutPos(item, holderEgo);

    }


    //[ClientRpc]
    //void RpcSendOutPos(GameObject item, GameObject holderEgo)
    //{
    //    print(item.name);
    //    print(holdPoint.name);
    //    //item.transform.position = holdPoint.transform.position;
    //}
}
