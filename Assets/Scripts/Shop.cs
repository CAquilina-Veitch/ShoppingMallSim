using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] Wallet wallet;
    [SerializeField] int purchasedCurrency;
    [SerializeField] int purchasedPremium;


    public void AddRegular()
    {
        wallet.Currency += purchasedCurrency;
    }

    public void AddSpecial()
    {
        wallet.Premium += purchasedPremium;
    }

}
