using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject deathEffect;
    private EnemySpawner enemySpawner;
     public AudioClip deathSound; 
    public AudioSource audioSource;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
        if (enemySpawner == null)
        {
            Debug.LogError("EnemySpawner not found in the scene.");
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Die();
        }
    }

    void Die()
    {
        // Instantiate death particle effect
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        // Notify enemy spawner that an enemy died
        if (enemySpawner != null)
        {
            enemySpawner.EnemyDied();
        }
        RoomManager.instance.kills++;

        // Destroy the enemy GameObject
        Destroy(gameObject);
    }
}
