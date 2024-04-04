using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObjectToCamera : MonoBehaviour
{
    void Update()
    {
        if(Camera.main != null)
            transform.LookAt(Camera.main.transform);
    }
}
