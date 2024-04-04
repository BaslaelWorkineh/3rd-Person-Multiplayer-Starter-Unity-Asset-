using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPun
{
    public GameObject enemyPrefab;
    public int maxEnemies = 5;
    public float spawnDelay = 2f;

    private int currentEnemies;

    void Update()
    {
        currentEnemies = CountEnemies();
        // Check if there are less than maxEnemies enemies in the scene
        if (currentEnemies < maxEnemies)
        {
            // Start spawning enemies
            if (PhotonNetwork.PlayerList.Length > 0)
            {
                SpawnEnemies();
                Debug.Log("Spawning Enemies");
            }
            Debug.Log("Enemy is less than Max");
        }
        else
        {
            Debug.Log("Enemy is at Max");
        }

        // If there are no players in the scene, reset the currentEnemies count
        if (PhotonNetwork.PlayerList.Length <= 0)
        {
            currentEnemies = 0;
        }
    }

    void SpawnEnemies()
    {
        // Calculate how many enemies need to be spawned
        int enemiesToSpawn = maxEnemies - currentEnemies;

        // Spawn enemies
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Instantiate enemy at a random position around the spawner
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 5f;
            spawnPosition.y = 7.67f; // Ensure enemies spawn at ground level
            GameObject enemy = PhotonNetwork.Instantiate(enemyPrefab.name, spawnPosition, Quaternion.identity);

            if (enemy == null)
            {
                Debug.LogError("Enemy instantiation failed!");
            }
            else
            {
                Debug.Log("Enemy instantiated: " + enemy.name);
                currentEnemies++;
            }
        }
    }

    // Count the number of enemies currently in the scene
    int CountEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length;
    }

    // Called by enemies when they die to decrement the count of current enemies
    public void EnemyDied()
    {
        currentEnemies--;
    }
}
