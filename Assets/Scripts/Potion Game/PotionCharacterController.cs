using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCharacterController : MonoBehaviour
{
    public Rigidbody rbody;
    public float speed;

	private bool hasPickedUp;

	public GameObject holdPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		//PickupCheck();
        Movement();
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
    }

	public void PickupCheck(GameObject pickupObject)
	{
		if (!hasPickedUp)
		{
			
			if (Input.GetKey(KeyCode.Space))
			{
				hasPickedUp = true;
				//pickupObject.GetComponent<BoxCollider>().enabled = false;

				pickupObject.transform.position = holdPoint.transform.position;
			}

				
		}

		if (Input.GetKeyUp(KeyCode.Space))
		{
			hasPickedUp = false;
			//pickupObject.GetComponent<BoxCollider>().enabled = true;
		}
	}
}
