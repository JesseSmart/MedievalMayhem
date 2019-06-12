using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAI : MonoBehaviour
{
    private GameObject[] playerObjs;
    public float minFleeRange;
    // Start is called before the first frame update
    void Start()
    {
        playerObjs = GameObject.FindGameObjectsWithTag("Player");   
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        for (int i = 0; i < playerObjs.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, playerObjs[i].transform.position);

            if (dist <= minFleeRange)
            {
                transform.position = -Vector3.MoveTowards(transform.position, playerObjs[i].transform.position, 0.5f * Time.deltaTime);
            }
        }
    }
}
