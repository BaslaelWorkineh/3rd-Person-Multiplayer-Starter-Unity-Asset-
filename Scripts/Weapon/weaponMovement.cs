using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponMovement : MonoBehaviour
{
    private float XMove;
    private float YMove;
    private float XRotation;
    public Vector2 LockAxis;
    public float Sensivity = 10f;
    void Start()
    {
        
    }

    
    void Update()
    {
        XMove = LockAxis.x * Sensivity * Time.deltaTime;
        YMove = LockAxis.y * Sensivity * Time.deltaTime;
        XRotation -= YMove;
        XRotation = Mathf.Clamp(XRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(XRotation,0,0);
    }
}
