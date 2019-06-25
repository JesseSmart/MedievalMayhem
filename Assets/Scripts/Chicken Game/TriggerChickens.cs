using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TriggerChickens : MonoBehaviour
{

    private GameObject minigameManager;

    // Start is called before the first frame update
    void Start()
    {
        minigameManager = GameObject.FindGameObjectWithTag("MinigameManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Chicken"))
        {
            other.gameObject.GetComponent<ChickenAI>().Capture();
            minigameManager.GetComponent<ChickenGameManager>().GainPoint();
        }
    }



}
