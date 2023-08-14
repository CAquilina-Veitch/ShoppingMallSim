using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GlobalFunctions
{
    public static Vector3 isoCoordToWorldPosition(this Vector2 coord)
    {
        return new Vector3(coord.x - coord.y, 0.5f * (coord.x + coord.y));
    }
    public static Vector3[] isoCoordToWorldPosition(this Vector2[] coords)
    {
        Vector3[] temp = new Vector3[coords.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = coords[i].isoCoordToWorldPosition();
        }
        return temp;
    }
    public static Vector3 worldToIsoCoord(this Vector2 pos)
    {
        return new Vector3(pos.x / 2 + pos.y, pos.y - pos.x / 2);
    }
    public static Vector2[] reverseArray(Vector2[] array)
    {
        Vector2[] temp = new Vector2[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            temp[i] = array[array.Length - 1 - i];
        }
        return temp;
    }
    public static Vector3[] reverseArray(Vector3[] array)
    {
        Vector3[] temp = new Vector3[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            temp[i] = array[array.Length - 1 - i];
        }
        return temp;
    }
    public static Vector2[] orderArrayToCoord(Vector2[] array, Vector2 coordGoal)
    {
        if (array.Length < 1)
        {
            return array;
        }
        if (array[0] == coordGoal)
        {
            if (array[array.Length - 1] != coordGoal)
            {
                return reverseArray(array);
            }
            Debug.Log(2);
        }
        Debug.Log(1);
        return array;

    }
    public static Vector2[] addToArrayStart(this Vector2[] array, Vector2 added)
    {
        List<Vector2> temp = new List<Vector2>();
        temp.Add(added);
        temp.AddRange(array);
        return temp.ToArray();
    }
    public static Vector2[] addToArrayEnd(this Vector2[] array, Vector2 added)
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
            Debug.Log("AAA" + array);
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
            Debug.Log("AAA" + array);
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
            Debug.Log("AAA" + array);
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
            Debug.Log("AAA" + array);
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
        //Debug.Log("worker" + ((species)Random.Range(0, System.Enum.GetValues(typeof(species)).Length)));
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

}