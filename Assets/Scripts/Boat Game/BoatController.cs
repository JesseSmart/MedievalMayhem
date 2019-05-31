using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public int playerNum;
    public Rigidbody rbody;
    public int currentSpeed;
    public GameObject testIndicatorObj;

    private string[] horizontalArray = new string[4] { "P1Horizontal", "P2Horizontal", "P3Horizontal", "P4Horizontal" };
    private string[] verticalArray = new string[4] { "P1Vertical", "P2Vertical", "P3Vertical", "P4Vertical" };
    private string[] leftBumperArray = new string[4] { "P1LeftBumper", "P2LeftBumper", "P3LeftBumper", "P4LeftBumper" };
    private string[] rightBumperArray = new string[4] { "P1RightBumper", "P2RightBumper", "P3RightBumper", "P4RightBumper" };

    public float scrollSpeed = 1f;
    public float constantForceBase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputBoat(playerNum);
        ConstantForce();
    }

    void InputBoat(int pNum)
    {
        if (Input.GetButtonDown(leftBumperArray[pNum]))
        {
            print("Player " + pNum + ": Pressed Left Bumper");
            //rbody.AddForce(-10, 0, 0);

            testIndicatorObj.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine(WaitSec(0.5f));

            MoveBoat(-1);
        }

        if (Input.GetButtonDown(rightBumperArray[pNum]))
        {
            print("Player " + pNum + ": Pressed Left Bumper");

            testIndicatorObj.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine(WaitSec(0.5f));

            MoveBoat(1);
        }

        //if (Input.GetButtonDown("joystick " + playerNum + " button " + 0))
        //if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        //{
        //    print("Player " + pNum + ": Pressed " + 0);
        //}

        if (Input.GetKeyDown(KeyCode.A) && playerNum == 0)
        {

            print("Player " + pNum + ": Pressed A");

            testIndicatorObj.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine(WaitSec(0.5f));

            MoveBoat(-1);
        }

        if (Input.GetKeyDown(KeyCode.D) && playerNum == 0)
        {

            print("Player " + pNum + ": Pressed A");

            testIndicatorObj.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine(WaitSec(0.5f));

            MoveBoat(1);
        }
    }

    void MoveBoat(int dir)
    {
        rbody.AddForce(dir * 10, 0, 0);
    }

    //void MovementFunction(int pNum)
    //{

    //    Vector3 currentPos = rbody.position;
    //    float horizontalInput = Input.GetAxis(horizontalArray[playerNum]);
    //    float verticalInput = Input.GetAxis(verticalArray[playerNum]);

    //    Vector3 inputVector = new Vector3(horizontalInput, verticalInput, 1);
    //    inputVector = Vector2.ClampMagnitude(inputVector, 1); //1 might have to be baseMovementSpeed instead. clamp prevents diagonal movement being faster
    //    Vector3 movement = inputVector * currentSpeed;
    //    Vector3 newPos = currentPos + movement * Time.fixedDeltaTime;
    //    //render
    //    rbody.MovePosition(newPos);
    //}

    IEnumerator WaitSec(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        testIndicatorObj.GetComponent<MeshRenderer>().enabled = false;

    }

    void ConstantForce() //look at this and the MovingWater script on the plane
    {
        float offset = Time.time * scrollSpeed;
        rbody.AddForce(Mathf.Sin(offset) * constantForceBase, 0, 0);

    }
}
