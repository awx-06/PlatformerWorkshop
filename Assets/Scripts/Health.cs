using UnityEngine;

// The player's hearts. Pickups heal it, enemies damage it.
public class Health : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth = 5;
    public float fallLimit = -10f;          // fall below this and you respawn

    Vector3 startPosition;
    Rigidbody2D rb;

    void Awake()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.y < fallLimit) Die();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Health: " + currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Health: " + currentHealth);
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        // Back to the start with full health
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
        currentHealth = maxHealth;
    }
}