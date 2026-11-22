using UnityEngine;
using UnityEngine.InputSystem;

// Moves the player left/right and (after the exercise) makes it jump.
public class PlayerMovement : MonoBehaviour
{
    [Header("Running")]
    public float moveSpeed = 6f;            // tiles per second

    [Header("Jumping")]
    public float jumpForce = 12f;           // upward speed when jumping
    public Transform groundCheck;           // empty object at the player's feet
    public LayerMask groundLayer;           // what counts as ground

    Rigidbody2D rb;
    SpriteRenderer sprite;
    InputAction moveAction;
    InputAction jumpAction;
    float moveInput;
    bool jumpRequested;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        // The key binds live in the InputSystem_Actions asset (Player map)
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Every frame: read the keyboard
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>().x;   // -1 = left, 0 = nothing, 1 = right

        // Face the way we're moving
        if (moveInput > 0) sprite.flipX = false;
        else if (moveInput < 0) sprite.flipX = true;

        // TODO (exercise part 1): if Jump was pressed this frame AND we are on the ground,
        // remember it: jumpRequested = true;
    }

    // Every physics step: move the body
    void FixedUpdate()
    {
        // Sideways speed from the input, keep the current up/down speed (gravity)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // TODO (exercise part 2): if jumpRequested, set the upward speed to jumpForce
        // (keep the sideways speed), then set jumpRequested back to false.
    }

    bool IsGrounded()
    {
        // Is a small circle at our feet touching anything on the Ground layer?
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Draws the ground check circle in the Scene view when the Player is selected
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null) Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }
}