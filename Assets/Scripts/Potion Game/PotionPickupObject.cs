﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPickupObject : MonoBehaviour
{
	public GameObject parentObj;

	private GameObject myObject;
	private int objsHeld;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	//private void OnTriggerStay(Collider other)
	//{
	//	if (other.gameObject.CompareTag("Ingredient"))
	//	{

	//		parentObj.GetComponent<PotionCharacterController>().PickupCheck(other.gameObject);


	//	}
	//}



}
