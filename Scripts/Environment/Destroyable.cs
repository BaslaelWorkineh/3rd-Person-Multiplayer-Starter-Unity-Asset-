using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public float deathDelay = 2f;
    void Start()
    {
        // Invoke the DestroyObject method with a delay of 2 seconds
        Invoke("DestroyObject", deathDelay);
    }

    void DestroyObject()
    {
        // Destroy the GameObject this script is attached to
        Destroy(gameObject);
    }
}