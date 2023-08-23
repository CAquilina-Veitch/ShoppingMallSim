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
    public DateTime timeIn;
    public DateTime timeOut;
    public HiredWorkerUI hwui;
    public UnhiredWorkerUI uhwui;

    public WorkerTimePacket(WorkerTimePacketData w)
    {
        _details = w._details;
        info = w.info;
        timeIn = w.timeIn;
        timeOut = w.timeOut;
        hwui = default;
        uhwui = default;
    }


}
[Serializable]
public struct WorkerTimePacketData
{
    public string _details;
    public WorkerInfo info;
    public DateTime timeIn;
    public DateTime timeOut;
    public WorkerTimePacketData(WorkerTimePacket w)
    {
        _details = w._details;
        info = w.info;
        timeIn = w.timeIn;
        timeOut = w.timeOut;
    }
}
public class AllWorkers : MonoBehaviour
{
    [SerializeField] RoomManager rM;
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
            WorkerTimePacket temp = new WorkerTimePacket() { _details = $"{who.info.name} - {who.bsns.oS.coord}", info = new WorkerInfo{name=who.info.name, process = currentWorkerProcess.working, Energy = who.info.Energy, businessCoord = who.info.businessCoord, level= who.info.level, specie = who.info.specie }, hwui = who, timeIn = DateTime.Now, timeOut = timeOut };
            currentProcesses.Add(temp);
            SortPackets();
        }
    }
    void SortPackets()
    {
        currentProcesses.OrderByDescending(x => x.timeOut);
    }


    public void LoadProgress(ProgressData data)
    {
        List<WorkerTimePacketData> currentWorkers = data.currentWorkers;

        List<WorkerTimePacket> newWorkerProcesses = new List<WorkerTimePacket>();

        foreach (WorkerTimePacketData wtpd in currentWorkers)
        {
            WorkerTimePacket _wtp = new WorkerTimePacket(wtpd);

            if (wtpd.timeOut < DateTime.Now)
            {
                //process finished

                if (wtpd.info.process == currentWorkerProcess.working)
                {
                    //finished working now has to be tired
                    //time since out;

                    float timeSinceOut = (float)(DateTime.Now - wtpd.timeOut).TotalMinutes;
                    if (timeSinceOut > 240)
                    {
                        _wtp.info.Energy = 120;
                        _wtp.info.process = currentWorkerProcess.empty;
                        rM.occupiedDictionary[wtpd.info.businessCoord.FloatArrayToVector()].business.hiredWorkers[rM.occupiedDictionary[wtpd.info.businessCoord.FloatArrayToVector()].business.hiredWorkers.FindIndex(x => x == wtpd.info)] = _wtp.info;

                    }
                    else
                    {
                        //potentially problematic
                        _wtp.hwui = rM.occupiedDictionary[_wtp.info.businessCoord.FloatArrayToVector()].business.hiredWUI.First(x => x.info == wtpd.info);
                        _wtp.info.Energy = (int)timeSinceOut / 2;
                        _wtp.info.process = currentWorkerProcess.recovering;

                        newWorkerProcesses.Add(_wtp);
                    }
                    

                }
                else if (wtpd.info.process == currentWorkerProcess.recovering)
                {
                    //finished recovering now done
                    rM.occupiedDictionary[_wtp.info.businessCoord.FloatArrayToVector()].business.hiredWUI.First(x => x.info == wtpd.info).info.Energy = 120;
                    rM.occupiedDictionary[_wtp.info.businessCoord.FloatArrayToVector()].business.hiredWUI.First(x => x.info == wtpd.info).info.process = currentWorkerProcess.empty;

                }

            }
            else
            {
                //process has not finished complete the wtp

                float timeToGo = (float)(wtpd.timeOut - DateTime.Now).TotalMinutes;
                if (wtpd.info.process == currentWorkerProcess.working)
                {
                    _wtp.hwui = rM.occupiedDictionary[_wtp.info.businessCoord.FloatArrayToVector()].business.hiredWUI.First(x => x.info == wtpd.info);
                    _wtp.info.Energy = (int)timeToGo;
                    newWorkerProcesses.Add(_wtp);
                    rM.occupiedDictionary[_wtp.info.businessCoord.FloatArrayToVector()].business.toggleWorker(_wtp.hwui, true);
                }
                else if (wtpd.info.process == currentWorkerProcess.recovering)
                {
                    _wtp.hwui = rM.occupiedDictionary[_wtp.info.businessCoord.FloatArrayToVector()].business.hiredWUI.First(x => x.info == wtpd.info);
                    _wtp.info.Energy = (int)(120 - (0.5 * timeToGo));
                    newWorkerProcesses.Add(_wtp);
                }

            }



        }
        currentWorkers = newWorkerProcesses.WorkerPacketsToWorkerData();

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

            newProcess.info.process = currentWorkerProcess.recovering;
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
