using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardScreenActivator : MonoBehaviour
{
    public string parentName = "Leaderboard"; // Name of the parent GameObject
    public string playerHolderName = "PlayerHolder"; // Name of the Player holder GameObject

    private GameObject playerHolder; // Reference to the Player holder GameObject

    public void LeaderboardBtnClicked()
    {
        // Find the parent GameObject by its name
        GameObject parentGameObject = GameObject.Find(parentName);

        // Check if the parent GameObject is found
        if (parentGameObject != null)
        {
            // Search for the Player holder GameObject within the parent
            playerHolder = parentGameObject.transform.Find(playerHolderName)?.gameObject;

            // Check if the Player holder is found
            if (playerHolder != null)
            {
                // Activate the Player holder GameObject
                playerHolder.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Player holder not found!");
            }
        }
        else
        {
            Debug.LogWarning("Parent GameObject not found!");
        }
    }
}
