using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectedObject : NetworkBehaviour
{
    //public static int playerTotal;
    public GameObject playerUnitPrefab;
    //GameObject myPlayerUnit;

    private GameObject boatObj;
    private GameObject[] networkPlayerObjs;
    private int netPlayerNum;
    private GameObject[] spArray;

    // Start is called before the first frame update
    void Start()
    {
        //playerTotal++;
        CmdSpawnMyUnit();

        if (isLocalPlayer)
        {
            //Instantiate(playerUnitPrefab);
            Debug.Log("PlayerConnectionObject:: Start -- Spawnming my own pwersonal unit.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Command]
    void CmdSpawnMyUnit()
    {
        boatObj = GameObject.FindGameObjectWithTag("Boat");
        spArray = boatObj.GetComponent<BoatStats>().spawnPointArray;
        networkPlayerObjs = GameObject.FindGameObjectsWithTag("NetworkPlayerObject");
        netPlayerNum = networkPlayerObjs.Length; //player 1 = int 0, just so you know


        GameObject go = Instantiate(playerUnitPrefab, spArray[netPlayerNum].transform);


        //myPlayerUnit = go;

        //go.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }

    
}
