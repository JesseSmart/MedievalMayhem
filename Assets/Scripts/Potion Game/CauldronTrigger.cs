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
    private Renderer myRen;
    // Start is called before the first frame update
    void Start()
    {
        minigameManager = GameObject.FindGameObjectWithTag("MinigameManager");
        myRen = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            if ((2 * liquidValue) >= failureThreshold || (2 * liquidValue) <= -failureThreshold)
            {
                if (!cauldHasBlown)
                {
                    //fail

                        minigameManager.GetComponent<PotionGameManager>().CauldronBlew();

                    cauldHasBlown = true;

                }
            }

            colorFloat = (liquidValue + (failureThreshold / 2)) / failureThreshold;
            cauldronColour = Color.Lerp(Color.red, Color.blue, colorFloat);
            RpcColorThePot(colorFloat, cauldronColour);
        }

        //myRen.material.color = cauldronColour;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (isServer) //maybe make this for everyone, then send cmd that does rpc
        {
            if (other.gameObject.CompareTag("Ingredient"))
            {
                   RpcIngredientAdd(other.gameObject.GetComponent<IngredientScript>().ingredientValue, other.gameObject);
            }
        }

            
    }

    [ClientRpc]
    void RpcIngredientAdd(float pVal, GameObject obj)
    {
        liquidValue += pVal;
        Destroy(obj);
    }

    [ClientRpc]
    void RpcColorThePot(float cfloat, Color colour)
    {
		colorFloat = cfloat;
        myRen.material.color = colour;
    }

    
}
