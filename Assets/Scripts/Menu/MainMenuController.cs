using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuController : MonoBehaviour
{

    public int loadScene;
    public GameObject panelNetwork;
	public PlayerCustoms playerCustom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnlinePlayPressed()
    {
        PlayerPrefs.SetInt("isOnline", 1);

        SceneManager.LoadScene(loadScene);
    }

    public void OfflinePlayPressed()
    {
        PlayerPrefs.SetInt("isOnline", 0);

        SceneManager.LoadScene(loadScene);
    }

    public void GoToNetworkPanelClick()
    {
        panelNetwork.SetActive(true);
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

	//Custom/store

	public void PurchaseItemOne()
	{
		playerCustom.custom1Unlocked = true;
		SaveSystem.SavePlayer(playerCustom);
		doTheLoad();
	}

	public void PurchaseItemTwo()
	{
		playerCustom.custom2Unlocked = true;
		SaveSystem.SavePlayer(playerCustom);
		doTheLoad();
	}

	private void doTheLoad()
	{
		PlayerData data = SaveSystem.LoadPlayer();
		if (data == null)
		{
			data = new PlayerData();
		}

		playerCustom.custom1Unlocked = data.cust1Unlocked;
		playerCustom.custom2Unlocked = data.cust2Unlocked;


		print("Loaded Cust1: " + playerCustom.custom1Unlocked + " || Loaded Cust2: " + playerCustom.custom2Unlocked);

	}
}
