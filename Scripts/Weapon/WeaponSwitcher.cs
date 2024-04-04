using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class WeaponSwitcher : MonoBehaviourPunCallbacks
{
    private int selectedWeapon = 0;

    public Animation anim;
    public AnimationClip draw;
    private Weapon[] weapons;

    void Start()
    {
        weapons = GetComponentsInChildren<Weapon>();
        SelectWeapon();
    }

    void Update()
    {
         if (photonView.IsMine)
        {
            int previousSelectedWeapon = selectedWeapon;

            if (previousSelectedWeapon != selectedWeapon)
            {
                photonView.RPC("RPCSelectWeapon", RpcTarget.All, selectedWeapon);
            }
        }
    }

     [PunRPC]
    void RPCSelectWeapon(int weaponIndex)
    {
        selectedWeapon = weaponIndex;
        SelectWeapon();
    }

    public void OnGun1ButtonClicked()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPCSelectWeapon", RpcTarget.All, 0);
        }
    }

    public void OnGun2ButtonClicked()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPCSelectWeapon", RpcTarget.All, 1);
        }
    }

    public void OnGun3ButtonClicked()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPCSelectWeapon", RpcTarget.All, 2);
        }
    }
    // Add similar methods for other buttons (Gun3, Gun4, ..., Gun9)

    void SelectWeapon()
    {
        Debug.Log(weapons.Length);
        anim.Stop();
        anim.Play(draw.name);

        for (int i = 0; i < weapons.Length; i++)
        {
            if (i == selectedWeapon)
            {
                weapons[i].gameObject.SetActive(true);
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
            Debug.Log("Selected Weapon: " + selectedWeapon);
        }
    }

    public void OnReloadButtonClicked()
    {
        if(photonView.IsMine)
            ReloadCurrentGun();
    }

    void ReloadCurrentGun()
    {
        // Check if selectedWeapon index is valid
        if (selectedWeapon >= 0 && selectedWeapon < weapons.Length)
        {
            weapons[selectedWeapon].Reload();
        }
        else
        {
            Debug.LogError("Invalid selectedWeapon index");
        }
    }
}
