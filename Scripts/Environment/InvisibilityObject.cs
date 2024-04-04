using UnityEngine;
using System.Collections;

public class InvisibilityObject : MonoBehaviour
{
    public float invisibilityDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Apply invisibility to the player and its child objects
            ApplyInvisibility(other.gameObject);
        }
    }

    private void ApplyInvisibility(GameObject player)
    {
        Renderer[] renderers = player.GetComponentsInChildren<Renderer>();

        // Disable renderers of player and its child objects to make them invisible
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        // Start a coroutine to make the player and its child objects visible again after a duration
        StartCoroutine(MakePlayerVisible(renderers));
    }

    private IEnumerator MakePlayerVisible(Renderer[] renderers)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(invisibilityDuration);

        // Enable renderers of player and its child objects to make them visible again
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }
    }
}
