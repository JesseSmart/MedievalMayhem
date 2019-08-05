using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

	public bool cust1Unlocked;
	public bool cust2Unlocked;

	public bool custom1Enabled;
	public bool custom2Enabled;

	public ShadowResolution shadowR;
	public ShadowQuality shadowQ;
	public AnisotropicFiltering aFiltering;



	public PlayerData (PlayerCustoms cust)
	{
		cust1Unlocked = cust.custom1Unlocked;
		cust2Unlocked = cust.custom2Unlocked;

		//debug reset
		//cust1Unlocked = false;
		//cust2Unlocked = false;

		custom1Enabled = cust.custom1Enabled;
		custom2Enabled = cust.custom2Enabled;

		shadowR = cust.shadowResolution;
		shadowQ = cust.shadowQuality;
		aFiltering = cust.anisotropicFiltering;

	}

	public PlayerData()
	{
		cust1Unlocked = false;
		cust2Unlocked = false;

		custom1Enabled = false;
		custom2Enabled = false;

		shadowR = ShadowResolution.Low;
		shadowQ = ShadowQuality.Disable;
		QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
		

		Debug.Log("PLAYER DATA BLANK");
	}

}
