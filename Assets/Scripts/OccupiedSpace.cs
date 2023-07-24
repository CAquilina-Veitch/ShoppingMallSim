using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class OccupiedSpace : MonoBehaviour
{
    public Vector2 coord;
    public constructionType constructionType;
    public constructionType type;

    [SerializeField] Sprite[] workSprites; //no final ones, contains paths, empty rooms and the constructed but not chosen type.
    [SerializeField] Sprite[] roomSprites; 

    SpriteRenderer sR;
    public RoomManager rM;
    public int currentRoomHighlight;


    public Vector2[] preExistingAdjPaths;



    //business stuff
    public Business business;
    [SerializeField] RectTransform bGList;

    //Pathstuff
    public Path path;



    public bool uiOpen;
    [SerializeField] GameObject BusinessCanvasOwner;

    //Second node thing
    public List<Vector2> p = new List<Vector2>();








    private void OnEnable()
    {
        sR = GetComponentInChildren<SpriteRenderer>();
        sR.sprite = workSprites[0];
        rM = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>();



    }
    public void pathEntranceSprite(bool to)
    {
        sR.sprite = workSprites[4];
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
        //hide canvas
        Destroy(transform.GetChild(0).GetComponent<Canvas>().gameObject);

        sR.sprite = workSprites[(int)cT + 1];
        if (preExistingAdjPaths.Length != 0)
        {
            if (preExistingAdjPaths.Length > 1)
            {
                //junciton, choose shorter, then update them untill all are shorter

                List<Vector2Int> pathAndLength = new List<Vector2Int>();
                for(int i = 0; i < preExistingAdjPaths.Length; i++)
                {
                    Vector2Int temp = new Vector2Int(i, rM.occupiedDictionary[preExistingAdjPaths[i]].p.Count);
                    pathAndLength.Add(temp);
                }




                p.Add(coord);

            }
            else
            {
                //not junction
                p = rM.occupiedDictionary[preExistingAdjPaths[0]].p;
                p.Add(coord);
            }



        }

        if (cT == constructionType.Path)
        {
            path = gameObject.AddComponent(typeof(Path)) as Path;
            path.oS = this;

            GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>().pathAdd(path, coord);
            path.init();
            
        }
        else if(cT == constructionType.Business)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            sR.sprite = roomSprites[currentRoomHighlight];

            business.oS = this;
            
        }
        else
        {

        }
    }


    public void confirmHoveredBusiness()
    {
        sR.sprite = roomSprites[currentRoomHighlight];
        Debug.Log(1);
        Destroy(transform.GetChild(0).GetComponent<Canvas>().gameObject);

        business = gameObject.AddComponent(typeof(Business)) as Business;
        rM.AddBusiness(business, coord);
        business.listBG = bGList;
        business.init();



    }

    public void ShowBusinessUI(bool to)
    {
        uiOpen = to;
        BusinessCanvasOwner.SetActive(uiOpen);


    }
    public void ShowBusinessUI()
    {
        ShowBusinessUI(!uiOpen);
    }

}
