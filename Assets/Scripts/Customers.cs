using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Customers : MonoBehaviour
{
    [SerializeField] List<Business> activeBusinesses;
    List<CustomerNPC> walkers = new List<CustomerNPC>();

    [SerializeField] GameObject customerPrefab;

    [SerializeField] Wallet w;

    [SerializeField] GameObject moneyEarntPrefab;

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
    public void StartNewCustomer(Business b)
    {
        Business target = b;
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
        StartCoroutine(saleDayLoop());
    }
    public void makeSale(Business b)
    {
        Debug.LogWarning($"Sale made {w} {w.Currency}, adding {b.stockDetails.value}");
        w.Currency += 1;
        Debug.LogWarning($"{w} {w.Currency}");
        b.stockDetails.amount--;

        GameObject temp = Instantiate(moneyEarntPrefab,transform);
        temp.GetComponent<MoneyEarnt>().StartMoving(b);
        Debug.Log(b.stockDetails.amount);
    }

    private void OnEnable()
    {
        StartCoroutine(summonLoop());
    }
    IEnumerator summonLoop()
    {
        //Debug.Log("looped");
        float delay = 5f;
        if (activeBusinesses.Count > 0)
        {
            foreach (Business b in activeBusinesses)
            {
                //stock check here ???

                int n = Random.Range(0, 5);
                n -= b.activeWorkers.Count;
                if (n <= 0)
                {
                    float r = Random.Range(0, delay / 2);
                    delay -= r;
                    yield return new WaitForSeconds(r);
                    StartNewCustomer(b);
                    
                }
            }
        }
        yield return new WaitForSeconds(delay);
        //Debug.Log($"waited {delay}");
        StartCoroutine(summonLoop());
    }
    IEnumerator saleDayLoop()
    {
        for (int i = 0; i < 30; i++)
        {
            StartNewCustomer();
            yield return new WaitForSeconds(0.4f);
        }
    }

}
