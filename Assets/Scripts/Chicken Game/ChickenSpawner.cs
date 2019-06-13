using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawner : MonoBehaviour
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
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnChicken();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnChicken()
    {
        float w = transform.localPosition.x + Random.Range(-zoneWidth, zoneWidth);
        float h = transform.localPosition.z + Random.Range(-zoneHeight, zoneHeight);

        Instantiate(chickenObj, new Vector3(w, 0, h), transform.rotation);

    }
}
