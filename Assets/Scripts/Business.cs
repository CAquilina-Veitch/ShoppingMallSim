using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum stockType
{
    groceries, two,three,four,five
}
[Serializable]
public struct WorkerInfo
{
    public string name;
    public int level;
    public species specie;


}

public class Business : MonoBehaviour
{

    public stockType stock;
    public int maxStock;
    public int currentStock;

    public List<WorkerInfo> hiredWorkers = new List<WorkerInfo>();
    public List<WorkerInfo> activeWorkers = new List<WorkerInfo>();


    public bool businessActive;

    DijkstraPathManager pM;

    private void OnEnable()
    {
        pM = GameObject.FindGameObjectWithTag("PathManager").GetComponent<DijkstraPathManager>();
        pM.allBusinesses.Add(this);

    }

    private void FixedUpdate()
    {
        if (!businessActive)
        {
            if (activeWorkers.Count  > 0 && currentStock > 0)
            {
                ToggleActivity(true);
            }
        }
        else
        {
            if (activeWorkers.Count == 0 || currentStock <= 0)
            {
                ToggleActivity(false);
            }
        }
        
    }

    public void ToggleActivity()
    {
        businessActive = !businessActive;
        if (pM == null)
        {
            pM = GameObject.FindGameObjectWithTag("PathManager").GetComponent<DijkstraPathManager>();
        }
        pM.ChangeBusinessActivity(this, businessActive);
    }
    public void ToggleActivity(bool to)
    {
        businessActive = to;
        if (pM == null)
        {
            pM = GameObject.FindGameObjectWithTag("PathManager").GetComponent<DijkstraPathManager>();
        }
        pM.ChangeBusinessActivity(this, businessActive);

    }

    public void Interact()
    {

    }
    public void Interact(bool to)
    {

    }



}
