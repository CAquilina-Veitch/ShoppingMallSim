using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomButton : MonoBehaviour
{
    public businessTypes bt;
    public void roomChoice()
    {
        GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>().currentRoom = bt;
    }
}
