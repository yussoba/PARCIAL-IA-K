using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public GameObject foodPrefab;   
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {

        Destroy(this.gameObject);

        /*Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Boid"))
        {
            
            Destroy(foodPrefab);
        }
        */
    }

    /*private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Boid"))
        {
            
            Destroy(foodPrefab);
        }
    }*/
}
