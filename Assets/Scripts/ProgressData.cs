using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[Serializable]
public class ProgressData
{
    public int[] money;
    public List<tileInfo> allOccupiedSpaces;
    public ProgressData(Progress progress)
    {
        money = progress.money;
        allOccupiedSpaces = progress.allOccupiedSpaces;
    }
}




