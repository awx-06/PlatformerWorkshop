using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Attack damages every enemy inside a circle around the attack point, once per swing.
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    public int attackDamage = 1;
    public float attackRadius = 1.2f;       // how far the attack reaches
    public float attackCooldown = 0.4f;     // time between attacks
    public LayerMask enemyLayer;            // the layer your enemies are on

    [Header("Attack Point")]
    public Transform attackPoint;           // empty child object (optional, falls back to the player)
    public bool mirrorWithSprite = true;    // place the point on the right, it flips when the player faces left

    SpriteRenderer sprite;
    InputAction attackAction;
    float nextAttackTime;

    void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();

        // The Attack action lives in the InputSystem_Actions asset (Player map)
        attackAction = InputSystem.actions.FindAction("Attack");
    }

    void Update()
    {
        if (attackAction.WasPressedThisFrame() && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            DealDamage();
        }
    }

    // Where the attack circle is centred
    Vector2 AttackCenter()
    {
        if (attackPoint == null) return transform.position;

        Vector2 center = attackPoint.position;

        // A child object doesn't flip with flipX, so mirror it ourselves
        if (mirrorWithSprite && sprite != null && sprite.flipX)
        {
            float offsetX = attackPoint.position.x - transform.position.x;
            center.x = transform.position.x - offsetX;
        }

        return center;
    }

    void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(AttackCenter(), attackRadius, enemyLayer);

        // Remember who we already hit this swing, so each enemy takes damage only once
        HashSet<Enemy> alreadyHit = new HashSet<Enemy>();

        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponentInParent<Enemy>();
            if (enemy == null || !alreadyHit.Add(enemy)) continue;   // Add returns false if already in the set

            enemy.TakeDamage(attackDamage);
        }
    }

    // Shows the attack range in the Scene view when the Player is selected
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackCenter(), attackRadius);
    }
}