using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class PotionCharacterController : NetworkBehaviour
{
    public Rigidbody rbody;
    public float speed;

	private bool isHolding;

	public GameObject holdPoint;
	private GameObject myObject;
	public GameObject[] ingredientArray;
	public Animator anim;

    public Material[] playerColours;

    public GameObject playerModelObj;
    public int playerNum;

    private int sabPNum;
    private GameObject saboteurIdentifier;
    // Start is called before the first frame update
    void Start()
    {
        if (hasAuthority)
        {
            playerNum = FindObjectOfType<IDSaver>().savedID - 1;
            playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[playerNum];
            CmdServerCharSetup(playerNum);

            //SAB Stuff
            sabPNum = FindObjectOfType<IDSaver>().sabNum;
            saboteurIdentifier = GameObject.FindGameObjectWithTag("SaboteurIdentifier");
            if (playerNum == sabPNum)
            {
                saboteurIdentifier.GetComponent<Image>().color = Color.red;

            }
            else
            {
                saboteurIdentifier.GetComponent<Image>().color = Color.green;

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
		if (hasAuthority)
		{
			ingredientArray = GameObject.FindGameObjectsWithTag("Ingredient");
			SortArray();
			PickupCheck();
			Movement();
            RotationChar();
		}
    }

    private void Movement()
    {


        Vector3 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput);
        inputVector = Vector3.ClampMagnitude(inputVector, 1); //1 might have to be baseMovementSpeed instead. clamp prevents diagonal movement being faster
        Vector3 movement = inputVector * speed;
        Vector3 newPos = currentPos + movement * Time.fixedDeltaTime;
        //transform.Translate(newPos);
        rbody.MovePosition(newPos);
		anim.SetFloat("mySpeed", Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")));

	}

    private void RotationChar()
    {
        Vector3 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("LookHorizontal");
        float verticalInput = Input.GetAxis("LookVertical");


        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 lookPos = currentPos + inputVector;

        if (lookPos != Vector3.zero)
        {
            transform.LookAt(lookPos);

        }




    }


	public void PickupCheck()
	{

		if (ingredientArray.Length > 0)
		{
			float dist = Vector3.Distance(transform.position, ingredientArray[0].transform.position);

			if (dist < 5)
			{
				if (Input.GetKey(KeyCode.Space) || Input.GetButton("P1AButton"))
				{
                    if (!isHolding)
                    {
                        myObject = ingredientArray[0];
                        isHolding = true;
                    }

					//pickupObject.GetComponent<BoxCollider>().enabled = false;
					myObject.GetComponent<Rigidbody>().useGravity = false;
					myObject.transform.position = holdPoint.transform.position;
				}

				if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("P1AButton"))
				{

					//pickupObject.GetComponent<BoxCollider>().enabled = false;
					myObject.GetComponent<Rigidbody>().useGravity = true;
                    isHolding = false;
					//ingredientArray[0].transform.position = holdPoint.transform.position;
				}


			}

		}


	}

	void SortArray()
	{

		for (int j = 0; j < ingredientArray.Length - 1; j++)
		{
			for (int i = 0; i < ingredientArray.Length - j - 1; i++)
			{
				float dist = Vector3.Distance(transform.position, ingredientArray[i].transform.position);
				float nextDist = Vector3.Distance(transform.position, ingredientArray[i + 1].transform.position);

				if (nextDist < dist)
				{
					GameObject tempObj;
					tempObj = ingredientArray[i];
					ingredientArray[i] = ingredientArray[i + 1];
					ingredientArray[i + 1] = tempObj;
				}
			}
		}
	}


    [Command]
    void CmdServerCharSetup(int id)
    {
        playerNum = id;
        playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[id];
        RpcBackToClientSetup(id);
    }

    [ClientRpc]
    void RpcBackToClientSetup(int id)
    {
        playerNum = id;
        playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[id];

    }
}
