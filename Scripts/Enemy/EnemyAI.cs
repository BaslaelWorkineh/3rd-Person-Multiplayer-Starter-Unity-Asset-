using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public string playerTag = "Player";
    public float moveSpeed = 3.0f;
    public float stoppingDistance = 5.0f; // Distance at which the enemy stops moving
    public float detectionRange = 10.0f; // Range within which the enemy can detect the player
    public float fireRate = 1.0f; // Rate of fire in bullets per second
    public Transform gunTip; // Transform representing the gun tip position
    public GameObject shootEffectPrefab; // Prefab of the shoot effect
    public int damageAmount = 10; // Damage amount to deduct from player's health
    public float wanderRadius = 5.0f; // Radius for wandering
    public float wanderInterval = 3.0f; // Interval for changing wander direction
    public float wanderTimer = 0.0f; // Timer for wander interval
    private Vector3 wanderTarget; // Target position for wandering

    private float nextFireTime;
    private bool isWandering = false;

    public AudioClip shootSound; // Variable to store the shooting sound effect
    public AudioSource audioSource; // Reference to the AudioSource component
    
    // Other variables...
    
    void Start()
    {
        SetNewWanderTarget();
         audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        // Calculate the direction to the player
        if(player !=null){
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0; // Ensure the object stays on the same level as the player

            // Check if the player is within detection range
            if (directionToPlayer.magnitude <= detectionRange)
            {
                // Rotate the object to face the player
                transform.LookAt(player.transform.position);
                isWandering = false;
                // Move towards the player
                if (directionToPlayer.magnitude > stoppingDistance)
                {
                    transform.Translate(directionToPlayer.normalized * moveSpeed * Time.deltaTime, Space.World);
                }
                else
                {
                    // Stop moving and shoot at the player
                    // You can add additional behavior here, such as playing an idle animation
                    Shoot(player.transform);
                }
            }
            else
            {
                Wander();
            }
            // Player is outside detection range, perform other behavior or idle
        }
    }


   void Shoot(Transform targetTransform)
{
    if (Time.time >= nextFireTime)
    {
        // Cast a ray from the gun tip towards the player
        RaycastHit hit;
        if (Physics.Raycast(gunTip.position, (targetTransform.position - gunTip.position).normalized, out hit))
        {
            // Check if the raycast hit the player
            if (hit.collider.CompareTag(playerTag))
            {
                // Perform actions when the raycast hits the player
                Debug.Log("Enemy hit the player!");

                // Instantiate shoot effect
                Instantiate(shootEffectPrefab, gunTip.position, Quaternion.LookRotation(hit.point - gunTip.position));

                // Play shooting sound effect
                if (shootSound != null)
                {
                    AudioSource.PlayClipAtPoint(shootSound, gunTip.position);
                }

                // Damage the player
                Health playerHealth = hit.collider.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                }
            }
        }

        // Set the next fire time based on fire rate
        nextFireTime = Time.time + 1 / fireRate;
    }
}


    void Wander()
    {
        if (!isWandering)
        {
            isWandering = true;
            wanderTimer = 0.0f;
            SetNewWanderTarget();
        }

        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderInterval)
        {
            wanderTimer = 0.0f;
            SetNewWanderTarget();
        }

        // Move towards the wander target if not close enough
        Vector3 directionToWanderTarget = wanderTarget - transform.position;
        float distanceToWanderTarget = directionToWanderTarget.magnitude;
        if (distanceToWanderTarget > 0.1f) // Adjust this threshold as needed
        {
            transform.Translate(directionToWanderTarget.normalized * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void SetNewWanderTarget()
    {
        wanderTarget = transform.position + Random.insideUnitSphere * wanderRadius;
        wanderTarget.y = transform.position.y; // Keep the same height as the enemy
    }

}
