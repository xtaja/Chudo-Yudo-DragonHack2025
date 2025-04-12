using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterControllerWithState : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float runSpeed = 8f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float rotationSpeed = 10f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private bool isGrounded;

    public CharacterState currentState = CharacterState.Idle;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        ApplyGravity();
        UpdateAnimator();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f; // Stick to the ground
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 inputDirection = cameraTransform.right * moveX + cameraTransform.forward * moveZ;
        inputDirection.y = 0;
        inputDirection.Normalize();

        bool isMoving = inputDirection.magnitude > 0.1f;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        float finalSpeed = isRunning ? runSpeed : speed;

        // Move and rotate
        if (isMoving)
        {
            controller.Move(inputDirection * finalSpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Set state based on movement
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            currentState = CharacterState.Jumping;
        }
        else if (isRunning)
        {
            currentState = CharacterState.Running;
        }
        else if (isMoving)
        {
            currentState = CharacterState.Walking;
        }
        else
        {
            currentState = CharacterState.Idle;
        }

        // Apply movement (horizontal only)
        Vector3 horizontalMove = inputDirection * finalSpeed;
        Vector3 finalMove = horizontalMove + velocity;
        controller.Move(finalMove * Time.deltaTime);
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    void UpdateAnimator()
    {
        animator.SetInteger("State", (int)currentState);
    }
}
