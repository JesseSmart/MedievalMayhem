using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PotionCharacterController : NetworkBehaviour
{
    public Rigidbody rbody;
    public float speed;

	private bool hasPickedUp;

	public GameObject holdPoint;
	private GameObject myObject;
	public GameObject[] ingredientArray;
	public Animator anim;
	// Start is called before the first frame update
	void Start()
    {

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

		Vector3 rot = transform.eulerAngles;
		rot.y = 180;//mathf.lerop from y > targetangflr
		transform.eulerAngles = rot;


		//transform.rotation = Quaternion.AngleAxis(90, Vector3.forward) * inputVector.normalized;
		//transform.rotation = Quaternion.Euler((0, 90, 0) * (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))));
		//transform.rotation = Quaternion.Lerp(transform.rotation, )
		//transform.rotation.y = Rotate(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 90);
	}



	public void PickupCheck()
	{
		if (ingredientArray.Length > 0)
		{
			float dist = Vector3.Distance(transform.position, ingredientArray[0].transform.position);

			if (dist < 5)
			{
				if (Input.GetKey(KeyCode.Space))
				{

					//pickupObject.GetComponent<BoxCollider>().enabled = false;
					ingredientArray[0].GetComponent<Rigidbody>().useGravity = false;
					ingredientArray[0].transform.position = holdPoint.transform.position;
				}

				if (Input.GetKeyUp(KeyCode.Space))
				{

					//pickupObject.GetComponent<BoxCollider>().enabled = false;
					ingredientArray[0].GetComponent<Rigidbody>().useGravity = true;

					ingredientArray[0].transform.position = holdPoint.transform.position;
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
}
