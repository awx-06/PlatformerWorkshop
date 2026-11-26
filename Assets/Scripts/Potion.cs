using UnityEngine;

// Goes on a potion prefab. Pick the type in the Inspector.
public class Potion : MonoBehaviour
{
    public enum PotionType { Healing, Damage }   // shows up as a dropdown

    public PotionType type = PotionType.Healing;
    public Vector2 throwVelocity = new Vector2(8f, 6f);   // sideways, upward
    public int damage = 1;                                // only used by Damage potions

    Rigidbody2D rb;
    bool used;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Colour the sprite by type so you can tell them apart
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            //switch (type)
            //{
            //    case PotionType.Healing: sr.color = Color.green; break;
            //    case PotionType.Damage: sr.color = Color.red; break;
            //}
        }
    }

    // Called by the thrower right after the potion is spawned
    public void Launch(float dir, float inheritedSpeedX, Collider2D ignore)
    {
        // Don't collide with the player who threw it
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ignore);

        rb.linearVelocity = new Vector2(dir * throwVelocity.x + inheritedSpeedX, throwVelocity.y);
        rb.angularVelocity = -dir * 360f;   // spin
    }

    // Runs the moment the potion touches something
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (type != PotionType.Damage || used) return;

        Enemy enemy = collision.collider.GetComponentInParent<Enemy>();
        if (enemy == null) return;          // hit the ground or a wall: just stay there

        used = true;                        // makes sure we only hit once
        enemy.TakeDamage(damage);
        Destroy(gameObject);                // the potion breaks on the enemy
    }
}