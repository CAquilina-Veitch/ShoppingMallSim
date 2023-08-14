using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[Serializable]
public class Progress : MonoBehaviour
{
    public int[] money = new int[2];

    public Wallet wallet;
    public RoomManager rM;
    public void updateProgress()
    {
        Debug.Log("Updated");
        money[0] = wallet.Currency;
        money[1] = wallet.Premium;
    }
    public void LoadTo(ProgressData data)
    {
        money = data.money;
        wallet.loadToCurrent();
    }
}



[Serializable]
public class ProgressData
{
    public int[] money;
    public ProgressData(Progress progress)
    {
        money = progress.money;
    }
}



