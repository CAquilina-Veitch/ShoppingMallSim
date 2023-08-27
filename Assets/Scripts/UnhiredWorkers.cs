using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Unity.VisualScripting;
using System.IO;

public class UnhiredWorkers : MonoBehaviour
{
    public List<WorkerInfo> unhiredWorkers = new List<WorkerInfo>();

    public List<UnhiredWorkerUI> unhiredWUI = new List<UnhiredWorkerUI>();
    public List<UnhiredWorkerUI> selected = new List<UnhiredWorkerUI>();
    [SerializeField] Storage fileStorage;
    //public RectTransform scrollParent;

    float scrollAmnt;

    int maxWorkers = 6;
    int maxWorkersBusiness = 3;

    public int currentAvgLevel;

    [SerializeField] RectTransform listBG;
    [SerializeField] GameObject workerUIPrefab;

    bool listShowing;

    public void showList(bool to)
    {
        if (unhiredWorkers.Count < 1)
        {
            return;
        }

        unhiredWorkers.Sort((x, y) => y.level.CompareTo(x.level));

        listShowing = to;
        

        if (listShowing)
        {
            //scrollParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, unhiredWorkers.Count * 50); ////////////////////////////////////////////////// if scroll

            float length = unhiredWorkers.Count * 50;
            if (length > 0)
            {
                listBG.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, length);
            }
            else
            {
                listBG.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20);
            }
            


            for(int i = 0; i < unhiredWorkers.Count; i++)
            {
                if (unhiredWUI.Count <= i)
                {
                    var temp = Instantiate(workerUIPrefab, /*scrollParent*/ listBG);
                    unhiredWUI.Add(temp.GetComponent<UnhiredWorkerUI>());
                    temp.GetComponent<UnhiredWorkerUI>().init(this);
                }
                unhiredWUI[i].info = unhiredWorkers[i];
                unhiredWUI[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -50);
            }
            if (unhiredWUI.Count > unhiredWorkers.Count)
            {
                unhiredWUI.RemoveRange(unhiredWorkers.Count - 1, unhiredWUI.Count - unhiredWorkers.Count);
            }
        }
        else
        {
            if (selected.Count > 0)
            {
                listShowing = true;
            }
            
        }
        listBG.gameObject.SetActive(listShowing);
        foreach (UnhiredWorkerUI uhwui in unhiredWUI)
        {
            uhwui.updateVisuals();
        }


    }

    public void ClearSelected()
    {
        selected = new List<UnhiredWorkerUI>();
    }


    public void showList()
    {
        showList(!listShowing);
    }

    private void OnEnable()
    {
        showList(false);
    }



    public int randomAvgLevel()
    {
        return currentAvgLevel + Random.Range(-2, 3);
    }



    public void TryDesignateSelectedWorkers(Business b)
    {
        Debug.Log("trying");
        List<UnhiredWorkerUI> toRemove = new List<UnhiredWorkerUI>();
        foreach( UnhiredWorkerUI uhw in selected)
        {
            if (b.hiredWorkers.Count < 3)
            {
                b.hiredWorkers.Add(uhw.info);
                toRemove.Add(uhw);
                b.UpdateWorkerUI();
            }
            else
            {
                Debug.LogError("FULL BUSINESS");
            }
        }
        foreach(UnhiredWorkerUI uhw in toRemove)
        {
            unhiredWorkers.Remove(uhw.info);
            selected.Remove(uhw);
            unhiredWUI.Remove(uhw);
            Destroy(uhw.gameObject);
        }
        StartCoroutine(updateLength());

    }

    public void designateWorker(Business where, UnhiredWorkerUI who)
    {
        if (where.hiredWorkers.Count < 3)
        {
            where.hiredWorkers.Add(who.info);
            unhiredWorkers.Remove(who.info);
            selected.Remove(who);
            unhiredWUI.Remove(who);
            Destroy(who.gameObject);

        }
        where.UpdateWorkerUI();
        
    }

    public void collectWorker(WorkerInfo worker)
    {
        if (unhiredWorkers.Count < maxWorkers)
        {
            unhiredWorkers.Add(worker);
        }
        else
        {
            Debug.LogError("Too many workers");
        }
    }
    public void collectWorker(CollectableWorker worker)
    {
        if (unhiredWorkers.Count < maxWorkers)
        {
            unhiredWorkers.Add(worker.info);
            worker.Collected();
        }
        else
        {
            Debug.LogError("Too many workers");
        }
    }



    private void Start()
    {
        if (!fileStorage.FileExists())
        {
            collectWorker(GlobalFunctions.RandomNewWorker(0));
            collectWorker(GlobalFunctions.RandomNewWorker(0));
            collectWorker(GlobalFunctions.RandomNewWorker(0));
        }
    }

    public void collectRandomWorker(species s)
    {
        collectWorker(GlobalFunctions.RandomNewWorker(s));
    }
    public void selectedUHW(UnhiredWorkerUI ui)
    {
        selected.Add(ui);
    }
    
    IEnumerator updateLength()
    {

        float target = unhiredWorkers.Count * 50;
        float was = listBG.sizeDelta.y;
        if (was!=target)
        {
            float[] wases = new float[unhiredWorkers.Count];
            for(int j = 0; j < unhiredWorkers.Count; j++)
            {
                unhiredWUI[j].info = unhiredWorkers[j]; // idk if this is needed;
                wases[j] = unhiredWUI[j].GetComponent<RectTransform>().sizeDelta.y;
            }
            float timeDelay = 0.1f;
            for (float i = 0; i < timeDelay; i += 0.02f)
            {
                float b = i / timeDelay;
                float c = Mathf.Lerp(was, Mathf.Clamp(target, 20, Mathf.Infinity), b);
                listBG.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, c);

                for (int j = 0; j < unhiredWorkers.Count; j++)
                {
                    float d = Mathf.Lerp(wases[j], j * -50, b);
                    unhiredWUI[j].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, d);
                }



                yield return new WaitForSeconds(0.02f);
                if (!listShowing)
                {
                    break;
                }
            }
        }
        else
        {
            yield return null;
        }
        if(target == 0)
        {
            showList(false);
        }
        

    }


}
