using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;
using UnityStandardAssets.CrossPlatformInput;

public class Weapon : MonoBehaviour
{
    public GameObject hitVFX;
    public GameObject MuzzleVFX;
    public int damage;

    public float fireRate;

    private float nextFire;
    public Camera mainCamera;
    public Transform gunTip;

    [Header("Ammo")]
    public int mag = 5;
    public int ammo = 30;
    public int magAmmo = 30;

    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    [Header("Animation")]
    public Animation anim;
    public AnimationClip reload;

    [Header("Recoil Setting")]
    // [Range(0,1)]
    // public float recoilPercent = 0.3f;

    [Range(0,2)]
    public float recoverPercent = 0.7f;
    [Space]
    public float recoilUp = 1f;
    public float recoilBack = 0f;



    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;

    private bool recoiling;
    private bool recovering;

    private float recoilLength;
    private float recoverLength;
    private PhotonView view;

    public AudioClip gunSound;
    public AudioClip reloadSound;
    public AudioClip noAmmoSound;
    private AudioSource audioSource;

    void Start()
    {
        view = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();

         magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;

        originalPosition = transform.localPosition;

        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;

    }
    // Update is called once per frame
    void Update()
    {
        if(view.IsMine)
        {
            if(nextFire > 0 )
            {
                nextFire -= Time.deltaTime;
            }
            if(CrossPlatformInputManager.GetButton("Shoot") && nextFire <= 0 && ammo > 0 && anim.isPlaying == false)
            {
                nextFire = 1/fireRate;

                ammo--;

                magText.text = mag.ToString();
                ammoText.text = ammo + "/" + magAmmo;

                Fire();
            }
            if(CrossPlatformInputManager.GetButton("Shoot") && nextFire <= 0 && ammo <= 0){
                
             audioSource.PlayOneShot(noAmmoSound);
            }

            if(Input.GetKeyDown(KeyCode.R) && mag > 0)
            {
                Reload();
            }

            if(recoiling)
            {
                Recoil();
            }
            if(recovering)
            {
                Recovering();
            }
        }
       
    }
   
    public void reloadButton()
    {
        if(mag > 0 && view.IsMine)
            Reload();
    }
    

    public void Reload()
    {
        Debug.Log("reload button clicked");
        if(view.IsMine){
            Debug.Log("view is still mine");
             anim.Play(reload.name);
             audioSource.PlayOneShot(reloadSound);
            if(mag > 0)
            {
                mag--;

                ammo = magAmmo;
            }

            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo;
        }
       
    }
    void Fire()
    {
        if(view.IsMine)
        {
            recoiling = true;
            recovering = false;
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

            RaycastHit hit;
            PhotonNetwork.LocalPlayer.AddScore(1);
            PhotonNetwork.Instantiate(MuzzleVFX.name, gunTip.position, gunTip.rotation);
            audioSource.PlayOneShot(gunSound);

            view.RPC("PlayGunSoundRPC", RpcTarget.All);

            
            
            if(Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
            {
                if(hit.transform.gameObject.GetComponent<Health>())
                {
                    PhotonNetwork.LocalPlayer.AddScore(damage);
                    if (damage >= hit.transform.gameObject.GetComponent<Health>().health)
                    {

                        RoomManager.instance.kills++;
                        RoomManager.instance.SetHashes();

                        PhotonNetwork.LocalPlayer.AddScore(100);
                    }

                    hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
                }

                PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.LookRotation(hit.normal));

                if ( hit.transform.gameObject.GetComponent<CubeController>())
                {
                    CubeController cube = hit.transform.GetComponent<CubeController>();
                    if (cube != null)
                    {
                        cube.TakeDamage(damage);
                    }
                }

                if ( hit.transform.gameObject.GetComponent<EnemyHealth>())
                {
                    EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        // Deal damage to the enemy
                        enemyHealth.TakeDamage(damage);
                    }
                }
            }
        }
        
    }
[PunRPC]
private void PlayGunSoundRPC()
{
    AudioSource.PlayClipAtPoint(gunSound, transform.position);
}
    void Recoil()
    {
        if(view.IsMine)
        {
            Vector3 finalPosition = new Vector3(originalPosition.x, originalPosition.y + recoilUp, originalPosition.z - recoilBack);

            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);

            if(transform.localPosition == finalPosition)
            {
                recoiling = false;
                recovering = true;
            }
        }
       
    }

    void Recovering()
    {
        if(view.IsMine)
        {
            Vector3 finalPosition = originalPosition;

            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);

            if(transform.localPosition == finalPosition)
            {
                recoiling = false;
                recovering = false;
            }
        }
       
    }
}
