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
}