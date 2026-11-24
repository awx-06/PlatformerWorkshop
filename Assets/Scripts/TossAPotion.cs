using UnityEngine;
using UnityEngine.InputSystem;

// G throws the selected potion in the direction the sprite is facing. R switches potion.
public class TossAPotion : MonoBehaviour
{
    public GameObject[] potionPrefabs;                     // e.g. PotionHealing, PotionDamage
    public Vector2 throwOffset = new Vector2(0.6f, 0.5f);  // spawn point relative to the player
    public float throwCooldown = 0.6f;

    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer sprite;
    InputAction throwAction;
    InputAction switchAction;
    int selected;
    float nextThrowTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        throwAction = new InputAction("ThrowPotion", InputActionType.Button, "<Keyboard>/g");
        switchAction = new InputAction("SwitchPotion", InputActionType.Button, "<Keyboard>/r");
    }

    void OnEnable()
    {
        throwAction.Enable();
        switchAction.Enable();
    }

    void OnDisable()
    {
        throwAction.Disable();
        switchAction.Disable();
    }

    void Update()
    {
        if (potionPrefabs.Length == 0) return;

        if (switchAction.WasPressedThisFrame())
        {
            selected = (selected + 1) % potionPrefabs.Length;
            Debug.Log("Selected potion: " + potionPrefabs[selected].name);
        }

        if (throwAction.WasPressedThisFrame() && Time.time >= nextThrowTime)
        {
            Throw();
        }
    }

    void Throw()
    {
        nextThrowTime = Time.time + throwCooldown;

        float dir = sprite.flipX ? -1f : 1f;   // the way the sprite is facing
        Vector2 spawn = (Vector2)transform.position + new Vector2(dir * throwOffset.x, throwOffset.y);

        GameObject obj = Instantiate(potionPrefabs[selected], spawn, Quaternion.identity);
        obj.GetComponent<Potion>().Launch(dir, rb.linearVelocity.x, col);
    }
}