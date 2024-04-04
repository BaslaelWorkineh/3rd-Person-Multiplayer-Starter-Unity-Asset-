using Photon.Pun;
using UnityEngine;

public class RoomExitManagerTwo : MonoBehaviour
{
    void Start()
    {
        // Check if we are currently in a room
        if (PhotonNetwork.InRoom)
        {
            // Leave the current room
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            Debug.LogWarning("Not in a room, cannot exit.");
        }
    }

    // Callback for when the local player leaves the room
    public virtual void OnLeftRoom()
    {
        // Handle any necessary cleanup tasks, such as destroying spawned objects, resetting game state, etc.
        Debug.Log("Left room");

        // For example, you might want to unload a scene specific to the room
        // SceneManager.UnloadSceneAsync("YourRoomSceneName");
    }
}
