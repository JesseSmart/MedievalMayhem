using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChickenPlayerController : NetworkBehaviour
{
    public int playerNum;
    public Rigidbody rbody;
    public float speed;
	public Animator anim;


    public Material[] playerColours;

    public GameObject playerModelObj;
    

    // Start is called before the first frame update
    void Start()
    {
        if (hasAuthority)
        {
            playerNum = FindObjectOfType<IDSaver>().savedID;
            playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[playerNum];
            CmdServerCharSetup(playerNum);

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

    // Update is called once per frame
    void Update()
    {
		if (hasAuthority)
		{
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
    }


}
