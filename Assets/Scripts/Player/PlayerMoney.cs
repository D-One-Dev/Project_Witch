using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public int Balance;

    public static PlayerMoney Instance;

    private void Awake()
    {
        Instance = this;
    }

    private bool CheckPurchaseAbility(int price)
    {
        if(Balance - price >= 0) return true;
        return false;
    }

    public void ChangeBalance(int income)
    {
        Balance += income;
        PlayerBalanceUI.instance.UpdateBalance(Balance);
    }

    private void Start()
    {
        Balance = 0;
        PlayerBalanceUI.instance.UpdateBalance(Balance);
    }
}
