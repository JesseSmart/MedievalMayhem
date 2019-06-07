using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatStats : MonoBehaviour
{
    public GameObject WaterObj;
    public float scrollSpeed = 1f;
    public float constantForceBase;
    public Rigidbody rbody;

    private bool isIncreasing;
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
            Destroy(gameObject);
                
        }
    }

    public void ConstantForce() //look at this and the MovingWater script on the plane
    {
        float offset = Time.time * scrollSpeed;
        WaterObj.GetComponent<MovingWater>().offset = offset;
        rbody.AddForce(Mathf.Sin(offset) * constantForceBase , 0, 0);

        print(offset + " | " + scrollSpeed);

        //if (offset >= scrollSpeed)
        //{
        //    isIncreasing = true;
        //}
        //else if (offset <= -scrollSpeed)
        //{
        //    isIncreasing = false;
        //}
        //if (isIncreasing)
        //{
        //    rbody.AddForce(constantForceBase, 0, 0);

        //}
        //else
        //{
        //    rbody.AddForce(-constantForceBase, 0, 0);

        //}


        //if it decreasing force in opposite direction

    }

}
