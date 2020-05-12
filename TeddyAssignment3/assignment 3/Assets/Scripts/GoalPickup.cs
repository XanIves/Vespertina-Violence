using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GoalPickup : MonoBehaviour
{
    private int goalCount = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Goal"))
        {
            Destroy(other.gameObject);
            goalCount++;
        }
    }
}