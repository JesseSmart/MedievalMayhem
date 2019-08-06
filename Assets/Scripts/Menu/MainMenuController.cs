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

	public Dropdown ddSQ;
	public Dropdown ddSR;
	public Dropdown ddAF;

	public Button btnPurchaseCos1;
	public Button btnPurchaseCos2;
	public GameObject pnlStore;
	public GameObject pnlPurchase;
	private int lastSelectedItem;

	public Button btnEquip1;
	public Button btnEquip2;

	
    // Start is called before the first frame update
    void Start()
    {
		SettingsLoad();
		doTheLoad();

		QualitySettings.shadows = playerCustom.shadowQuality;
		QualitySettings.shadowResolution = playerCustom.shadowResolution;
		QualitySettings.anisotropicFiltering = playerCustom.anisotropicFiltering;
		SetUI();

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
        //gameObject.SetActive(false);
    }

	public void NetworkBackToMenu()
	{
		panelNetwork.SetActive(false);

	}

	public void QuitGame()
    {
        Application.Quit();
    }

	//Custom/store

	public void PurchaseItemOne()
	{
		lastSelectedItem = 1;
		pnlStore.SetActive(false);
		pnlPurchase.SetActive(true);

	}

	public void PurchaseItemTwo()
	{
		lastSelectedItem = 2;
		pnlStore.SetActive(false);
		pnlPurchase.SetActive(true);
	}

	public void FinalPurchaseSelection()
	{
		if (lastSelectedItem == 1)
		{
			playerCustom.custom1Unlocked = true;
			btnPurchaseCos1.interactable = false;
			btnEquip1.interactable = true;
		}
		else if (lastSelectedItem == 2)
		{
			playerCustom.custom2Unlocked = true;
			btnPurchaseCos2.interactable = false;
			btnEquip2.interactable = true;
		}
		lastSelectedItem = 0;

		pnlStore.SetActive(true);
		pnlPurchase.SetActive(false);

		SaveSystem.SavePlayer(playerCustom);
		doTheLoad();
	}

	public void PurchBack()
	{
		lastSelectedItem = 0;
		pnlStore.SetActive(true);
		pnlPurchase.SetActive(false);
	}

	public void EquipItemOne()
	{
		playerCustom.custom1Enabled = !playerCustom.custom1Enabled;
		if (playerCustom.custom1Enabled)
		{
			btnEquip1.GetComponentInChildren<Text>().text = "Unequip";
		}
		else
		{
			btnEquip1.GetComponentInChildren<Text>().text = "Equip";
		}
		SaveSystem.SavePlayer(playerCustom);
		doTheLoad();
	}

	public void EquipItemTwo()
	{
		playerCustom.custom2Enabled = !playerCustom.custom2Enabled;
		if (playerCustom.custom2Enabled)
		{
			btnEquip2.GetComponentInChildren<Text>().text = "Unequip";
		}
		else
		{
			btnEquip2.GetComponentInChildren<Text>().text = "Equip";
		}
		SaveSystem.SavePlayer(playerCustom);
		doTheLoad();
	}

	public void SQSettingsChanged()
	{
		//playerCustom.shadowQuality = sq;
		switch (ddSQ.value)
		{
			case 0:
				playerCustom.shadowQuality = ShadowQuality.Disable;
				break;
			case 1:
				playerCustom.shadowQuality = ShadowQuality.HardOnly;
				break;
			case 2:
				playerCustom.shadowQuality = ShadowQuality.All;
				break;
		}
		QualitySettings.shadows = playerCustom.shadowQuality;
		SaveSystem.SavePlayer(playerCustom);
		SettingsLoad();
	}

	public void SRSettingsChanged()
	{
		//playerCustom.shadowQuality = sq;
		switch (ddSR.value)
		{
			case 0:
				playerCustom.shadowResolution = ShadowResolution.Low;
				break;
			case 1:
				playerCustom.shadowResolution = ShadowResolution.Medium;

				break;
			case 2:
				playerCustom.shadowResolution = ShadowResolution.High;

				break;
			case 3:
				playerCustom.shadowResolution = ShadowResolution.VeryHigh;

				break;
		}
		//QualitySettings.SetQualityLevel(ddSR.value, true);
		QualitySettings.shadowResolution = playerCustom.shadowResolution;
		SaveSystem.SavePlayer(playerCustom);
		SettingsLoad();
	}


	public void AFSettingsChanged()
	{
		//playerCustom.shadowQuality = sq;
		switch (ddAF.value)
		{
			case 0:
				playerCustom.anisotropicFiltering = AnisotropicFiltering.Disable;
				break;
			case 1:
				playerCustom.anisotropicFiltering = AnisotropicFiltering.Enable;
				break;

		}
		QualitySettings.anisotropicFiltering = playerCustom.anisotropicFiltering;
		SaveSystem.SavePlayer(playerCustom);
		SettingsLoad();
	}

	private void SettingsLoad()
	{
		PlayerData data = SaveSystem.LoadPlayer();
		if (data == null)
		{
			data = new PlayerData();
		}

		playerCustom.shadowQuality = data.shadowQ;
		playerCustom.shadowResolution = data.shadowR;
		playerCustom.anisotropicFiltering = data.aFiltering;
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

		playerCustom.custom1Enabled = data.custom1Enabled;
		playerCustom.custom2Enabled = data.custom2Enabled;


		print("Loaded Cust1: " + playerCustom.custom1Unlocked + " || Loaded Cust2: " + playerCustom.custom2Unlocked);

	}

	void SetUI()
	{
		switch (playerCustom.shadowQuality)
		{
			case ShadowQuality.Disable:
				ddSQ.value = 0;
				break;
			case ShadowQuality.HardOnly:
				ddSQ.value = 1;
				break;
			case ShadowQuality.All:
				ddSQ.value = 2;
				break;
		}

		switch (playerCustom.shadowResolution)
		{
			case ShadowResolution.Low:
				 ddSR.value = 0;
				break;
			case ShadowResolution.Medium:
				ddSR.value = 1;
				break;
			case ShadowResolution.High:
				ddSR.value = 2;
				break;
			case ShadowResolution.VeryHigh:
				ddSR.value = 3;
				break;			
		}


		switch (playerCustom.anisotropicFiltering)
		{
			case AnisotropicFiltering.Disable:
				  ddAF.value = 0;
				break;
			case AnisotropicFiltering.Enable:
				ddAF.value = 1;
				break;

		}

		if (playerCustom.custom1Unlocked)
		{
			btnPurchaseCos1.interactable = false;
		}
		else
		{
			btnEquip1.interactable = false;
		}

		if (playerCustom.custom2Unlocked)
		{
			btnPurchaseCos2.interactable = false;
		}
		else
		{
			btnEquip2.interactable = false;
		}

		if (playerCustom.custom1Enabled)
		{
			btnEquip1.GetComponentInChildren<Text>().text = "Unequip";
		}
		else
		{
			btnEquip1.GetComponentInChildren<Text>().text = "Equip";
		}

		if (playerCustom.custom2Enabled)
		{
			btnEquip2.GetComponentInChildren<Text>().text = "Unequip";
		}
		else
		{
			btnEquip2.GetComponentInChildren<Text>().text = "Equip";
		}

	
	}
}
