using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] obstaclesArray;
    //private int rndLane;
    //private int rndObstacle;
    private bool[] nowOccupiedLaneArray = new bool[5];

    [Space]
    public int minObstacleSpawn;
    public int maxObstacleSpawn;
    
    [Space]
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
            Spawner();
            spawnTimer = spawnInterval;
        }
    }

    void Spawner()
    {
        int rndObstacleTotal;
        rndObstacleTotal = Random.Range(minObstacleSpawn, maxObstacleSpawn);

        bool hasSpawned = false;
        for (int i = 0; i < rndObstacleTotal; i++)
        {
            int rndLane;
            rndLane = Random.Range(0, spawnPoints.Length);

            while (hasSpawned == false)
            {
                if (nowOccupiedLaneArray[rndLane] == false)
                {
                    nowOccupiedLaneArray[rndLane] = true;
                    SpawnObstacle(rndLane);
                    hasSpawned = true;
                }
                else
                {
                    rndLane = Random.Range(0, spawnPoints.Length);
                }

            }

        }




    }

    void SpawnObstacle(int lane)
    {
        Instantiate(obstaclesArray[Random.Range(0, obstaclesArray.Length)], spawnPoints[lane].transform.position, spawnPoints[lane].transform.rotation);
    }
}
