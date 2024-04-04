using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SuddenDisconnectionHandler : MonoBehaviourPunCallbacks
{
    public GameObject disconnectionCanvas;
    public TextMeshProUGUI connectionStatusText;
    public GameObject playerPrefab;
    public Transform spawnPoint;
    public float reconnectInterval = 5f;

    private bool isAttemptingReconnect = false;
    public string lastRoomName;

    void Start()
    {
        disconnectionCanvas.SetActive(false);
        lastRoomName = PhotonNetwork.CurrentRoom.Name;
    }

    // void Update()
    // {
    //     if(PhotonNetwork.CurrentRoom.Name != null)
    //        
    // }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Disconnected due to: {0}", cause);
        ShowConnectionError("Connection lost. Reconnecting...");
        TryReconnect();
    }
    
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected to Master Server.");
    }

    public override void OnJoinedLobby()
    {
        if (!string.IsNullOrEmpty(lastRoomName))
        {
            Debug.Log("Attempting to re-enter the room: " + lastRoomName);
            PhotonNetwork.RejoinRoom(lastRoomName);
        }
    }

     public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        lastRoomName = PhotonNetwork.CurrentRoom.Name;
        SpawnPlayer();
        HideConnectionError();
    }

    void TryReconnect()
    {
        if (!isAttemptingReconnect)
        {
            isAttemptingReconnect = true;
            InvokeRepeating("AttemptToReconnect", 0f, reconnectInterval);
        }
    }

    void AttemptToReconnect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Attempting to reconnect...");
            PhotonNetwork.ConnectUsingSettings();
           
        }
        else
        {
            Debug.Log("Is connected.");
            CancelReconnectionAttempts();
           
        }
    }

    void CancelReconnectionAttempts()
    {
        isAttemptingReconnect = false;
        CancelInvoke("AttemptToReconnect");
    }

    void ShowConnectionError(string errorMessage)
    {
        connectionStatusText.text = errorMessage;
        disconnectionCanvas.SetActive(true);
    }

    void HideConnectionError()
    {
        connectionStatusText.text = "";
        disconnectionCanvas.SetActive(false);
    }

    void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
    }
}
