using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

[Serializable]
public struct stockInfo
{
    public stockType type;
    public int maxStock;
    public int amount;
    public int value;
}
public class shopUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI quantity;
    [SerializeField] Sprite[] icons;

    Business b;
    
    public void init(Business _b)
    {
        b = _b;
        icon.sprite = icons[(int)b.stockDetails.type];
        updateVisual();
    }

    public void updateVisual()
    {
        stockInfo stock = b.stockDetails;
        title.text = $"{Enum.GetName(typeof(stockType), stock.type)}";
        quantity.text = stock.amount.ToString();
        icon.sprite = icons[(int)stock.type];

    }


}
