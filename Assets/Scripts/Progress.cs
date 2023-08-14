using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[Serializable]
public class Progress : MonoBehaviour
{
    public int[] money = {0,0};
    public Dictionary<Vector2, OccupiedSpace> allOccupiedSpaces; 
    public Wallet wallet;
    public RoomManager rM;
    public void updateProgress()
    {
        money[0] = wallet.Currency;
        money[1] = wallet.Premium;
        allOccupiedSpaces = rM.occupiedDictionary;
    }
    public void LoadTo(ProgressData data)
    {
        money = data.money;
        wallet.loadToCurrent();
        rM.occupiedDictionary = allOccupiedSpaces;
    }
}