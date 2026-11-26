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
    Animator anim;
    InputAction moveAction;
    InputAction jumpAction;
    float moveInput;
    bool jumpRequested;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>().x;   // -1 = left, 0 = nothing, 1 = right

        if (moveInput > 0) sprite.flipX = false;
        else if (moveInput < 0) sprite.flipX = true;

        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        // TODO (exercise part 1): if Jump was pressed this frame AND we are on the ground,
        // remember it: jumpRequested = true;
    }

    void FixedUpdate()
    {

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // TODO (exercise part 2): if jumpRequested, set the upward speed to jumpForce
        // (keep the sideways speed), then set jumpRequested back to false.
    }

    bool IsGrounded()
    {
        // Is a small circle at our feet touching anything on the Ground layer?
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

}