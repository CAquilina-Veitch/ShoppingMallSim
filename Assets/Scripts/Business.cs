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

    public Worker worker;

    public List<WorkerInfo> hiredWorkers;


    public bool businessActive;

    DijkstraPathManager pM;

    private void OnEnable()
    {
        pM = GameObject.FindGameObjectWithTag("PathManager").GetComponent<DijkstraPathManager>();

        if (worker == null)
        {
            if(TryGetComponent(out Worker w))
            {
                worker = w;
            }
            else
            {
                worker = gameObject.AddComponent<Worker>();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!businessActive)
        {
            if (worker != null && currentStock > 0)
            {
                ToggleActivity(true);
            }
        }
        else
        {
            if (worker == null || currentStock <= 0)
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
