using UnityEngine;

// Parent class for every pickup: it notices the player and disappears.
// Children only say WHAT happens, by writing Apply().
public abstract class Pickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;    // only the player picks things up

        if (CanPickUp(other.gameObject))
        {
            Apply(other.gameObject);
            Destroy(gameObject);
        }
    }

    // Children can say "not now" (e.g. a heart when health is full)
    public virtual bool CanPickUp(GameObject player)
    {
        return true;
    }

    // Every child must write this
    public abstract void Apply(GameObject player);
}