using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustoms : MonoBehaviour
{


	public bool custom1Unlocked;
	public bool custom2Unlocked;

	public bool custom1Enabled;
	public bool custom2Enabled;

	public GameObject cosItem1;
	public GameObject cosItem2;

	private bool hasRunCos1;
	private bool hasRunCos2;


	public ShadowResolution shadowResolution;
	public ShadowQuality shadowQuality;
	public AnisotropicFiltering anisotropicFiltering;


	// Start is called before the first frame update
	void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
		if (custom1Unlocked && custom1Enabled && !hasRunCos1)
		{
			cosItem1.SetActive(true);
			hasRunCos1 = true;
		}

		if (custom2Unlocked && custom2Enabled && !hasRunCos2)
		{
			cosItem2.SetActive(true);
			hasRunCos2 = true;
		}
	}
}
