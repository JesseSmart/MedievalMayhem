using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ChickenPlayerController : NetworkBehaviour
{
    public int playerNum;
    public Rigidbody rbody;
    public float speed;
	public Animator anim;


    public Material[] playerColours;

    public GameObject playerModelObj;
    
    private int sabPNum;
    private GameObject saboteurIdentifier;
	private GameObject innocentIdentifier;

	public PlayerCustoms playerCustom;
    // Start is called before the first frame update
    void Start()
    {
        if (hasAuthority || FindObjectsOfType<PlayerConnectedObject>().Length == 1)
        {
            playerNum = FindObjectOfType<IDSaver>().savedID - 1;
            playerModelObj.GetComponent<SkinnedMeshRenderer>().material = playerColours[playerNum];
            CmdServerCharSetup(playerNum);


            //SAB Stuff
            sabPNum = FindObjectOfType<IDSaver>().sabNum;
			saboteurIdentifier = GameObject.FindGameObjectWithTag("SaboteurIdentifier");
			innocentIdentifier = GameObject.FindGameObjectWithTag("InnocentIdentifier");
			if (playerNum == sabPNum)
			{
				//saboteurIdentifier.GetComponent<Image>().color = Color.red;
				saboteurIdentifier.GetComponent<Image>().enabled = true;

			}
			else
			{
				//saboteurIdentifier.GetComponent<Image>().color = Color.green;
				innocentIdentifier.GetComponent<Image>().enabled = true;

			}

			doTheLoad();
        }
    }

	private void doTheLoad()
	{
		PlayerData data = SaveSystem.LoadPlayer();
		if (data == null)
		{
			data = new PlayerData();
		}
		CmdLoadCos(data.cust1Unlocked, data.cust2Unlocked, data.custom1Enabled, data.custom2Enabled);

	}

	[Command]
	void CmdLoadCos(bool b1, bool b2, bool equip1, bool equip2)
	{
		RpcLoadCos(b1, b2, equip1, equip2);
	}

	[ClientRpc]
	void RpcLoadCos(bool b1, bool b2, bool equip1, bool equip2)
	{
		playerCustom.custom1Unlocked = b1;
		playerCustom.custom2Unlocked = b2;
		playerCustom.custom1Enabled = equip1;
		playerCustom.custom2Enabled = equip2;
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
		CmdRecieveAnim(Input.GetAxis("Horizontal"), Input.GetAxis("Horizontal"));
    }

	[Command]
	void CmdRecieveAnim(float hor, float ver)
	{
		anim.SetFloat("mySpeed", Mathf.Abs(hor) + Mathf.Abs(ver));
		RpcSendOutAnim(hor, ver);
	}
	[ClientRpc]
	void RpcSendOutAnim(float hor, float ver)
	{
		anim.SetFloat("mySpeed", Mathf.Abs(hor) + Mathf.Abs(ver));
	}


    private void RotationChar()
    {
        Vector3 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("LookHorizontal");
        float verticalInput = Input.GetAxis("LookVertical");



        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 lookPos = currentPos + inputVector;


        if (inputVector != lookPos)
        {
            transform.LookAt(lookPos);

        }


    }
}
