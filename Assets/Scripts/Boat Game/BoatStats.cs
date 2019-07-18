using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class BoatStats : NetworkBehaviour
{
    public GameObject WaterObj;
    public float scrollSpeed = 1f;
    public float constantForceBase;
    public Rigidbody rbody;

    public GameObject[] spawnPointArray;
    //[SyncVar]
    public int health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDeath();
        ConstantForce();

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    void CheckDeath()
    {
        if (health <= 0)
        {
            print("DEAD");
            if (isServer)
            {
                FindObjectOfType<BoatGameManager>().BoatBroke();
            }
            Destroy(gameObject);
                
        }
    }

    public void ConstantForce() //look at this and the MovingWater script on the plane
    {
        if (isServer)
        {
            float offset = Time.time * scrollSpeed;
            WaterObj.GetComponent<MovingWater>().offset = offset;
            rbody.AddForce(Mathf.Cos(offset) * constantForceBase , 0, 0);

        }
        //print(scrollSpeed + " | " + Mathf.Cos(offset));



    }

}
