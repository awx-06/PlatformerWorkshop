using UnityEngine;

// Example pickup: gives back health.
public class HealPickup : Pickup
{
    public int healAmount = 1;

    public override bool CanPickUp(GameObject player)
    {
        Health health = player.GetComponent<Health>();
        return health.currentHealth < health.maxHealth;
    }

    public override void Apply(GameObject player)
    {
        player.GetComponent<Health>().Heal(healAmount);
    }
}