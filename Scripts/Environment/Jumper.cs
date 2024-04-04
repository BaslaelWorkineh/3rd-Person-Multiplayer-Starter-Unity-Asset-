using UnityEngine;

public class Jumper : MonoBehaviour
{
    public float jumpForce = 30f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Call a method to trigger the higher jump
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.jumpHeight = jumpForce;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Call a method to trigger the higher jump
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.jumpHeight = 10;
            }
        }
    }
}