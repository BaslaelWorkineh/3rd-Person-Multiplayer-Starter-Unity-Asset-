using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public GameObject lobbyCanvas;
    public GameObject roomManagerCanvas;

    public GameObject player;
    [Space]
    public Transform[] spawnPoints;

    [Space]
    public GameObject roomCam;

    private string nickname = "unnamed";

    public string roomNameToJoin = "test";

    [HideInInspector]
    public int kills = 0;
    [HideInInspector]
    public int deaths = 0;

    [Space]
    public GameObject nameUI;
    public GameObject disconnectingUI;
    public GameObject connectingtoServer;

    public GameObject suddenConnectionProblem;
    public GameObject ThrowableInstantiator;

    void Awake()
    {
        instance = this;
    }
   void Start()
    {
        // Check if we are currently connecting to the Photon server
        if (!PhotonNetwork.IsConnected && PhotonNetwork.NetworkingClient.State == ClientState.PeerCreated)
        {
            Debug.Log("Trying to connect");
            // Start the connection process
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log("Already Connected");
        }
        suddenConnectionProblem.SetActive(false);
        ThrowableInstantiator.SetActive(false);
    }

   
    public void ChangeNickName(string _name)
    {
        nickname = _name;
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        // Once connected to the Master Server, join the lobby
        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        // Now that we've joined the lobby, attempt to join or create the room
    }

    public void JoinRoomButtonPressed()
    {
            Debug.Log("Connecting...");
            PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null);    
    }

    void Update()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        if(PhotonNetwork.InLobby)
        {
            nameUI.SetActive(true);
            connectingtoServer.SetActive(false);
        }
        else{
            connectingtoServer.SetActive(true);
            nameUI.SetActive(false);
        } 
    }


    public override void OnJoinedRoom()
    {
        Debug.Log(" We're connected and in a room");
        roomCam.SetActive(false);
        spawnPlayer();
       
    }

    public void spawnPlayer()
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;
        _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, nickname);
        PhotonNetwork.LocalPlayer.NickName = nickname;
        suddenConnectionProblem.SetActive(true);
        ThrowableInstantiator.SetActive(true);
    }

    public void SetHashes()
    {
        try
        {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;

            hash["kills"] = instance.kills;
            hash["deaths"] = instance.deaths;

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        catch
        {

        }
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join or create a room. Reason: " + message);
        disconnectingUI.SetActive(true); // Show the join room failure UI
       
    }
     // Added variable for the disconnected UI canvas

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from the server. Reason: " + cause);
        disconnectingUI.SetActive(true); // Show the disconnected UI
      
    }

    public void RetryConnection()
    {
        Debug.Log("Retrying Connection");
        PhotonNetwork.ConnectUsingSettings(); // Attempt to reconnect
        disconnectingUI.SetActive(false); // Hide the disconnected UI
        lobbyCanvas.SetActive(true); // Show the name UI to join/create a room again
        nameUI.SetActive(true);
        roomManagerCanvas.SetActive(false);
        
    }
}
