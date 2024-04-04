using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float xRange = 1.0f; // Range of movement in x direction
    public float yRange = 1.0f; // Range of movement in y direction
    public float zRange = 1.0f; // Range of movement in z direction
    public float speed = 1.0f; // Speed of the movement

    private Vector3 initialPosition; // Store the initial position of the camera

    void Start()
    {
        initialPosition = transform.position; // Record the initial position of the camera
    }

    void Update()
    {
        // Calculate the position offsets using random ranges
        float x = Mathf.PingPong(Time.time * speed, xRange * 2) - xRange;
        float y = Mathf.PingPong(Time.time * speed, yRange * 2) - yRange;
        float z = Mathf.PingPong(Time.time * speed, zRange * 2) - zRange;

        // Calculate the new position based on initial position and offsets
        Vector3 newPosition = initialPosition + new Vector3(x, y, z);

        // Apply the new position to the camera
        transform.position = newPosition;
    }
}
