using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;

public enum stockType
{
    Books, Clothing, Games, Groceries, Shoes
}
[Serializable]
public class WorkerInfo
{
    public string name;
    public int level;
    public species specie; 
    private int energy;
    public currentWorkerProcess process;
    public int Energy
    {
        get
        {
            return energy;
        }
        set
        {
            energy = Mathf.Clamp(value, 0, 120);
        }
    }
}

public class Business : MonoBehaviour
{
    public OccupiedSpace oS;


    public stockInfo stockDetails;

    Vector2 pathFrom;

    public List<WorkerInfo> hiredWorkers = new List<WorkerInfo>();
    public List<HiredWorkerUI> activeWorkers = new List<HiredWorkerUI>();
    public List<HiredWorkerUI> hiredWUI = new List<HiredWorkerUI>();

    public bool businessActive;

    Customers c;

    bool interactionOpen = false;
    public RectTransform listBG;
    Transform canvasesOwner;

    public shopUI shopGUI;
    public ShopPosition visualPositions;


    public void toggleWorker(HiredWorkerUI who)
    {
        if (activeWorkers.Contains(who))
        {
            Debug.Log(3);
            activeWorkers.Remove(who);
            GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<AllWorkers>().StopWork(who);

        }
        else
        {
            Debug.Log(4);
            activeWorkers.Add(who);
            GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<AllWorkers>().StartWork(who);
            //add activeworkers
            
        }
    }

    public void ChangeEntranceDirection()
    {
        pathFrom = oS.pathFromEntrance.ToArray().Last() - oS.pathFromEntrance.ToArray().Last(1);
        Debug.Log(pathFrom);
        if(pathFrom == Vector2.up)
        {
            
        }
        else if (pathFrom == Vector2.down)
        {

        }else if(pathFrom == Vector2.left)
        {

        }else if (pathFrom == Vector2.right)
        {

        }
        else
        {
            Debug.LogError($"this shouldnt happen, had {pathFrom}, from {oS.pathFromEntrance.ToArray().Last()} -  {oS.pathFromEntrance.ToArray().Last(1)}");
        }


    }

    public void init()
    {
        canvasesOwner = listBG.parent.parent;
        GameObject businessWorkerUIPrefab = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>().businessWorkerUIPrefab;
        for (int i = 0; i < 3; i++)
        {
            //Debug.LogWarning(listBG);
            GameObject temp = Instantiate(businessWorkerUIPrefab,listBG);
            hiredWUI.Add(temp.GetComponent<HiredWorkerUI>());
            temp.GetComponent<HiredWorkerUI>().init(this);

            
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
        shopGUI = canvasesOwner.GetComponentInChildren<shopUI>();
        shopGUI.init(this);
        updateVisualWorkers();
        StartCoroutine(secondTicked());
    }

    private void FixedUpdate()
    {
        if (!businessActive)
        {
            if (activeWorkers.Count > 0 && stockDetails.amount > 0)
            {
                ToggleActivity(true);
            }
        }
        else
        {
            if (activeWorkers.Count == 0 || stockDetails.amount <= 0)
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
        updateVisualWorkers();
    }
    public void updateVisualWorkers()
    {
        for(int i = 0; i < visualPositions.npcs.Length; i++)
        {
            if (i >= activeWorkers.Count)
            {
                visualPositions.ChangeSprite(i);
            }
            else
            {
                visualPositions.ChangeSprite(i, activeWorkers[i].info.specie);
            }
        }
    }

    public void Interact()
    {
        Interact(!interactionOpen);
    }
    public void Interact(bool to)
    {
        //Debug.Log($" {to} ing, {oS.coord}");
        if (listBG == null)
        {
            Debug.LogError("no listbg" + gameObject);
            return;
        }
        interactionOpen = to;

        if (interactionOpen)
        {
            shopGUI.updateVisual();
            for (int i = 0; i < 3; i++)
            {
                //Debug.Log($"setting hired worker {i} of {hiredWUI.Count} to {i} of {hiredWorkers.Count}");
                if (hiredWorkers.Count > i)
                {
                    hiredWUI[i].info = hiredWorkers[i];
                    hiredWUI[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -20);
                    hiredWUI[i].Unswipe();
                }
                
            }
        }
        else
        {
            

        }
        Debug.Log(interactionOpen);
        canvasesOwner.gameObject.SetActive(interactionOpen);
        oS.uiOpen = interactionOpen;
        foreach (HiredWorkerUI hwui in hiredWUI)
        {
            hwui.updateVisuals();
        }
        shopGUI.updateVisual();
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
        foreach(HiredWorkerUI hwui in hiredWUI)
        {
            Debug.Log($"updating {hwui.info.name}, to be {hwui.info.specie} ");
            hwui.updateVisuals();
        }
    }
    IEnumerator secondTicked()
    {
        stockDetails.amount += activeWorkers.Count;
        yield return new WaitForSeconds(1);
        StartCoroutine(secondTicked());
    }

}
