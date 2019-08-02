using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustoms : MonoBehaviour
{


	public bool custom1Unlocked;
	public bool custom2Unlocked;

	public GameObject cosItem1;
	public GameObject cosItem2;

	private bool hasRunCos1;
	private bool hasRunCos2;


	// Start is called before the first frame update
	void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
		if (custom1Unlocked && !hasRunCos1)
		{
			cosItem1.SetActive(true);
			hasRunCos1 = true;
		}

		if (custom2Unlocked && !hasRunCos2)
		{
			cosItem2.SetActive(true);
			hasRunCos2 = true;
		}
	}



	//public void SetCust1(bool b)
	//{
	//	custom1Unlocked = b;
	//}

	//public void SetCust2(bool b)
	//{
	//	custom2Unlocked = b;
	//}
}
