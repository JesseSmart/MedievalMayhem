﻿using System.Collections;
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
    [SyncVar]
    private int sabPNum;

    // Start is called before the first frame update
    void Start()
    {
        boatObj = GameObject.FindGameObjectWithTag("Boat");
        rbody = boatObj.GetComponent<Rigidbody>();

		sceneManager = GameObject.FindGameObjectWithTag("MinigameManager");


        if (isClient)
        {
            playerNum = PlayerPrefs.GetInt("LocalPlayerNum") - 1;
        }

        if (isServer)
        {
            sabPNum = PlayerPrefs.GetInt("SabPlayerNumber");
        }

        saboteurIdentifier = GameObject.FindGameObjectWithTag("SaboteurIdentifier");
        if (playerNum == sabPNum)
        {
            saboteurIdentifier.GetComponent<Image>().color = Color.red;

        }
        else
        {
            saboteurIdentifier.GetComponent<Image>().color = Color.green;

        }

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

}
