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
[Serializable]
public struct WorkerTimePacket
{
    public string _details;
    public WorkerInfo info;
    public currentWorkerProcess process;
    public DateTime timeIn;
    public DateTime timeOut;
    public HiredWorkerUI hwui;
    public UnhiredWorkerUI uhwui;
}
public class AllWorkers : MonoBehaviour
{
    public List<WorkerTimePacket> currentProcesses = new List<WorkerTimePacket>();

    public void StartWork(HiredWorkerUI who)
    {
        Debug.Log(1);
        if (currentProcesses.Any(x => x.hwui == who))
        {
            Debug.LogError($"somehow found {currentProcesses.First(x => x.hwui == who)._details}");
        }
        else
        {
            DateTime timeOut = DateTime.Now.AddMinutes(who.info.Energy);
            WorkerTimePacket temp = new WorkerTimePacket() { _details = $"{who.info.name} - {who.bsns.oS.coord}", info = who.info, hwui = who, timeIn = DateTime.Now, process = currentWorkerProcess.working, timeOut = timeOut };
            currentProcesses.Add(temp);
            SortPackets();
        }
    }
    void SortPackets()
    {
        currentProcesses.OrderByDescending(x => x.timeOut);
    }
    public void StopWork(HiredWorkerUI who)
    {
        if (currentProcesses.Any(x => x.hwui == who))
        {
            //+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+   check if there are multiple;

            SortPackets();

            WorkerTimePacket process = currentProcesses.First(x => x.hwui == who);

            if (process.timeOut.CompareTo(DateTime.Now) >= 0)
            {
                //then timeout is larger than now - yet to come;
                who.info.Energy = process.timeOut.Subtract(process.timeOut).Minutes - DateTime.Now.Subtract(process.timeIn).Minutes;
            }
            else
            {
                who.info.Energy = 0;
            }


            WorkerTimePacket newProcess = process;

            currentProcesses.Remove(process);

            newProcess.process = currentWorkerProcess.recovering;
            newProcess.timeIn = DateTime.Now;
            newProcess.timeOut = DateTime.Now.AddMinutes((120 - who.info.Energy)*2);

            currentProcesses.Add(newProcess);

            SortPackets();

        }
        UpdateAllEnergies();


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
