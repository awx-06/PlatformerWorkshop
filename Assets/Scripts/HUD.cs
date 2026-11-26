using UnityEngine;
using TMPro;

// Shows the player's health, money and XP on the screen.
// Put it on the Canvas, then drag the Player and the three texts into its slots.
public class HUD : MonoBehaviour
{
    [Header("Player (drag the Player here)")]
    public Health health;
    public Wallet wallet;
    public Experience experience;      // optional: leave empty if you have no XP

    [Header("Texts (drag the TextMeshPro objects here)")]
    public TMP_Text healthText;
    public TMP_Text moneyText;
    public TMP_Text xpText;            // optional

    // Every frame: copy the numbers into the texts
    void Update()
    {
        healthText.text = "Health: " + health.currentHealth + " / " + health.maxHealth;
        moneyText.text = "Money: " + wallet.money;

        if (experience != null && xpText != null)
            xpText.text = "XP: " + experience.xp;
    }
}