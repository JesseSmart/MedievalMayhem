using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
	//public int totalWins;
	//public int totalLosses;

		public bool cust1Unlocked;
	public bool cust2Unlocked;


	public PlayerData (PlayerCustoms cust)
	{
		//totalWins = winner.totalWins;
		//totalLosses = winner.totalLosses;

		cust1Unlocked = cust.custom1Unlocked;
		cust2Unlocked = cust.custom2Unlocked;
	}

	public PlayerData()
	{
		//totalWins = 0;
		//totalLosses = 0;

		cust1Unlocked = false;
		cust2Unlocked = false;
	}

}
