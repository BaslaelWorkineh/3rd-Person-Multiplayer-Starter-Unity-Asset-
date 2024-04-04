using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class OwnershipTransfer : MonoBehaviourPunCallbacks
{
    private Transform spawnPoint;

    void Start()
    {
        // Find the spawn point in the scene
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;

        // Check if this instance of the player is owned by the local player
        if (photonView.IsMine)
        {
            // Subscribe to disconnection event
            PhotonNetwork.AddCallbackTarget(this);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from disconnection event
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (photonView.IsMine)
        {
            // Transfer ownership of the player object to a new owner
            TransferOwnership();
        }
    }

    void TransferOwnership()
    {
        // Instantiate a new player object at the spawn point
        GameObject newPlayer = PhotonNetwork.Instantiate("PlayerPrefab", spawnPoint.position, spawnPoint.rotation);

        // Transfer ownership of the new player object to a random player in the room
        PhotonView newPlayerView = newPlayer.GetComponent<PhotonView>();
        if (newPlayerView != null)
        {
            Player[] players = PhotonNetwork.PlayerListOthers;
            if (players.Length > 0)
            {
                int randomIndex = Random.Range(0, players.Length);
                int newOwnerId = players[randomIndex].ActorNumber;

                newPlayerView.TransferOwnership(newOwnerId);
            }
            else
            {
                // If there are no other players in the room, destroy the new player object
                PhotonNetwork.Destroy(newPlayer);
            }
        }
    }
}
