using UnityEngine;

public class CameraLook : MonoBehaviour
{
    private float XMove;
    private float YMove;
    private float XRotation;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform gunObject;
    public Vector2 lockAxis;
    public float sensitivity = 40f;
    public Transform cameraPivot;
    public float distance = 2f;
    public float smoothSpeed = 5f;
    public float minVerticalRotation = -60f;
    public LayerMask obstacleMask; // Layer mask for obstacles

    void Update()
    {
        XMove = lockAxis.x * sensitivity * Time.deltaTime;
        YMove = lockAxis.y * sensitivity * Time.deltaTime;
        XRotation -= YMove;
        XRotation = Mathf.Clamp(XRotation, minVerticalRotation, 60f);

        // Smoothly interpolate the camera pivot's rotation
        Quaternion targetRotation = Quaternion.Euler(XRotation, 0, 0);
        cameraPivot.localRotation = Quaternion.Lerp(cameraPivot.localRotation, targetRotation, smoothSpeed * Time.deltaTime); // Changed to Quaternion.Lerp

        playerBody.Rotate(Vector3.up * XMove);

        // Calculate the desired camera position based on the camera pivot's rotation and the set distance
        Vector3 offset = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(XRotation, cameraPivot.eulerAngles.y, 0);
        Vector3 targetPosition = cameraPivot.position + rotation * offset;

        // Check for obstacles between the current camera position and the desired position
        RaycastHit hit;
        if (Physics.Linecast(cameraPivot.position, targetPosition, out hit, obstacleMask))
        {
            // Adjust the camera position to avoid the obstacle
            transform.position = hit.point;
        }
        else
        {
            // Smoothly interpolate to the desired camera position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime); // Changed to Vector3.Lerp
        }

        transform.LookAt(cameraPivot.position);

        if (gunObject != null)
        {
            gunObject.localRotation = Quaternion.Lerp(gunObject.localRotation, Quaternion.Euler(XRotation, 0, 0), smoothSpeed * Time.deltaTime); // Changed to Quaternion.Lerp
        }
    }
}
