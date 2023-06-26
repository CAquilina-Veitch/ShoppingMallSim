using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using TMPro;

public class Wallet : MonoBehaviour
{
    public TextMeshProUGUI uiCurrency, uiPremium;



    private int standardCurrency, premiumCurrency;
    public int Currency
    {
        get
        {
            return standardCurrency;
        }
        set
        {
            standardCurrency = value;
        }
    }
    public int Premium
    {
        get
        {
            return premiumCurrency;
        }
        set
        {
            premiumCurrency = value;
        }
    }
    public bool hasCurrency(bool isPremium,int amount)
    {
        if (isPremium)
        {
            return premiumCurrency >= amount;
        }
        else
        {
            return standardCurrency >= amount;
        }
    }
    public bool trySpend(bool isPremium, int amount)
    {
        if (!hasCurrency(isPremium, amount))
        {
            return false;
        }
        else
        {
            if (isPremium)
            {
                premiumCurrency -= amount;
            }
            else
            {
                standardCurrency -= amount;
            }
            return true;
        }
    }
    private void FixedUpdate()
    {
        uiCurrency.text = $"{Currency}";
        uiPremium.text = $"{premiumCurrency}";
    }
}
