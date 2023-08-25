using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class GlobalFunctions
{
    public static Vector3 IsoCoordToWorldPosition(this Vector2 coord)
    {
        return new Vector3(coord.x - coord.y, 0.5f * (coord.x + coord.y));
    }
    public static Vector3[] IsoCoordToWorldPosition(this Vector2[] coords)
    {
        Vector3[] temp = new Vector3[coords.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = coords[i].IsoCoordToWorldPosition();
        }
        return temp;
    }
    public static Vector3 WorldToIsoCoord(this Vector2 pos)
    {
        return new Vector3(pos.x / 2 + pos.y, pos.y - pos.x / 2);
    }
    public static Vector2[] ReverseArray(Vector2[] array)
    {
        Vector2[] temp = new Vector2[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            temp[i] = array[array.Length - 1 - i];
        }
        return temp;
    }
    public static Vector3[] ReverseArray(Vector3[] array)
    {
        Vector3[] temp = new Vector3[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            temp[i] = array[array.Length - 1 - i];
        }
        return temp;
    }
    public static Vector2[] OrderArrayToCoord(Vector2[] array, Vector2 coordGoal)
    {
        if (array.Length < 1)
        {
            return array;
        }
        if (array[0] == coordGoal)
        {
            if (array[array.Length - 1] != coordGoal)
            {
                return ReverseArray(array);
            }
        }
        return array;
    }
    public static Vector2[] AddToArrayStart(this Vector2[] array, Vector2 added)
    {
        List<Vector2> temp = new List<Vector2>();
        temp.Add(added);
        temp.AddRange(array);
        return temp.ToArray();
    }
    public static Vector2[] AddToArrayEnd(this Vector2[] array, Vector2 added)
    {
        Vector2[] temp;
        if (array == null)
        {
            temp = new Vector2[1] { added };
        }
        else
        {
            temp = new Vector2[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
            {
                temp[i] = array[i];
            }
            temp[temp.Length - 1] = added;
        }
        return temp;
    }

    public static int Last(this int[] array)
    {
        int temp;
        try
        {
            temp = array[array.Length - 1];
        }
        catch
        {
            temp = -123;
        }
        return temp;
    }
    public static Vector2 Last(this Vector2[] array)
    {
        Vector2 temp;
        try
        {
            temp = array[array.Length - 1];
        }
        catch
        {
            temp = Vector2.negativeInfinity;
        }
        return temp;
    }
    public static Vector2 Last(this Vector2[] array, int from)
    {
        Vector2 temp;
        try
        {
            temp = array[array.Length - 1 - from];
        }
        catch
        {
            temp = Vector2.negativeInfinity;
        }
        return temp;
    }
    public static Vector2 First(this Vector2[] array)
    {
        Vector2 temp;
        try
        {
            temp = array[0];
        }
        catch
        {
            temp = Vector2.negativeInfinity;
        }
        return temp;
    }
    public static Vector2 Flip(this Vector2 v)
    {
        return new Vector2(v.y, v.x);
    }


    public static Vector2[] AddArrayToArrayEnd(this Vector2[] array, Vector2[] arrayToAdd)
    {
        List<Vector2> list = new List<Vector2>(array);

        if (array.Length > 0 && arrayToAdd.Length > 0)
        {
            if (list[list.Count - 1] == arrayToAdd[0])
            {
                list.RemoveAt(list.Count - 1);
            }
        }
        list.AddRange(arrayToAdd);
        return list.ToArray();
    }
    static string[][] nameSegments =
        {
        new string[]
        {
            "C","B","R","G","J","L","W","Sh","Bl","N","Fr","F","K","S","P","Cr"
        },
        new string[]
        {
            "o","i","u","e"
        },
        new string[]
        {
            "bb","dd","ff","ch","pp","rt","sh","ll","l","d"
        },new string[]
        {
            "y","s","ie","er","olas","opher"
        }

    };
    public static WorkerInfo RandomNewWorker(int lvl)
    {
        string _name = "";
        for (int i = 0; i < nameSegments.Count(); i++)
        {
            _name = $"{_name}{nameSegments[i][UnityEngine.Random.Range(0, nameSegments[i].Count())]}";
        }
        return new WorkerInfo
        {
            name = _name,
            level = Mathf.Max(0, lvl + UnityEngine.Random.Range(-2, 2)),
            specie = (species)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(species)).Length),
            Energy = 120
        };
    }
    public static WorkerInfo RandomNewWorker(species s)
    {
        string _name = "";
        for (int i = 0; i < nameSegments.Count(); i++)
        {
            _name = $"{_name}{nameSegments[i][UnityEngine.Random.Range(0, nameSegments[i].Count())]}";
        }
        return new WorkerInfo
        {
            name = _name,
            level = Mathf.Max(0, UnityEngine.Random.Range(-2, 2)),
            specie = s,
            Energy = 120
        };
    }
    public static List<float[]> VectorToFloatArray(this List<Vector2> list)
    {
        List<float[]> temp = new List<float[]>();
        foreach(Vector2 v in list)
        {
            temp.Add(new float[2] { v.x, v.y });
        }
        return temp;
    }
    public static float[] VectorToFloatArray(this Vector2 v)
    {
        return new float[2] { v.x, v.y };
    }
    public static List<Vector2> FloatArrayToVector(this List<float[]> list)
    {
        List<Vector2> temp = new List<Vector2>();
        foreach(float[] f in list)
        {
            temp.Add(new Vector2(f[0], f[1]));
        }
        return temp;
    }
    public static Vector2 FloatArrayToVector(this float[] f)
    {
        return new Vector2(f[0], f[1]);
    }
    public static string ConvertTimeSpanToDigitalClock(this TimeSpan timeSpan)
    {
        string formattedTime;
        if (timeSpan.TotalMinutes >= 60)
        {
            formattedTime = string.Format("{0}:{1:D2}", (int)timeSpan.TotalHours, timeSpan.Minutes);
        }
        else
        {
            formattedTime = string.Format("{0}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
        return formattedTime;
    }

    public static List<ConstructionTimePacketData> TimePacketsToTimeData(this List<ConstructionTimePacket> cTP)
    {
        List<ConstructionTimePacketData> temp = new List<ConstructionTimePacketData>();
        foreach(ConstructionTimePacket p in cTP)
        {
            temp.Add(new ConstructionTimePacketData(p));
        }
        return temp;
    }   
    public static List<ConstructionTimePacket> TimeDataToTimePacks(this List<ConstructionTimePacketData> cTP)
    {
        List<ConstructionTimePacket> temp = new List<ConstructionTimePacket>();
        foreach(ConstructionTimePacketData p in cTP)
        {
            temp.Add(new ConstructionTimePacket(p));
        }
        return temp;
    }    
    public static List<WorkerTimePacketData> WorkerPacketsToWorkerData(this List<WorkerTimePacket> cTP)
    {
        List<WorkerTimePacketData> temp = new List<WorkerTimePacketData>();
        foreach(WorkerTimePacket p in cTP)
        {
            temp.Add(new WorkerTimePacketData(p));
        }
        return temp;
    }   
    public static List<WorkerTimePacket> WorkerDataToWorkerPacks(this List<WorkerTimePacketData> cTP)
    {
        List<WorkerTimePacket> temp = new List<WorkerTimePacket>();
        foreach(WorkerTimePacketData p in cTP)
        {
            temp.Add(new WorkerTimePacket(p));
        }
        return temp;
    }

    public static bool ContainsTile(this List<ConstructionTimePacketData> list, tileInfo tI, out ConstructionTimePacketData matchingTileData)
    {
        matchingTileData = default;
        foreach (ConstructionTimePacketData p in list)
        {
            if (p.coord.FloatArrayToVector() == tI.coord.FloatArrayToVector())
            {
                matchingTileData = p;
                return true;
            }
        }
        return false;
    }    
    public static bool ContainsTile(this List<ConstructionTimePacketData> list, tileInfo tI)
    {
        foreach (ConstructionTimePacketData p in list)
        {
            if (p.coord.FloatArrayToVector() == tI.coord.FloatArrayToVector())
            {
                return true;
            }
        }
        return false;
    }
    public static Vector3 camZ(this Vector3 v)
    {
        return new Vector3(v.x, v.y, -10);
    }
    public static int EquivilantSortingLayer(this Vector3 v)
    {
        Vector2 coord = GlobalFunctions.WorldToIsoCoord(v);
        
        return -(int)(coord.x + coord.y);
    }
    public static int EquivilantSortingLayer(this Vector2 v)
    {
        return -(int)(v.x + v.y);
    }
}