using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OccupiedSpace : MonoBehaviour
{
    public Vector2 coord;

    [SerializeField] Sprite[] workSprites; //no final ones, contains paths, empty rooms and the constructed but not chosen type.

    SpriteRenderer sR;

    private void OnEnable()
    {
        sR = GetComponentInChildren<SpriteRenderer>();
        sR.sprite = workSprites[0];






    }

    public void chooseConstructionInt(int i )//cos u cant choose an enum in inspector
    {
        chooseConstruction((constructionType)i);
    }
    public void chooseConstruction(constructionType cT)
    {
        //hide canvas
        Destroy(GetComponentInChildren<Canvas>().gameObject);/*
        if (transform.GetChild(0).name == "ChooseConstructionCanvas")
        {
            Destroy(transform.GetChild(0));

        }*/



        if (cT == constructionType.Path)
        {
            GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>().pathAdd(gameObject, coord);
            sR.sprite = workSprites[(int)cT + 1];
        }
        else if(cT == constructionType.Business)
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
