using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using TMPro;

public class Wallet : MonoBehaviour
{
    public TextMeshProUGUI uiCurrency, uiPremium;



    private int standardCurrency, premiumCurrency;

    public int visualCurrency, visualPremium;

    private float textTransitionTime = 0.2f;
    float s,p;

    public void Start()
    {
        Currency += 100;
    }

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
        s = Mathf.Lerp(s, standardCurrency != visualCurrency ? 1 : 0, 0.1f);
        p = Mathf.Lerp(p, standardCurrency != visualCurrency ? 1 : 0, 0.1f);

        if (standardCurrency != visualCurrency)
        {
            visualCurrency = Mathf.RoundToInt(Mathf.Lerp(visualCurrency, standardCurrency, 0.5f));
            uiCurrency.text = ""+visualCurrency;
        }
        if (premiumCurrency != visualCurrency)
        {
            visualPremium = Mathf.RoundToInt(Mathf.Lerp(visualPremium, premiumCurrency, 0.5f));
            uiPremium.text = "" + premiumCurrency;
        }
    }


    //dev
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Currency += 10000;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Currency += 3;
        }
    }

}
