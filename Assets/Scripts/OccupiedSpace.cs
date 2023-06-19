using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OccupiedSpace : MonoBehaviour
{
    public Vector2 coord;

    [SerializeField] Sprite[] workSprites; //no final ones, contains paths, empty rooms and the constructed but not chosen type.
    [SerializeField] Sprite[] roomSprites; 

    SpriteRenderer sR;
    public RoomManager rM;
    public int currentRoomHighlight;

    private void OnEnable()
    {
        sR = GetComponentInChildren<SpriteRenderer>();
        sR.sprite = workSprites[0];
        rM = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>();
        rM.updatePaths(coord);





    }

    public void switchPathIsEntrance(bool to)
    {
        sR.sprite = to ? workSprites[4] : workSprites[1];
        rM.changeEntrance(coord,to);
    }

    public void changeCurrentRoomHighlight(bool increase)
    {
        currentRoomHighlight += increase ? 1 : -1;
        if (currentRoomHighlight < 0)
        {
            currentRoomHighlight = 5;
        }
        else if (currentRoomHighlight > 5)
        {
            currentRoomHighlight = 0;
        }
        sR.sprite = roomSprites[currentRoomHighlight];

    }


    public void chooseConstructionInt(int i )//cos u cant choose an enum in inspector
    {
        chooseConstruction((constructionType)i);
    }
    public void chooseConstruction(constructionType cT)
    {
        rM.updatePaths(coord);
        //hide canvas
        Destroy(transform.GetChild(0).GetComponent<Canvas>().gameObject);

        sR.sprite = workSprites[(int)cT + 1];

        if (cT == constructionType.Path)
        {
            GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>().pathAdd(gameObject, coord);
            switchPathIsEntrance(rM.checkAdjacentIsEmpty(coord));
            
        }
        else if(cT == constructionType.Business)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {

        }
    }
    public void chooseBusiness(businessTypes bT)
    {

    }
}
