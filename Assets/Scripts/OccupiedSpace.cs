using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OccupiedSpace : MonoBehaviour
{
    public Vector2 coord;
    public void chooseConstruction(constructionType CT)
    {
        if(CT == constructionType.Path)
        {
            GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>().pathAdd(gameObject, coord);
        }
        else if(CT == constructionType.Business)
        {

        }
        else
        {

        }
    }
    public void chooseBusiness(businessTypes bT)
    {

    }
}
