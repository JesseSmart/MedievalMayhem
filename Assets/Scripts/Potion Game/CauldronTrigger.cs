using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class CauldronTrigger : NetworkBehaviour
{
    [SyncVar]
    public float liquidValue;

    public float failureThreshold;

    public Color cauldronColour;
    public float colorFloat;

    public float gameDuration;
    private float gameTimer;
    private bool cauldHasBlown;
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
            if (!cauldHasBlown)
            {
                //fail
                if (isServer)
                {

                    minigameManager.GetComponent<PotionGameManager>().CauldronBlew();

                }
                cauldHasBlown = true;

            }
        }
        colorFloat = (liquidValue + (failureThreshold / 2)) / failureThreshold;
        cauldronColour = Color.Lerp(Color.red, Color.blue, colorFloat);
        gameObject.GetComponent<Renderer>().material.color = cauldronColour;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ingredient"))
        {
            print("EAT POTION");
            if (isServer)
            {
                RpcIngredientAdd(other.gameObject.GetComponent<IngredientScript>().ingredientValue, other.gameObject);

            }
            //Destroy(other.gameObject);
        }
        else
        {
            print("NOOOOOOOOOOOOOOOOOOOOOOOOOO Object = " + other.gameObject.name);
            
        }

            
    }

    [ClientRpc]
    void RpcIngredientAdd(float pVal, GameObject obj)
    {
        liquidValue += pVal;
        Destroy(obj);
    }

    
}
