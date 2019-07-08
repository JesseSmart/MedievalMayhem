using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class IngredientScript : NetworkBehaviour
{

    public float ingredientValue;

    public float minValue;
    public float maxValue;

	public GameObject potionColourObj;
    // Start is called before the first frame update

    void Start()
    {//stuff is purple error

		if (isServer)
		{
			ingredientValue = Random.Range(minValue, maxValue);
			if (ingredientValue >= 0)
			{
				potionColourObj.GetComponent<Renderer>().material.color = Color.blue;
			}
			else
			{
				potionColourObj.GetComponent<Renderer>().material.color = Color.red;
			}
			//RpcSetPotion(ingredientValue);

		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	[ClientRpc]
	void RpcSetPotion(float val)
	{
		

		if (val >= 0)
		{
			potionColourObj.GetComponent<Renderer>().material.color = Color.blue;
		}
		else
		{
			potionColourObj.GetComponent<Renderer>().material.color = Color.red;
		}
	}
}
