using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class IngredientSpawner : NetworkBehaviour
{

    public float spawnInterval;
    private float spawnTimer;


    public float zoneWidth;
    public float zoneHeight;


    public GameObject[] ingredientItems;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnIngredient();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnIngredient()
    {
        float w = transform.localPosition.x + Random.Range(-zoneWidth, zoneWidth);
        float h = transform.localPosition.z + Random.Range(-zoneHeight, zoneHeight);
		RpcSpawn(w, h);
    }

	[ClientRpc]
	void RpcSpawn(float x, float y)
	{
        Instantiate(ingredientItems[Random.Range(0, ingredientItems.Length)], new Vector3(x, transform.position.y, y), transform.rotation);

	}
}
