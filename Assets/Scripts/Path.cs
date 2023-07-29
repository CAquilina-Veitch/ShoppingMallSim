using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public RoomManager rM;
    public OccupiedSpace oS;
    public Vector2 coord;
   
    public void created()
    {

    }

    public void init()
    { 
        //figure out nodeInfo




        rM = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>();
        oS = GetComponent<OccupiedSpace>();
        coord = oS.coord;
        Debug.Log("path inited");

    }



}
