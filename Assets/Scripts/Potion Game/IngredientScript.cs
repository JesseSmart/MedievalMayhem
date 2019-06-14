using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientScript : MonoBehaviour
{

    public float ingredientValue;

    public float minValue;
    public float maxValue;
    // Start is called before the first frame update
    void Start()
    {
        ingredientValue = Random.Range(minValue, maxValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
