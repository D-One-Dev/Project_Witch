using UnityEngine;
using Zenject;

public class EnemyMoneyCost : MonoBehaviour
{
    [SerializeField] private int moneyCost;

    private PlayerMoney _playerMoney;

    [Inject]
    public void Construct(PlayerMoney playerMoney)
    {
        _playerMoney = playerMoney;
    }

    public void DropMoney()
    {
        _playerMoney.ChangeBalance(moneyCost);
    }

    public void SetMoneyCost(int newMoneyCost)
    {
        if (newMoneyCost >= 0) moneyCost = newMoneyCost;
    }
}