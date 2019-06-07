using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{

    public GameObject[] terrainObjArray;

    public float spawnInterval;
    private float spawnTimer;


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
            Instantiate(terrainObjArray[Random.Range(0, terrainObjArray.Length)], transform.position, transform.rotation);
            spawnTimer = spawnInterval;
        }
    }
}
