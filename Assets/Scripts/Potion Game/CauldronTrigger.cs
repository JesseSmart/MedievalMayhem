using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronTrigger : MonoBehaviour
{

    public float liquidValue;

    public float failureThreshold;

    public Color cauldronColour;
    public float colorFloat;

    public float gameDuration;
    private float gameTimer;

    public GameObject minigameManager;
    // Start is called before the first frame update
    void Start()
    {
        minigameManager = GameObject.FindGameObjectWithTag("MinigameManager");

    }

    // Update is called once per frame
    void Update()
    {
        if (liquidValue >= failureThreshold || liquidValue <= -failureThreshold)
        {
            //fail
            minigameManager.GetComponent<PotionGameManager>().GameEnd();
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
