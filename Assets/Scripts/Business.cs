using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum stockType
{
    groceries, two,three,four,five
}

public class Business : MonoBehaviour
{

    public stockType stock;
    public int maxStock;
    public int currentStock;

    public Worker worker;

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

}
