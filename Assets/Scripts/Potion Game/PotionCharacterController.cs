﻿using System.Collections;
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

	public PlayerCustoms playerCustom;
    // Start is called before the first frame update
    void Start()
    {
        if (hasAuthority || FindObjectsOfType<PlayerConnectedObject>().Length == 1)
        {
            playerNum = FindObjectOfType<IDSaver>().savedID - 1;
            playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[playerNum];
            CmdServerCharSetup(playerNum);

            //SAB Stuff
            sabPNum = FindObjectOfType<IDSaver>().sabNum;
            saboteurIdentifier = GameObject.FindGameObjectWithTag("SaboteurIdentifier");
            if (playerNum == sabPNum)
            {
                saboteurIdentifier.GetComponent<Image>().color = Color.red;

            }
            else
            {
                saboteurIdentifier.GetComponent<Image>().color = Color.green;

            }
			doTheLoad();
        }
    }

	private void doTheLoad()
	{
		PlayerData data = SaveSystem.LoadPlayer();
		if (data == null)
		{
			data = new PlayerData();
		}

		//playerCustom.custom1Unlocked = data.cust1Unlocked;
		//playerCustom.custom2Unlocked = data.cust2Unlocked;

		//playerCustom.SetCos1(playerCustom.custom1Unlocked);
		//playerCustom.SetCos2(playerCustom.custom2Unlocked);

		//print("Loaded Cust1: " + playerCustom.custom1Unlocked + " || Loaded Cust2: " + playerCustom.custom2Unlocked);

		//CmdLoadCos(playerCustom.custom1Unlocked, playerCustom.custom2Unlocked);
		CmdLoadCos(data.cust1Unlocked, data.cust2Unlocked);

	}

	[Command]
	void CmdLoadCos(bool b1, bool b2)
	{
		RpcLoadCos(b1, b2);
	}

	[ClientRpc]
	void RpcLoadCos(bool b1, bool b2)
	{
		playerCustom.custom1Unlocked = b1;
		playerCustom.custom2Unlocked = b2;
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

	[Command]
	void CmdRecieveAnim(float hor, float ver)
	{
		anim.SetFloat("mySpeed", Mathf.Abs(hor) + Mathf.Abs(ver));
		RpcSendOutAnim(hor, ver);
	}
	[ClientRpc]
	void RpcSendOutAnim(float hor, float ver)
	{
		anim.SetFloat("mySpeed", Mathf.Abs(hor) + Mathf.Abs(ver));
	}

	private void RotationChar()
    {
        Vector3 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("LookHorizontal");
        float verticalInput = Input.GetAxis("LookVertical");


        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 lookPos = currentPos + inputVector;

        if (lookPos != Vector3.zero)
        {
            transform.LookAt(lookPos);

        }




    }


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
					myObject.GetComponent<Rigidbody>().useGravity = false;
                    CmdSetGravityState(myObject, false);
                    print(holdPoint);
                    CmdSetPotionPos(myObject, holdPoint);
					//myObject.transform.position = holdPoint.transform.position;
				}

				if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("P1AButton"))
				{

					//pickupObject.GetComponent<BoxCollider>().enabled = false;
					myObject.GetComponent<Rigidbody>().useGravity = true;
                    CmdSetGravityState(myObject, true);

                    myObject = null;
                    isHolding = false;
					//ingredientArray[0].transform.position = holdPoint.transform.position;
				}


			}

		}


	}

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


    [Command]
    void CmdServerCharSetup(int id)
    {
        playerNum = id;
        playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[id];
        RpcBackToClientSetup(id);
    }

    [ClientRpc]
    void RpcBackToClientSetup(int id)
    {
        playerNum = id;
        playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[id];

    }

    [Command]
    void CmdSetGravityState(GameObject item, bool b)
    {
        item.GetComponent<Rigidbody>().useGravity = b;
        RpcSendOutGravity(item, b);
    }

    [ClientRpc]
    void RpcSendOutGravity(GameObject item, bool b)
    {
        item.GetComponent<Rigidbody>().useGravity = b;
    }

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
