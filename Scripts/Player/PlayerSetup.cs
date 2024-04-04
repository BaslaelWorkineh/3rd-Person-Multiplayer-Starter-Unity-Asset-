using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    public PlayerController movement;
    public GameObject mainCamera;
    public GameObject MiniMapCamera;
    public GameObject controllerCanvas;
    public string nickname;
    public TextMeshPro nicknameText;

    public void IsLocalPlayer()
    {
        movement.enabled = true;
        mainCamera.SetActive(true);
        MiniMapCamera.SetActive(true);
        controllerCanvas.SetActive(true);
    }

    [PunRPC]
    public void SetNickname(string _name)
    {
        nickname = _name;

        nicknameText.text = nickname;
    }
}
