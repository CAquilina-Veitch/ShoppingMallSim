using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customers : MonoBehaviour
{
    [SerializeField] List<Business> activeBusinesses;
    List<CustomerNPC> walkers = new List<CustomerNPC>();

    [SerializeField] GameObject customerPrefab;

    [SerializeField] Wallet w;
    public void ChangeBusinessActivity(Business b, bool to)
    {
        if (!activeBusinesses.Contains(b) && to)
        {
            activeBusinesses.Add(b);
        }
        else if (activeBusinesses.Contains(b)&&!to)
        {
            activeBusinesses.Remove(b);
        }
        else
        {
            Debug.LogError($"!!!!!! {b} ,{activeBusinesses.Contains(b)} - exists , {to}");
        }
    }

    public void StartNewCustomer()
    {
        if (activeBusinesses.Count == 0)
        {
            return;
        }
        Business target = activeBusinesses[Random.Range(0, activeBusinesses.Count)];
        GameObject temp = Instantiate(customerPrefab, transform);
        Debug.Log(temp);
        Debug.Log(walkers);
        Debug.Log(temp.GetComponent<CustomerNPC>());
        walkers.Add(temp.GetComponent<CustomerNPC>());
        temp.GetComponent<CustomerNPC>().init(target);
        temp.GetComponent<CustomerNPC>().cM = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartNewCustomer();
        }
    }
    public void SaleDay()
    {
        Debug.LogWarning("SaleDay");
    }
    public void makeSale(Business b)
    {
        Debug.LogWarning($"Sale made {w} {w.Currency}, adding {b.stockDetails.value}");
        w.Currency += b.stockDetails.value;
        Debug.LogWarning($"{w} {w.Currency}");
        b.stockDetails.amount--;
    }

}
