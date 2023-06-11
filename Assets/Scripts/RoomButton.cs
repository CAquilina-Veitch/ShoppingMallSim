using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomButton : MonoBehaviour
{
    public roomTypes rt;
    public void roomChoice()
    {
        GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>().currentRoom = rt;
    }
    
}
