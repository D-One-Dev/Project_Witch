using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerBalanceUI : MonoBehaviour
{
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private float animationSmoothness;
    private int targetBalance;
    private float temp;

    public static PlayerBalanceUI instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(IndependentUpdate());
    }

    public void UpdateBalance(int newBalance)
    {
        if(!float.TryParse(balanceText.text, out temp)) temp = newBalance;
        targetBalance = newBalance;
    }

    private IEnumerator IndependentUpdate()
    {
        temp = Mathf.Lerp(temp, targetBalance, animationSmoothness);
        if (targetBalance + 1 > temp) balanceText.text = targetBalance.ToString();
        else balanceText.text = Mathf.RoundToInt(temp).ToString();
        yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        StartCoroutine(IndependentUpdate());
    }
}