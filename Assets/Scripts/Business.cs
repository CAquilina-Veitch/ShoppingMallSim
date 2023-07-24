using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

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
    public float energy;
}

public class Business : MonoBehaviour
{
    public OccupiedSpace oS;

    public stockType stock;
    public int maxStock;
    public int currentStock;

    public List<WorkerInfo> hiredWorkers = new List<WorkerInfo>();
    public List<WorkerInfo> activeWorkers = new List<WorkerInfo>();
    public List<HiredWorkerUI> hiredWUI = new List<HiredWorkerUI>();

    public bool businessActive;

    Customers c;

    bool interactionOpen = false;
    public RectTransform listBG;
    public void init()
    {
        GameObject businessWorkerUIPrefab = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>().businessWorkerUIPrefab;
        for (int i = 0; i < 3; i++)
        {
            Debug.LogWarning(listBG);
            GameObject temp = Instantiate(businessWorkerUIPrefab,listBG);
            hiredWUI.Add(temp.GetComponent<HiredWorkerUI>());
            temp.GetComponent<HiredWorkerUI>().init(this);

            Debug.Log(i);
            
            hiredWUI[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -20);

            if (hiredWorkers.Count>i)
            {
                hiredWUI[i].info = hiredWorkers[i];
            }
            else
            {
                hiredWUI[i].info = new WorkerInfo
                {
                    name = "null"
                };
            }



        }
        
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
        if (c == null)
        {
            c = GameObject.FindGameObjectWithTag("Customers").GetComponent<Customers>();
        }
        c.ChangeBusinessActivity(this, businessActive);
    }
    public void ToggleActivity(bool to)
    {
        businessActive = to;
        if (c == null)
        {
            c = GameObject.FindGameObjectWithTag("Customers").GetComponent<Customers>();
        }
        c.ChangeBusinessActivity(this, businessActive);

    }

    public void Interact()
    {
        Interact(!interactionOpen);
    }
    public void Interact(bool to)
    {
        if (listBG == null)
        {
            Debug.LogError("no listbg" + gameObject);
            return;
        }
        interactionOpen = to;

        if (interactionOpen)
        {
            for (int i = 0; i < 3; i++)
            {
                hiredWUI[i].info = hiredWorkers[i];
                hiredWUI[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -50);
            }
            if (hiredWUI.Count > hiredWUI.Count)
            {
                hiredWUI.RemoveRange(hiredWUI.Count - 1, hiredWUI.Count - hiredWUI.Count);
            }
        }
        else
        {
            interactionOpen = true;

        }

        listBG.gameObject.SetActive(interactionOpen);
    }
    
    public void UpdateWorkerUI()
    {
        hiredWorkers.Sort((x, y) => y.level.CompareTo(x.level));

        for (int i = 0; i < 3; i++)
        {
            Debug.Log(i);

            if (hiredWorkers.Count > i)
            {
                hiredWUI[i].info = hiredWorkers[i];
            }
            else
            {
                hiredWUI[i].info = new WorkerInfo
                {
                    name = "null"
                };
            }



        }
    }

}
