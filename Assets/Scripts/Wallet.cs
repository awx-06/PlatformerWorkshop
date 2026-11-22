using UnityEngine;

// The player's money.
public class Wallet : MonoBehaviour
{
    public int money = 0;

    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log("Money: " + money);
    }
}