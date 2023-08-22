using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[Serializable]
public class ProgressData
{
    public int[] money;
    public List<tileInfo> allOccupiedSpaces;
    public int[] carparkProgress;
    public List<ConstructionTimePacketData> currentConstructions = new List<ConstructionTimePacketData>();
    public List<WorkerTimePacketData> currentWorkers = new List<WorkerTimePacketData>();
    public ProgressData(Progress progress)
    {
        money = progress.money;
        allOccupiedSpaces = progress.allOccupiedSpaces;
        carparkProgress = progress.carParkUpgrades;
        currentConstructions = progress.currentConstructions;
        currentWorkers = progress.currentWorkers;
    }
}




