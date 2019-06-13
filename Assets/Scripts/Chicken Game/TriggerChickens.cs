using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TriggerChickens : MonoBehaviour
{

    public GameObject gameMaster;

    // Start is called before the first frame update
    void Start()
    {
        
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
            gameMaster.GetComponent<ChickenGameManager>().GainPoint();
        }
    }



}
