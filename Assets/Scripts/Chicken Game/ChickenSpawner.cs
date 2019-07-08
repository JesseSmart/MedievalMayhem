using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class ChickenSpawner : NetworkBehaviour
{

    public float zoneWidth;
    public float zoneHeight;

    public float spawnInterval;
    private float spawnTimer;

    public GameObject chickenObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (isServer)
		{
			spawnTimer -= Time.deltaTime;
			if (spawnTimer <= 0)
			{
				SpawnChicken();
				spawnTimer = spawnInterval;
			}
		}
    }

    private void SpawnChicken()
    {
        float w = transform.localPosition.x + Random.Range(-zoneWidth, zoneWidth);
        float h = transform.localPosition.z + Random.Range(-zoneHeight, zoneHeight);
		RpcSpawn(w, h);

    }

	[ClientRpc]
	void RpcSpawn(float x, float y)
	{
		GameObject chicken = Instantiate(chickenObj, new Vector3(x, 0, y), transform.rotation);
		NetworkServer.Spawn(chicken);

	}
}
