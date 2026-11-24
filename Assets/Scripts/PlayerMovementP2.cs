using UnityEngine;
using UnityEngine.InputSystem;

// Moves the player left/right, makes it jump, and drives the animations.
public class PlayerMovementP2 : MonoBehaviour
{
    [Header("Running")]
    public float moveSpeed = 6f;            // tiles per second

    [Header("Jumping")]
    public float jumpForce = 12f;           // upward speed when jumping
    public Transform groundCheck;           // empty object at the player's feet
    public LayerMask groundLayer;           // what counts as ground

    Rigidbody2D rb;
    SpriteRenderer sprite;
    Animator anim;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    float moveInput;
    bool jumpRequested;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();

        // The key binds live in the InputSystem_Actions asset (Player map)
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        attackAction = InputSystem.actions.FindAction("Attack");
    }

    // Every frame: read the keyboard
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>().x;   // -1 = left, 0 = nothing, 1 = right

        // Face the way we're moving
        if (moveInput > 0) sprite.flipX = false;
        else if (moveInput < 0) sprite.flipX = true;

        // Remember the jump press so FixedUpdate can use it
        if (jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            jumpRequested = true;
            anim.SetTrigger("Jump");
        }

        // Attack
        if (attackAction.WasPressedThisFrame())
        {
            anim.SetTrigger("Attack");
        }

        // Feed the Animator
        anim.SetFloat("Speed", Mathf.Abs(moveInput));
        anim.SetFloat("YVelocity", rb.linearVelocity.y);
        anim.SetBool("IsGrounded", IsGrounded());
    }

    // Every physics step: move the body
    void FixedUpdate()
    {
        // Sideways speed from the input, keep the current up/down speed (gravity)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Jump: set the upward speed, keep the sideways speed
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
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