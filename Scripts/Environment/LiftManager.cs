using UnityEngine;
using System.Collections;
public class LiftManager : MonoBehaviour
{
    public static LiftManager Instance;

    public Transform liftObject;
    public float liftSpeed = 5f;
    public float liftDistance = 5f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private bool isMoving = false;
    private Vector3 originalPos;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Store the initial position of the lift
        initialPosition = liftObject.position;
        // Calculate the target position for moving the lift up
        targetPosition = initialPosition + Vector3.up * liftDistance;

        originalPos = transform.position;
    }

    public void MoveLiftUp()
    {
        if (!isMoving)
        {
            // Start moving the lift towards the target position
            StartCoroutine(MoveLift(targetPosition));
        }
    }
    public void MoveLiftDown()
    {
        if(!isMoving)
        {
            StartCoroutine(MoveLift(originalPos));
        }
    }

    private IEnumerator MoveLift(Vector3 destination)
    {
        yield return new WaitForSeconds(3);
        isMoving = true;

        while (Vector3.Distance(liftObject.position, destination) > 0.01f)
        {
            // Move the lift towards the destination using Lerp
            liftObject.position = Vector3.Lerp(liftObject.position, destination, liftSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure the lift reaches the exact target position
        liftObject.position = destination;

        isMoving = false;
    }

    
}