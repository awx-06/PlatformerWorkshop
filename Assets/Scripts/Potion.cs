using UnityEngine;

// Goes on a potion prefab. Pick the type in the Inspector.
public class Potion : MonoBehaviour
{
    public enum PotionType { Healing, Damage }   // shows up as a dropdown

    public PotionType type = PotionType.Healing;
    public Vector2 throwVelocity = new Vector2(8f, 6f);   // sideways, upward

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Colour the sprite by type so you can tell them apart
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        //if (sr != null)
        //{
        //    switch (type)
        //    {
        //        case PotionType.Healing: sr.color = Color.green; break;
        //        case PotionType.Damage: sr.color = Color.red; break;
        //    }
        //}
    }

    // Called by the thrower right after the potion is spawned
    public void Launch(float dir, float inheritedSpeedX, Collider2D ignore)
    {
        // Don't collide with the player who threw it
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ignore);

        rb.linearVelocity = new Vector2(dir * throwVelocity.x + inheritedSpeedX, throwVelocity.y);
        rb.angularVelocity = -dir * 360f;   // spin
    }
}