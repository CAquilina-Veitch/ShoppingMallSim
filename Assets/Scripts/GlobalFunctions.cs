using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalFunctions
{
    public static Vector3 isoCoordToWorldPosition(this Vector2 coord)
    {
        return new Vector3(coord.x - coord.y, 0.5f * (coord.x + coord.y));
    }
    public static Vector3 worldToIsoCoord(this Vector2 pos)
    {
        return new Vector3(pos.x / 2 + pos.y, pos.y - pos.x / 2);
    }
    public static Vector2[] reverseArray(Vector2[] array)
    {
        Vector2[] temp = array;
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

    public static int LastInt(this int[] array)
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


}
