using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnhiredWorkers : MonoBehaviour
{
    public List<WorkerInfo> unhiredWorkers = new List<WorkerInfo>();

    int maxWorkers = 6;


    public string[] names = { "robby", "gobby", "jobby", "yobby", "hobby", "fobby", "cobbie" };

    public int currentAvgLevel;

    public int randomAvgLevel()
    {
        return currentAvgLevel + Random.Range(-2, 3);
    }

    public WorkerInfo GenerateNewWorker()
    {
        return new WorkerInfo { name = names[Random.Range(0, names.Length)], level = randomAvgLevel(),specie = (species)Random.Range(0,System.Enum.GetValues(typeof(species)).Length) };
    }

    public void designateWorker(Business where, WorkerInfo who)
    {
        if (where.hiredWorkers.Count < 3)
        {
            where.hiredWorkers.Add(who);
            unhiredWorkers.Remove(who);
        }
    }

    public void hireWorker(WorkerInfo worker)
    {

    }




}
