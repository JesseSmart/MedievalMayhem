using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
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
        for (int i = 0; i < rndObstacleTotal; i++)
        {
            int rndLane;
            rndLane = Random.Range(0, spawnPoints.Length);

            if (nowOccupiedLaneArray[rndLane] == false)
            {
                nowOccupiedLaneArray[rndLane] = true;
                //SpawnObstacle(rndLane);
                StartCoroutine(WaitAndSpawn(Random.Range(0, randomObstacleDelay), rndLane));
                hasSpawned = true;
            }
            else
            {

                for (int j = 0; j < 10; j++)
                {
                    rndLane = Random.Range(0, spawnPoints.Length);
                    if (hasSpawned == false)
                    {

                        nowOccupiedLaneArray[rndLane] = true;
                        StartCoroutine(WaitAndSpawn(Random.Range(0, randomObstacleDelay), rndLane));
                        //SpawnObstacle(rndLane);
                        hasSpawned = true;
                    }

                }
            }

            ResetArrays();
        }




        IEnumerator WaitAndSpawn(float waitTime, int lane)
        {
            yield return new WaitForSecondsRealtime(waitTime);
            Instantiate(obstaclesArray[Random.Range(0, obstaclesArray.Length)], spawnPoints[lane].transform.position, spawnPoints[lane].transform.rotation);
            //ResetArrays();

        }

        void ResetArrays()
        {
            for (int i = 0; i < nowOccupiedLaneArray.Length; i++)
            {
                nowOccupiedLaneArray[i] = false;
            }
        }

        void SpawnObstacle(int lane)
        {
            Instantiate(obstaclesArray[Random.Range(0, obstaclesArray.Length)], spawnPoints[lane].transform.position, spawnPoints[lane].transform.rotation);
        }

        IEnumerator CheckAllLanes(float waitTime, int lane)
        {
            return null;
        }
    }
}


