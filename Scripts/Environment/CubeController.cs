using UnityEngine;
using Photon.Pun;

public class CubeController : MonoBehaviourPun
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject deathEffectPrefab;
    public AudioClip deathSound; 
    public AudioSource audioSource;
    

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (!photonView.IsMine) return;

        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Die();
        }
    }

    void Die()
    {
        if (deathEffectPrefab != null)
        {
            PhotonNetwork.Instantiate(deathEffectPrefab.name, transform.position, Quaternion.identity);
        }
        PhotonNetwork.Destroy(gameObject);
    }

    void OnHealthChanged(float newValue)
    {
        currentHealth = newValue;
    }
}
