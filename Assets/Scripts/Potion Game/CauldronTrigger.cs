using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronTrigger : MonoBehaviour
{

    public float liquidValue;

    public float failureThreshold;

    public Color cauldronColour;
    public float colorFloat;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (liquidValue >= failureThreshold || liquidValue <= -failureThreshold)
        {
            //fail
        }
        colorFloat = (liquidValue + (failureThreshold / 2)) / failureThreshold;
        cauldronColour = Color.Lerp(Color.red, Color.blue, colorFloat);
        gameObject.GetComponent<Renderer>().material.color = cauldronColour;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ingredient"))
        {
            IngredientAdd(other.gameObject.GetComponent<IngredientScript>().ingredientValue);
            Destroy(other.gameObject);
        }

            
    }

    public void IngredientAdd(float pVal)
    {
        liquidValue += pVal;
    }
}
