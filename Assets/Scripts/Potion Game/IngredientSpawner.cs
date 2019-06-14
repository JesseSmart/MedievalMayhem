using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
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

        Instantiate(ingredientItems[Random.Range(0, ingredientItems.Length)], new Vector3(w, transform.position.y, h), transform.rotation);
    }
}
