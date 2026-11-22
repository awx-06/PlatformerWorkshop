using UnityEngine;

// The player's XP.
public class Experience : MonoBehaviour
{
    public int xp = 0;

    public void AddXP(int amount)
    {
        xp += amount;
        Debug.Log("XP: " + xp);
    }
}