using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenAI : MonoBehaviour
{
    private GameObject[] playerObjs;
    public float minFleeRange;
    public float fleeSpeed;

    private NavMeshAgent agent;

    private bool isCaptured;
    private GameObject coopObj;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerObjs = GameObject.FindGameObjectsWithTag("Player");
        coopObj = GameObject.FindGameObjectWithTag("Coop");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCaptured)
        {
            CheckDistance();
        }
    }

    private void CheckDistance()
    {
        playerObjs = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < playerObjs.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, playerObjs[i].transform.position);

            if (dist <= minFleeRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerObjs[i].transform.position.x, transform.position.y, playerObjs[i].transform.position.z), -fleeSpeed * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(transform.position - playerObjs[i].transform.position);
            }
            else
            {
                //idle
                //transform.LookAt(playerObjs[i].transform.position);
            }
        }
    }

    public void Capture()
    {
        isCaptured = true;
        Destroy(gameObject.GetComponent<Rigidbody>());
        agent.SetDestination(coopObj.transform.position);
    }
}
