using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
public enum currentWorkerProcess
{
    empty, working, recovering
}
public struct WorkerTimePacket
{
    public WorkerInfo info;
    public currentWorkerProcess process;
    public DateTime point;
    public HiredWorkerUI hwui;
    public UnhiredWorkerUI uhwui;
}
public class AllWorkers : MonoBehaviour
{
    
    public List<HiredWorkerUI> recoveringHiredWorkers = new List<HiredWorkerUI>();
    public List<UnhiredWorkerUI> recoveringFiredWorkers = new List<UnhiredWorkerUI>();
    public List<HiredWorkerUI> workingWorkers = new List<HiredWorkerUI>();

    Dictionary<HiredWorkerUI, DateTime> timeIn = new Dictionary<HiredWorkerUI,DateTime>();


    private void Update()
    {
       
    }
    public void StartWork(HiredWorkerUI who)
    {
        if (!workingWorkers.Contains(who))
        {
            if (recoveringHiredWorkers.Contains(who))
            {
                UpdateAllEnergies();
                recoveringHiredWorkers.Remove(who);
            }
            workingWorkers.Add(who);
            timeIn.Add(who, DateTime.Now);
        }
    }
    public void StopWork(HiredWorkerUI who)
    {
        if (!recoveringHiredWorkers.Contains(who))
        {
            if (workingWorkers.Contains(who))
            {
                //can work




                UpdateAllEnergies();
                workingWorkers.Remove(who);
            }
            recoveringHiredWorkers.Add(who);
        }
    }
    public void ConvertRecovering(UnhiredWorkerUI whoWas)
    {

    }
    public void ConvertRecovering(HiredWorkerUI whoWas)
    {

    }
    private void OnEnable()
    {
        StartCoroutine(UpdateEverySecond());
    }
    IEnumerator UpdateEverySecond()
    {
        UpdateAllEnergies();
        yield return new WaitForSeconds(1);
        StartCoroutine(UpdateEverySecond());
    }
    public void UpdateAllEnergies()
    {

    }
    
}
