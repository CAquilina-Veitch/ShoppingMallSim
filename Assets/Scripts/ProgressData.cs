using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[Serializable]
public class ProgressData
{
    public int[] money;
    public Dictionary<Vector2, OccupiedSpace> allOccupiedSpaces;
    public ProgressData(Progress progress)
    {
        money = progress.money;
        allOccupiedSpaces = progress.allOccupiedSpaces;
    }
}




