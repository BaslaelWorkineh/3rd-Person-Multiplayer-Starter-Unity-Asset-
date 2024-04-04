using UnityEngine;
using TMPro;
using Photon.Pun;

public class Health : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    public int health;

    public bool isLocalPlayer;

    public AudioClip deathSound; 
    public AudioSource audioSource;
    
    public GameObject deathEffectPrefab;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    [PunRPC]
    public void TakeDamage(int _damage)
    {
        health -= _damage;
        
        healthText.text = health.ToString();

        if(health <= 0)
        {
            if(isLocalPlayer)
            {
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer.ActorNumber);
                RoomManager.instance.spawnPlayer();
           
                RoomManager.instance.deaths++;
                RoomManager.instance.SetHashes();
            }
                
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            if (deathEffectPrefab != null)
            {
                PhotonNetwork.Instantiate(deathEffectPrefab.name, transform.position, Quaternion.identity);
            }
            
        }
    }
}
