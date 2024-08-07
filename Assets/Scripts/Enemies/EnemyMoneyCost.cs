using UnityEngine;

public class EnemyMoneyCost : MonoBehaviour
{
    [SerializeField] private int moneyCost;

    public void DropMoney()
    {
        PlayerMoney.Instance.ChangeBalance(moneyCost);
    }
}
