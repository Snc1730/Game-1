using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public Vector3 offset = new Vector3(0f, 3f, -10f); // Offset from the player's position
    public float rotationSpeed = 5f; // Speed of camera rotation
    public float verticalRotationLimit = 80f; // Maximum vertical angle of the camera

    private float cameraRotationX = 0f; // Current rotation around the x-axis of the camera

    void LateUpdate()
    {
        // Camera rotation based on mouse or input
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Update camera rotation around the y-axis
        transform.RotateAround(player.position, Vector3.up, mouseX);

        // Update camera rotation around the x-axis with clamping
        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -verticalRotationLimit, verticalRotationLimit);

        // Apply camera rotation around the x-axis
        transform.localRotation = Quaternion.Euler(cameraRotationX, transform.localEulerAngles.y, 0f);

        // Calculate target position for the camera
        Vector3 targetPosition = player.position + transform.forward * offset.z; // Use + to move behind the player
        targetPosition += player.up * offset.y; // Adjust for vertical offset
        targetPosition += player.right * offset.x; // Adjust for horizontal offset

        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * rotationSpeed);

        // Ensure the camera looks at the player
        transform.LookAt(player.position + Vector3.up * offset.y);

        // Update player rotation to face camera direction
        player.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }
}
