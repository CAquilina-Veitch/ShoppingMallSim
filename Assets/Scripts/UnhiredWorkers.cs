using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UnhiredWorkers : MonoBehaviour
{
    public List<WorkerInfo> unhiredWorkers = new List<WorkerInfo>();

    public List<UnhiredWorkerUI> unhiredWUI = new List<UnhiredWorkerUI>();

    public RectTransform scrollParent;

    float scrollAmnt;

    int maxWorkers = 6;


    public string[] names = { "robby", "gobby", "jobby", "yobby", "hobby", "fobby", "cobbie" };

    public int currentAvgLevel;

    [SerializeField] RectTransform listBG;
    [SerializeField] GameObject workerUIPrefab;

    bool listShowing;
    public void showList(bool to)
    {
        unhiredWorkers.Sort((x, y) => y.level.CompareTo(x.level));

        listShowing = to;
        listBG.gameObject.SetActive(listShowing);

        if (listShowing)
        {
            scrollParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, unhiredWorkers.Count * 50);
            listBG.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, unhiredWorkers.Count * 50);


            for(int i = 0; i < unhiredWorkers.Count; i++)
            {
                if (unhiredWUI.Count <= i)
                {
                    var temp = Instantiate(workerUIPrefab, scrollParent);
                    unhiredWUI.Add(temp.GetComponent<UnhiredWorkerUI>());
                    temp.GetComponent<UnhiredWorkerUI>().UHWM = this;
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

        }
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

    public WorkerInfo RandomNewWorker()
    {
        return new WorkerInfo
        {
            name = names[Random.Range(0, names.Length)],
            level = randomAvgLevel(),
            specie = (species)Random.Range(0, System.Enum.GetValues(typeof(species)).Length)
        };
    }

    public void designateWorker(Business where, WorkerInfo who)
    {
        if (where.hiredWorkers.Count < 3)
        {
            where.hiredWorkers.Add(who);
            unhiredWorkers.Remove(who);
        }
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
    public void collectWorker(Worker worker)
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
        collectWorker(RandomNewWorker());
        collectWorker(RandomNewWorker());
        collectWorker(RandomNewWorker());
    }


    public void selectedUHW(bool)
    {

    }



}
