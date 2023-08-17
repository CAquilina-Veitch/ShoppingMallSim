using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[Serializable]
public class Progress : MonoBehaviour
{
    public int[] money = {0,0};
    public List<tileInfo> allOccupiedSpaces = new List<tileInfo>();
    public Wallet wallet;
    public RoomManager rM;
    public void updateProgress()
    {
        money[0] = wallet.Currency;
        money[1] = wallet.Premium;
        updateTiles();
    }
    void updateTiles()
    {
        List<Vector2> coords = new List<Vector2>(rM.occupiedDictionary.Keys);
        allOccupiedSpaces = new List<tileInfo>();
        foreach(Vector2 c in coords)
        {
            OccupiedSpace oS = rM.occupiedDictionary[c];

            tileInfo temp = new tileInfo(new float[] {oS.coord.x,oS.coord.y},oS.cType); 
            allOccupiedSpaces.Add(temp);
        }
    }
    public void LoadProgress(ProgressData data)
    {
        money = data.money;
        allOccupiedSpaces = data.allOccupiedSpaces;
        wallet.loadToCurrent();
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
    public float[] coord;
    public constructionType cT;
}