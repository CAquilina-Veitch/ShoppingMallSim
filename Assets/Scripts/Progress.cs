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
            Vector2 coord = rM.occupiedDictionary[c].coord;
            tileInfo temp = new tileInfo(new float[] {coord.x,coord.y}); 
            allOccupiedSpaces.Add(temp);
        }
    }
    public void LoadTo(ProgressData data)
    {
        money = data.money;
        wallet.loadToCurrent();
    }
}

[Serializable]
public class tileInfo
{
    public tileInfo(float[] v)
    {
        coord = v;
    }
    public float[] coord;
}