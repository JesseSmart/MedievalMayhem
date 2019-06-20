using System.Collections;
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

    private bool hasReadied;
    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.FindGameObjectWithTag("MinigameManager");
        playerObjs = GameObject.FindGameObjectsWithTag("Player");

        playerNum = playerObjs.Length - 1;
        readyTexts = sceneManager.GetComponent<LobbySceneManager>().readyTextArray;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            InputManager(playerNum);
        }
    }

    void InputManager(int pNum)
    {
        if (!hasReadied)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                readyTexts[pNum].GetComponent<TextMeshProUGUI>().enabled = true;
                sceneManager.GetComponent<LobbySceneManager>().PlayerReadyUp(pNum);
                hasReadied = true;
            }

        }
    }
}
