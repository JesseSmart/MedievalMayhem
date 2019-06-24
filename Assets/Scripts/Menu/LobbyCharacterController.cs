﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class LobbyCharacterController : NetworkBehaviour
{
    public int playerNum;
    public GameObject[] readyTexts;
    private GameObject[] playerObjs;

    private GameObject sceneManager;

	public Material[] playerColours;

	[SyncVar]
    private bool hasReadied;

	public GameObject playerModelObj;
	// Start is called before the first frame update

	void Start()
    {
        sceneManager = GameObject.FindGameObjectWithTag("MinigameManager");
        playerObjs = GameObject.FindGameObjectsWithTag("Player");

        playerNum = playerObjs.Length - 1;
        readyTexts = sceneManager.GetComponent<LobbySceneManager>().readyTextArray;

		playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[playerNum];
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            InputManager(playerNum);
        }

		if (isServer)
		{
			int readyPlayers = 0;

			LobbyCharacterController[] playersInLobby = FindObjectsOfType<LobbyCharacterController>();
			foreach (LobbyCharacterController cha in playersInLobby)
			{
				if (cha.hasReadied)
				{
					readyPlayers++;
				}
			}

			if (readyPlayers == playersInLobby.Length)
			{
				sceneManager.GetComponent<LobbySceneManager>().CmdPlayersHaveReadyUp();
			}
		}
    }

    void InputManager(int pNum)
    {
        if (!hasReadied)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
				CmdReadyUp(pNum);
            }

        }
    }

	[Command]
	void CmdReadyUp(int pNum)
	{
		RpcSnedPlayerReadied(pNum);
	}

	[ClientRpc]
	void RpcSnedPlayerReadied(int pNum)
	{
		foreach (LobbyCharacterController cha in FindObjectsOfType<LobbyCharacterController>())
		{

			readyTexts[pNum].GetComponent<TextMeshProUGUI>().enabled = true;
			if (cha.playerNum == pNum)
			{
				cha.hasReadied = true;
			}
		}
	}
}