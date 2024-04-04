using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class LadderClimbing : MonoBehaviour
{
    public float climbSpeed = 5f; // Adjust this value to control climbing speed
    private bool isClimbing = false;
    private Transform ladderTransform; // Reference to the ladder's transform

    void Update()
    {
        if (isClimbing)
        {
            // Check if the climb button is pressed
            if (CrossPlatformInputManager.GetButton("Climb"))
            {
                // Move the player upwards relative to the ladder's up direction
                transform.Translate(Vector3.up * climbSpeed * Time.deltaTime);
                Debug.Log(Vector3.up * climbSpeed * Time.deltaTime);
            }
            // Check if the climb button is released
            else
            {
                // Stop the player's movement
                transform.Translate(Vector3.zero);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            Debug.Log("Ladder Entered");
            // Start climbing when entering the ladder zone
            isClimbing = true;
            // Set the ladder's transform reference
            ladderTransform = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            Debug.Log("Ladder Exited");
            // Stop climbing when exiting the ladder zone
            isClimbing = false;
            // Clear the ladder's transform reference
            ladderTransform = null;
        }
    }

    void FixedUpdate()
    {
        // Adjust the player's rotation to match the ladder's rotation while climbing
        if (isClimbing && ladderTransform != null)
        {
            Vector3 ladderForward = ladderTransform.forward;
            ladderForward.y = 0f; // Ignore the ladder's vertical direction
            if (ladderForward != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(ladderForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
            }
        }
    }
}
