using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 2f; // Adjust this for jump height
    public float gravity = -9.81f;
    public Transform cameraTransform; // Reference to the camera's transform
    private CharacterController characterController;
    private bool isGrounded;
    private float raycastDistance = 1.2f; // Adjust this based on your player's collider height
    private float verticalVelocity; // Vertical velocity for jumping and falling

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component not found on PlayerObj.");
        }
    }

    void Update()
    {
        // Check if characterController is null to prevent errors
        if (characterController == null)
        {
            return;
        }

        // Perform the raycast to check if the player is grounded
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, raycastDistance);

        // Player movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate the forward and right directions relative to the camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Normalize the vectors to ensure consistent speed
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Calculate the direction to move based on input and camera orientation
        Vector3 direction = forward * moveVertical + right * moveHorizontal;

        // Apply the movement to the player using CharacterController
        characterController.Move(direction * speed * Time.deltaTime);

        // Handle jumping
        HandleJump();

        // Visualize the raycast for debugging purposes
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * raycastDistance, Color.red);
    }

    void HandleJump()
    {
        // Check if grounded to allow jumping
        if (isGrounded)
        {
            // Jump input detected
            if (Input.GetButtonDown("Jump"))
            {
                // Calculate initial jump velocity to achieve desired jump height
                verticalVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            }
            else
            {
                // Reset vertical velocity when grounded and not jumping
                verticalVelocity = 0f;
            }
        }
        else
        {
            // Apply gravity to vertical velocity when not grounded
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Apply vertical velocity to move the player
        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}
