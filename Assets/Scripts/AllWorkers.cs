using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Linq;
public enum currentWorkerProcess
{
    empty, working, recovering
}
public struct WorkerTimePacket
{
    public currentWorkerProcess process;
    public DateTime timeIn;
    public DateTime timeOut;
    public HiredWorkerUI hwui;
    public UnhiredWorkerUI uhwui;
}
public class AllWorkers : MonoBehaviour
{
    
    public List<HiredWorkerUI> recoveringHiredWorkers = new List<HiredWorkerUI>();
    public List<UnhiredWorkerUI> recoveringFiredWorkers = new List<UnhiredWorkerUI>();
    public List<HiredWorkerUI> workingWorkers = new List<HiredWorkerUI>();

    Dictionary<HiredWorkerUI, DateTime> timeIn = new Dictionary<HiredWorkerUI,DateTime>();

    List<WorkerTimePacket> currentProcesses = new List<WorkerTimePacket>();

    private void Update()
    {
       
    }
    public void StartWork(HiredWorkerUI who)
    {
        if (currentProcesses.Any(x => x.hwui == who))
        {
            currentProcesses.First(x => x.hwui == who);
        }
        else
        {
            DateTime timeOut = DateTime.Now.AddMinutes(who.energy);
            WorkerTimePacket temp = new WorkerTimePacket() { hwui = who, timeIn = DateTime.Now, process = currentWorkerProcess.working, timeOut = timeOut };
            currentProcesses.Add(temp); 
        }

/*        if (!workingWorkers.Contains(who))
        {
            if (recoveringHiredWorkers.Contains(who))
            {
                UpdateAllEnergies();
                recoveringHiredWorkers.Remove(who);
            }
            workingWorkers.Add(who);
            timeIn.Add(who, DateTime.Now);
        }*/
    }
    void SortPackets()
    {

    }
    public void StopWork(HiredWorkerUI who)
    {
        if (!recoveringHiredWorkers.Contains(who))
        {
            if (workingWorkers.Contains(who))
            {
                //can work

                SortPackets();


                //calculate energy for this guy



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
