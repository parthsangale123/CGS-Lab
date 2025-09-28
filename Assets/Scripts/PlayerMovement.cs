using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;

    [SerializeField] float speed = 5f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -9.81f;

    CharacterController controller;
    Vector3 velocity;
    bool isGrounded;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    public LayerMask groundMask;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
    }

    void Update()
    {
        MovePlayer();
        ApplyGravityAndJump();
    }

    void MovePlayer()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        //Get camera direction
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        //Ignore vertical tilt
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        //Movement relative to camera
        Vector3 move = forward * input.y + right * input.x;

        //Apply movement only
        controller.Move(move * speed * Time.deltaTime);
    }

    void ApplyGravityAndJump()
    {
        //Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //small downward force to stick to ground
        }

        //Set jump velocity
        if (jumpAction.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Apply gravity
        velocity.y += gravity * Time.deltaTime;

        //Jump
        controller.Move(velocity * Time.deltaTime);
    }
}
