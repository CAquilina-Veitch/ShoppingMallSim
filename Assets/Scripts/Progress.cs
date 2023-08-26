using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Progress : MonoBehaviour
{
    

    public Wallet wallet;
    public RoomManager rM;
    public AllWorkers aW;
    public DailySpin ds;
    public UnhiredWorkers uHWM;    public TutorialProgress tP;
    public int[] money = {0,0};
    public int[] carParkUpgrades = new int[3] {0,0,0 };
    public List<tileInfo> allOccupiedSpaces = new List<tileInfo>();
    public DateTime lastSpin;
    public List<ConstructionTimePacketData> currentConstructions = new List<ConstructionTimePacketData>();
    public List<WorkerTimePacketData> currentWorkers = new List<WorkerTimePacketData>();

    public List<WorkerInfo> unhiredWorkers = new List<WorkerInfo>();
    public int tutorialProgress;
    public TimeSpan[] constructionTimes;
    [SerializeField] Carpark initialCarpark;
    [SerializeField] Carpark[] carparkButtonsLeft;
    [SerializeField] Carpark[] carparkButtonsRight;


    public void updateProgress()
    {
        money[0] = wallet.Currency;
        money[1] = wallet.Premium;
        currentConstructions = rM.currentConstructions.TimePacketsToTimeData();
        currentWorkers = aW.currentProcesses.WorkerPacketsToWorkerData();
        updateTiles();        unhiredWorkers = uHWM.unhiredWorkers;        tutorialProgress = tP.UpdateProgress();
        constructionTimes = rM.ConstructionTimes;
    }

    public void LoadProgress(ProgressData data)
    {
        money = data.money;
        allOccupiedSpaces = data.allOccupiedSpaces;
        wallet.LoadToCurrent();
        rM.LoadToCurrent(data);
        SetCarpark(data.carparkProgress);

        rM.currentConstructions = data.currentConstructions.TimeDataToTimePacks();
        aW.LoadProgress(data);
        lastSpin = data.lastSpin;
        ds.lastSpun = lastSpin;
        uHWM.unhiredWorkers = data.unhiredWorkers;
        tP.SetProgress(data.tP);
        rM.ConstructionTimes = data.constructionTimes;
    }




    void SetCarpark(int[] progress)
    {
        carParkUpgrades = progress;

        if (carParkUpgrades[0] != 0)
        {
            initialCarpark.forceCarpark();

            if (carParkUpgrades[0] == -1)
            {
                foreach(Carpark c in carparkButtonsLeft)
                {
                    c.forceCarpark();
                }
                foreach(Carpark c in carparkButtonsRight)
                {
                    c.forceCarpark();
                }
            }
            else
            {
                for (int i = 0; i < carParkUpgrades[1]; i++)
                {                    Debug.Log("number is" + i);
                    Debug.Log("carpark is" + carparkButtonsLeft.Length);
                    carparkButtonsLeft[i].forceCarpark();
                }
                for (int i = 0; i < carParkUpgrades[2]; i++)
                {
                    carparkButtonsRight[i].forceCarpark();
                }
            }
        }





    }

    void updateTiles()
    {
        List<Vector2> coords = new List<Vector2>(rM.occupiedDictionary.Keys);
        allOccupiedSpaces = new List<tileInfo>();
        foreach(Vector2 c in coords)
        {
            OccupiedSpace oS = rM.occupiedDictionary[c];

            tileInfo temp = new tileInfo(oS); 
            allOccupiedSpaces.Add(temp);
        }
    }

}

[Serializable]
public class tileInfo
{
    public tileInfo(float[] v,constructionType c)
    {
        coord = v;
        cT = c;
    }
    public tileInfo(OccupiedSpace oS)
    {
        coord = oS.coord.VectorToFloatArray();
        cT = oS.cType;
        businessType = oS.currentRoomHighlight;

        if (cT == constructionType.Business)
        {            try
            {
                workers = oS.business.hiredWorkers.ToArray();

                stock = oS.business.stockDetails;
            }
            catch
            {
                //business is still being constructed
            }
        }
        pathFromEntrance = oS.pathFromEntrance.VectorToFloatArray();
        preExistingTiles = oS.preExistingAdjPaths.ToList().VectorToFloatArray();
    }
    public float[] coord;
    public constructionType cT;
    public int businessType;
    public WorkerInfo[] workers;
    public stockInfo stock;
    public List<float[]> pathFromEntrance;
    public List<float[]> preExistingTiles;
}