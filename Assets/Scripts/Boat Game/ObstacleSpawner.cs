using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ObstacleSpawner : NetworkBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] obstaclesArray;
    private int[] laneNumbers = new int[5] { 0, 1, 2, 3, 4 };
    //private int rndLane;
    //private int rndObstacle;
    private bool[] nowOccupiedLaneArray = new bool[5];

    [Space]
    public int minObstacleSpawn;
    public int maxObstacleSpawn;

    [Space]
    public float spawnInterval;
    private float spawnTimer;

    public float randomObstacleDelay;

    //private int totalSpawnedBatch;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {

        if (isServer)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                Spawner();
                spawnTimer = spawnInterval;
            }

        }
    }

    void Spawner()
    {
        //foreach (GameObject spawn in spawnPoints)
        //{
        //    int rndNum = Random.Range(0, 3);
        //    if (rndNum == 1)
        //    {
        //        Instantiate(obstaclesArray[Random.Range(0, obstaclesArray.Length)], spawn.transform.position, spawn.transform.rotation);
        //    }
        //}

        int rndObstacleTotal;
        rndObstacleTotal = Random.Range(minObstacleSpawn, maxObstacleSpawn);

        bool hasSpawned = false;

        int totalSpawnedBatch;
        totalSpawnedBatch = rndObstacleTotal;
        for (int i = 0; i < rndObstacleTotal; i++)
        {
            int rndLane;
            rndLane = Random.Range(0, spawnPoints.Length);

            if (nowOccupiedLaneArray[rndLane] == false)
            {
                nowOccupiedLaneArray[rndLane] = true;
                //SpawnObstacle(rndLane);
                StartCoroutine(WaitAndSpawn(Random.Range(0, randomObstacleDelay), rndLane, Random.Range(0, obstaclesArray.Length)));
                hasSpawned = true;
            }
            else
            {

                //StartCoroutine(CheckAllLanes(hasSpawned));

                for (int j = 0; j < 10; j++)
                {
                    rndLane = Random.Range(0, spawnPoints.Length);
                    if (hasSpawned == false)
                    {

                        nowOccupiedLaneArray[rndLane] = true;
                        StartCoroutine(WaitAndSpawn(Random.Range(0, randomObstacleDelay), rndLane, Random.Range(0, obstaclesArray.Length)));
                        //SpawnObstacle(rndLane);
                        hasSpawned = true;
                    }

                }
            }
            //if all spawned

            ResetArrays();
            
        }
    }


    
    IEnumerator WaitAndSpawn(float waitTime, int lane, int rndObstIndex)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        //Instantiate(obstaclesArray[rndObstIndex], spawnPoints[lane].transform.position, spawnPoints[lane].transform.rotation);
        //ResetArrays();
        CmdSpawnObstacle(lane, rndObstIndex);

    }

    [Command]
    void CmdSpawnObstacle(int lane, int index)
    {
        //totalSpawnedBatch--;
        GameObject obstacle = Instantiate(obstaclesArray[index], spawnPoints[lane].transform.position, spawnPoints[lane].transform.rotation);
        NetworkServer.Spawn(obstacle);
    }

    void ResetArrays()
    {
        for (int i = 0; i < nowOccupiedLaneArray.Length; i++)
        {
            nowOccupiedLaneArray[i] = false;
        }
    }

        

    IEnumerator CheckAllLanes(bool spawned)
    {
        while (spawned == false)
        {
            int rndLane;
            rndLane = Random.Range(0, spawnPoints.Length);

            if (nowOccupiedLaneArray[rndLane] == false)
            {
                nowOccupiedLaneArray[rndLane] = true;
                //SpawnObstacle(rndLane);
                StartCoroutine(WaitAndSpawn(Random.Range(0, randomObstacleDelay), rndLane, Random.Range(0, obstaclesArray.Length)));
                spawned = true;
            }
        }

        return null;
    }
}




