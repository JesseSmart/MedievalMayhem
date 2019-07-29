using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
	public int totalWins;
	public int totalLosses;

	public PlayerData (WinnerManager winner)
	{
		totalWins = winner.totalWins;
		totalLosses = winner.totalLosses;
	}

	public PlayerData()
	{
		totalWins = 0;
		totalLosses = 0;
	}

}
