using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Call a method to move the lift
            LiftManager.Instance.MoveLiftUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player Exited");
        if(other.CompareTag("Player"))
        {
            LiftManager.Instance.MoveLiftDown();
        }
    }
}

