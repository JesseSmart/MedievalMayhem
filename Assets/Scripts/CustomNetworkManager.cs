using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkManager : NetworkManager
{
	private int tempTotal;

	NetworkStartPosition[] tempSpawn;
	//Host
	public void StartUpHost()
    {
        SetPort();
        NetworkManager.singleton.StartHost();

    }

	//JoinGame
    public void JoinGame()
    {
        SetIPAdress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

	//Set IP
    void SetIPAdress()
    {
        string ipAdress = GameObject.Find("InputFieldIPAdress").transform.Find("Text").GetComponent<Text>().text; //hmm
        NetworkManager.singleton.networkAddress = ipAdress;
    }


    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777; //hmm
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 0) // = menu
        {
            SetUpMenuSceneButtons();
        }
        else
        {
            SetupOtherSceneButtons();
			
		}

		if (FindObjectOfType<NetworkStartPosition>())
		{
			tempSpawn = FindObjectsOfType<NetworkStartPosition>();

		}
	}

    void SetUpMenuSceneButtons()
    {
        GameObject.Find("btnHost").GetComponent<Button>().onClick.RemoveAllListeners(); //button name?
        GameObject.Find("btnHost").GetComponent<Button>().onClick.AddListener(StartUpHost); //button name?

        GameObject.Find("btnJoin").GetComponent<Button>().onClick.RemoveAllListeners(); //button name?
        GameObject.Find("btnJoin").GetComponent<Button>().onClick.AddListener(JoinGame); //button name?
    }

    void SetupOtherSceneButtons() //does not exist yet and button name may change
    {
        //GameObject.Find("btnDisconnect").GetComponent<Button>().onClick.RemoveAllListeners(); //button name?
        //GameObject.Find("btnDisconnect").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost); //button name?
    }

    public void LoadGameScene(string sceneName)
    {
        ServerChangeScene(sceneName);
    }

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		StartCoroutine(WaitLoad(conn, playerControllerId));
	}

	IEnumerator WaitLoad(NetworkConnection conn, short playerControllerId) //THIS METHOD DOESNT WORK ON BOAT GAME
	{
		yield return new WaitForSeconds(0.5f);
		if (FindObjectOfType<NetworkStartPosition>())
		{
			GameObject player = (GameObject)Instantiate(playerPrefab, tempSpawn[playerControllerId].transform.position, Quaternion.identity);
			tempTotal++;
			NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		}
		else
		{
			GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
			NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		}
	}

}
