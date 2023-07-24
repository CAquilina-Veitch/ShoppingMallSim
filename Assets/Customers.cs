using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customers : MonoBehaviour
{
    List<Business> activeBusinesses;
    List<CustomerNPC> walkers;

    [SerializeField] GameObject customerPrefab;

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

    public void StartCustomer()
    {
        Business target = activeBusinesses[Random.Range(0, activeBusinesses.Count)];
        GameObject temp = Instantiate(customerPrefab, transform);
        walkers.Add(temp.GetComponent<CustomerNPC>());
    }




}
