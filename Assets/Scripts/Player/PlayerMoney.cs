public class PlayerMoney
{
    public int Balance;

    public bool CheckPurchaseAbility(int price)
    {
        if(Balance - price >= 0) return true;
        return false;
    }

    public void ChangeBalance(int income)
    {
        Balance += income;
        PlayerBalanceUI.instance.UpdateBalance(Balance);
    }

    public void SetBalance(int value)
    {
        Balance = value;
        PlayerBalanceUI.instance.UpdateBalance(Balance);
    }
}
