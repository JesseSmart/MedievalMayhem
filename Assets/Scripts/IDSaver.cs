using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDSaver : MonoBehaviour
{
    public int savedID;
	public int points;
    public int sabNum;
    public int[] levelLoadArray;
    public string[] levelNames;
    public int gamesPlayed;
    public int maxGames;
    public bool lastGameTeamWon;
	// Start is called before the first frame update
	void Start()
    {
        Object.DontDestroyOnLoad(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
