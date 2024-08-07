using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VolFx;

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

    private void FixedUpdate()
    {
        temp = Mathf.Lerp(temp, targetBalance, animationSmoothness);
        if (targetBalance + 1 > temp) balanceText.text = targetBalance.ToString();
        else
        balanceText.text = temp.RoundToInt().ToString();
    }

    public void UpdateBalance(int newBalance)
    {
        temp = Int32.Parse(balanceText.text);
        targetBalance = newBalance;
    }
}
