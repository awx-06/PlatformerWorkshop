using UnityEngine;

// Parent class for every enemy: walks back and forth, hurts the player on touch,
// and takes damage when the player lands on its head.
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int health = 2;
    public int damage = 1;
    public float speed = 2f;

    [Header("Patrol")]
    public float patrolDistance = 3f;       // how far it walks either side of its start

    [Header("Walls")]
    public LayerMask groundLayer;
    public float wallCheckDistance = 0.1f;  // how far to check for walls

    protected Rigidbody2D rb;
    protected SpriteRenderer sprite;
    protected Collider2D body;
    protected int direction = -1;           // -1 = left, 1 = right
    Vector2 startPosition;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        body = GetComponent<Collider2D>();
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        Move();
    }

    // Walk, and turn around at the edge of the patrol area
    protected virtual void Move()
    {
        float distanceFromStart = transform.position.x - startPosition.x;
        if (distanceFromStart > patrolDistance) 
            direction = -1;
        else if (distanceFromStart < -patrolDistance) 
            direction = 1;

        if (WallAhead()) 
            direction = -direction;

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        sprite.flipX = direction < 0;       // Pixel Adventure characters face right
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) 
            return;

        Rigidbody2D playerBody = collision.rigidbody;
        bool landedOnHead = collision.collider.bounds.min.y > body.bounds.center.y;

        if (landedOnHead)
        {
            TakeDamage(1);
            playerBody.linearVelocity = new Vector2(playerBody.linearVelocity.x, 10f);   // bounce
        }
        else
        {
            HitPlayer(collision.gameObject);
            playerBody.linearVelocity = new Vector2(playerBody.linearVelocity.x, 6f);    // knock up
        }
    }

    bool WallAhead()
    {
        Bounds b = body.bounds;
        Vector2 origin = new Vector2(direction > 0 ? b.max.x : b.min.x, b.center.y);
        Vector2 forward = new Vector2(direction, 0f);

        Debug.DrawRay(origin, forward * wallCheckDistance, Color.red);
        return Physics2D.Raycast(origin, forward, wallCheckDistance, groundLayer);
    }

    // What happens when we touch the player. Children can change this.
    protected virtual void HitPlayer(GameObject player)
    {
        player.GetComponent<Health>().TakeDamage(damage);
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) 
            Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}