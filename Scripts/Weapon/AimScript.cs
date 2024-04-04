using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AimScript : MonoBehaviour
{
    public Transform playerBody;
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float aimFOV = 30f;
    public float aimSpeed = 10f;

    private bool isAiming = false;

    void Update()
    {
        HandleAimingInput();

        // Adjust camera FOV and rotation based on whether aiming or not
        if (isAiming)
        {
            Aim();
        }
        else
        {
            StopAiming();
        }
    }

    void HandleAimingInput()
    {
        // Assuming you have an input button named "Aim" (you can customize this)
        if (CrossPlatformInputManager.GetButtonDown("Aim"))
        {
            isAiming = true;
        }
        else if (CrossPlatformInputManager.GetButtonUp("Aim"))
        {
            isAiming = false;
        }
    }

    void Aim()
    {
        // Adjust FOV towards aimFOV
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, aimFOV, Time.deltaTime * aimSpeed);

        // Rotate player body based on camera rotation
        float mouseX = Input.GetAxis("Mouse X") * aimSpeed * Time.deltaTime;
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void StopAiming()
    {
        // Reset FOV to normalFOV
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, Time.deltaTime * aimSpeed);
    }
}
