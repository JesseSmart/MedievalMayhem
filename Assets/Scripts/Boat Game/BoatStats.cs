using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatStats : MonoBehaviour
{

    public int health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDeath();
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

}
