using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.CrossPlatformInput;

public class GrabAndThrow : MonoBehaviourPunCallbacks
{
    public Transform handTransform;
    public float throwForce = 10f;
    private GameObject grabbedObject;
    private bool isGrabbing = false;

    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (CrossPlatformInputManager.GetButton("Grab"))
        {
            if (!isGrabbing)
            {
                photonView.RPC("TryGrabObject", RpcTarget.AllBuffered);
            }
        }

        if (CrossPlatformInputManager.GetButton("Release") && isGrabbing)
        {
            photonView.RPC("ReleaseObject", RpcTarget.AllBuffered);
        }

        if (isGrabbing)
        {
            photonView.RPC("SyncHandTransform", RpcTarget.AllBuffered, handTransform.position, handTransform.rotation);
        }
    }

    [PunRPC]
    void TryGrabObject()
    {
        Camera mainCamera = Camera.main;
        Vector3 screenCenter = new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane);
        Vector3 rayOrigin = mainCamera.ViewportToWorldPoint(screenCenter);

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, mainCamera.transform.forward, out hit, Mathf.Infinity))
        {
            grabbedObject = hit.collider.gameObject;
            if (grabbedObject.CompareTag("Grabbable"))
            {
                isGrabbing = true;
                grabbedObject.GetComponent<PhotonView>().RequestOwnership(); // Request ownership of the grabbed object
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                grabbedObject.transform.parent = handTransform;
                grabbedObject.transform.localPosition = Vector3.zero;

            }
        }
    }

    [PunRPC]
    void ReleaseObject()
    {
        isGrabbing = false;
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        grabbedObject.transform.parent = null;
        grabbedObject.GetComponent<Rigidbody>().velocity = handTransform.forward * throwForce;
        grabbedObject = null;
    }

    [PunRPC]
    void SyncHandTransform(Vector3 position, Quaternion rotation)
    {
        handTransform.position = position;
        handTransform.rotation = rotation;
    }
}
