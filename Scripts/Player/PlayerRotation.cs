using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float rotationSpeed = 5.0f; // Speed of rotation

    private float rotationY = 0.0f; // Current rotation around Y-axis

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");

        rotationY -= mouseX * rotationSpeed;

        // Apply the rotation to the player around the Y-axis
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
