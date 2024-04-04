using UnityEngine;
using Photon.Pun;

public class NetworkObjectInstantiator : MonoBehaviourPunCallbacks
{
    public GameObject objectPrefab;
    public Transform[] spawnPoints; // Array of spawn points
    public int numberOfObjectsToInstantiate = 5;
    private int currentInstantiated = 0;

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            // Loop to instantiate multiple objects
            for (int i = 0; i < numberOfObjectsToInstantiate; i++)
            {
                // Instantiate the object across the network
                GameObject obj = PhotonNetwork.Instantiate(objectPrefab.name, spawnPoints[i % spawnPoints.Length].position, spawnPoints[i % spawnPoints.Length].rotation);
                currentInstantiated++;
            }
        }
    }
}
