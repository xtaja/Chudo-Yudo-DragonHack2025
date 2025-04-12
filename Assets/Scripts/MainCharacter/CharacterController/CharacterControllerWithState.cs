using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class CharacterControllerWithState : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float runSpeed = 4f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float rotationSpeed = 5f;
    public float jumpDelay = 0.5f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isInAir = false;

    private bool isJumpingQueued = false;
    private float jumpTimer = 0f;

    public CharacterState currentState = CharacterState.Idle;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false; // <<< Important!
    }

    void Update()
    {
        HandleMovement();
        HandleJumpTimer();
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

        // Start jump timer when pressed
        if (Input.GetButtonDown("Jump") && isGrounded && !isJumpingQueued && !isInAir)
        {
            isJumpingQueued = true;
            jumpTimer = 0f;
            currentState = CharacterState.Jumping; // Trigger jump animation before actual jump
        }

        // State transitions
        if (isInAir)
        {
            if (isGrounded && velocity.y <= 0)
            {
                isInAir = false;
                currentState = isMoving ? CharacterState.Walking : CharacterState.Idle;
            }
            else
            {
                currentState = CharacterState.Jumping;
            }
        }
        else if (!isJumpingQueued)
        {
            if (isRunning && isGrounded)
            {
                currentState = CharacterState.Running;
            }
            else if (isMoving && isGrounded)
            {
                currentState = CharacterState.Walking;
            }
            else if (isGrounded)
            {
                currentState = CharacterState.Idle;
            }
        }

        // Final movement apply
        Vector3 horizontalMove = inputDirection * finalSpeed;
        Vector3 finalMove = horizontalMove + velocity;
        controller.Move(finalMove * Time.deltaTime);
    }

    void HandleJumpTimer()
    {
        if (isJumpingQueued)
        {
            jumpTimer += Time.deltaTime;

            if (jumpTimer >= jumpDelay)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                isInAir = true;
                isJumpingQueued = false;
            }
        }
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
