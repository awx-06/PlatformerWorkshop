using UnityEngine;

// Spikes hurt the player every time they touch them, and (unlike a pickup) they stay in the level.
// Needs a Collider 2D with Is Trigger ticked. The player needs the "Player" tag and a Health component.
public class Spikes : MonoBehaviour
{
    public int damage = 1;
    public float bounce = 8f;          // throws the player up off the spikes
    public float hitCooldown = 0.5f;   // seconds between hits if the player stays on them

    float nextHitTime;

    void OnTriggerEnter2D(Collider2D other)
    {
        TryHurt(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        TryHurt(other);
    }

    void TryHurt(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;     // only the player gets hurt
        if (Time.time < nextHitTime) return;          // still in the cooldown
        nextHitTime = Time.time + hitCooldown;

        // Bounce first: if this hit kills the player, Health resets the speed when it respawns them
        Rigidbody2D body = other.GetComponent<Rigidbody2D>();
        body.linearVelocity = new Vector2(body.linearVelocity.x, bounce);

        other.GetComponent<Health>().TakeDamage(damage);
    }
}