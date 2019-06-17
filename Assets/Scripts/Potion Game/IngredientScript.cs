using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientScript : MonoBehaviour
{

    public float ingredientValue;

    public float minValue;
    public float maxValue;

	public GameObject potionColourObj;
    // Start is called before the first frame update

    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
