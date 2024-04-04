using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityStandardAssets.CrossPlatformInput;
using TMPro;

public class BombThrower : MonoBehaviourPun
{
    public GameObject bombPrefab; 
    public Transform throwPoint; 
    public float throwForce = 10f; 
    public GameObject explosionPrefab; 
    public float explosionDuration = 2f; 
    public float explosionRadius = 5f; 
    public int damageAmount = 50; 
    public AudioClip explosionSound; 

    private AudioSource audioSource; 
    public int currentGrenadeAmount = 5; 
    public TextMeshProUGUI grenadeText; 

    public void ThrowBomb()
    {
        if (!photonView.IsMine || currentGrenadeAmount <= 0)
            return;

        photonView.RPC("InstantiateBomb", RpcTarget.AllViaServer);

        currentGrenadeAmount--;
        UpdateGrenadeText();
    }

    [PunRPC]
    void InstantiateBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
        }
        StartCoroutine(ExplodeAfterDelay(bomb));
    }

    IEnumerator ExplodeAfterDelay(GameObject bomb)
    {
        yield return new WaitForSeconds(explosionDuration);

        Collider[] colliders = Physics.OverlapSphere(bomb.transform.position, explosionRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player") || col.CompareTag("Enemy"))
            {
                photonView.RPC("ApplyDamage", RpcTarget.AllViaServer, col.gameObject.GetPhotonView().ViewID);
            }
        }

        GameObject explosion = Instantiate(explosionPrefab, bomb.transform.position, Quaternion.identity);
        if (explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }
        Destroy(bomb);
        Destroy(explosion, 2f);
    }

    [PunRPC]
    void ApplyDamage(int targetID)
    {
        GameObject target = PhotonView.Find(targetID).gameObject;
        if (target.CompareTag("Player"))
        {
            Health health = target.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
        }
        else if (target.CompareTag("Enemy"))
        {
            EnemyHealth health = target.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
        }
    }

    void UpdateGrenadeText()
    {
        if (grenadeText != null)
        {
            grenadeText.text = "Grenades: " + currentGrenadeAmount;
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
