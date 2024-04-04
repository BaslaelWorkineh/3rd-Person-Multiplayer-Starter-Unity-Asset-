using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonConnection : MonoBehaviourPunCallbacks
{
    // public GameObject connectingCanvas;
    // void Start()
    // {
    //     ConnectToPhoton();
    // }

    // void ConnectToPhoton()
    // {
    //     PhotonNetwork.ConnectUsingSettings(); // Connect using Photon settings from the Unity Editor
    // }

    // public override void OnConnectedToMaster()
    // {
    //     connectingCanvas.SetActive(false);
    //     Debug.Log("Connected to Photon Master Server");
    //     // Once connected, you might want to join a lobby or perform other operations
    //     // For example:
    //     // PhotonNetwork.JoinLobby();
    //     // or
    //     // PhotonNetwork.JoinOrCreateRoom("RoomName", new RoomOptions { MaxPlayers = 4 }, null);
    // }

    // Other Photon callbacks like OnDisconnected, OnJoinRoomFailed, etc. can be overridden as needed
}
