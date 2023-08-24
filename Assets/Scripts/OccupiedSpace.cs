using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


public class OccupiedSpace : MonoBehaviour
{
    public Vector2 coord;
    public constructionType cType;

    [SerializeField] Sprite[] workSprites; //no final ones, contains paths, empty rooms and the constructed but not chosen type.
    [SerializeField] Sprite[] roomSprites; 

    SpriteRenderer sR;
    public RoomManager rM;
    public int currentRoomHighlight;


    public Vector2[] preExistingAdjPaths;

    //business stuff
    public Business business;
    [SerializeField] RectTransform bGList;

    [SerializeField] GameObject chooseBuildingCanvas; 

    //Pathstuff
    public Path path;

    public Wallet w;

    public bool uiOpen;
    [SerializeField] GameObject businessCanvasOwner;
    [SerializeField] shopUI shop;

    //Second node thing
    public List<Vector2> pathFromEntrance = new List<Vector2>();

    public GameObject[] visualLayeredPrefabs;

    public ConstructionVisuals cV;
    


    private void OnEnable()
    {
        sR = GetComponentInChildren<SpriteRenderer>();
        sR.sprite = workSprites[0];
        rM = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>();


    }
    public void init()
    {
        sR = GetComponentInChildren<SpriteRenderer>();
        sR.sortingOrder = -(int)(coord.x + coord.y);
    }
    public void pathEntranceSprite(bool to)
    {
        sR.sprite = workSprites[3];
    }

    public void removeBuilding()
    {
        rM.currentlyOpenedInteractWindow = Vector2.one * -1;
        rM.businesses.Remove(coord);
        rM.occupiedDictionary.Remove(coord);
        Destroy(gameObject);
    }

    public void changeCurrentRoomHighlight(bool increase)
    {
        currentRoomHighlight += increase ? 1 : -1;
        if (currentRoomHighlight < 0)
        {
            currentRoomHighlight = System.Enum.GetValues(typeof(stockType)).Length-1;
        }
        else if (currentRoomHighlight >= System.Enum.GetValues(typeof(stockType)).Length)
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
        cType = cT;
        //hide canvas
        Destroy(transform.GetChild(0).GetComponent<Canvas>().gameObject);

        sR.sprite = workSprites[0];
        
        if (cT == constructionType.Path)
        {
            cV.gameObject.SetActive(true);
            rM.StartConstruction(coord, constructionType.Path);
        }
        else if(cT == constructionType.Business)
        {
            transform.GetChild(1).gameObject.SetActive(true);

            sR.sprite = roomSprites[currentRoomHighlight];

        }
        else
        {

        }
    }

    public void LoadConstruction(tileInfo tI)
    {
        cType = tI.cT;
        Destroy(transform.GetChild(0).GetComponent<Canvas>().gameObject);

        if (tI.cT == constructionType.Path)
        {
            cV.gameObject.SetActive(true);

            CompletePathConstruction();
            Destroy(cV.gameObject);

        }
        else if (tI.cT == constructionType.Business)
        {
            //sR.sprite = workSprites[0];

            sR.enabled = false;

            Destroy(chooseBuildingCanvas);


            CompleteBusinessConstruction();

            Destroy(cV.gameObject);


            business.hiredWorkers = tI.workers.ToList();
            business.WorkerProgress();

        }

    }    
    public void LoadConstructionProcess(tileInfo tI, ConstructionTimePacketData cTPD)
    {
        cType = tI.cT;
        Destroy(transform.GetChild(0).GetComponent<Canvas>().gameObject);
        cV.gameObject.SetActive(true);
        cV.SetPacket(new ConstructionTimePacket(cTPD));
        if (tI.cT == constructionType.Path)
        {
            cV.gameObject.SetActive(true);

        }
        else if (tI.cT == constructionType.Business)
        {
            sR.sprite = workSprites[0];
            cV.gameObject.SetActive(true);

        }

    }



    


    public void CompletePathConstruction()
    {
        path = gameObject.AddComponent(typeof(Path)) as Path;
        path.oS = this;
        Destroy(businessCanvasOwner.gameObject);
        Destroy(chooseBuildingCanvas);
        sR.sprite = workSprites[1];
        GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>().pathAdd(path, coord);
        if (preExistingAdjPaths.Length != 0)
        {
            if (preExistingAdjPaths.Length > 1)
            {
                //junciton, choose shorter, then update them until all are shorter

                List<Vector2Int> pathAndLength = new List<Vector2Int>();
                for (int i = 0; i < preExistingAdjPaths.Length; i++)
                {
                    Vector2Int temp = new Vector2Int(i, rM.occupiedDictionary[preExistingAdjPaths[i]].pathFromEntrance.Count);
                    pathAndLength.Add(temp);
                }



                pathAndLength.Sort((a, b) => a.y.CompareTo(b.y));

                Debug.Log($"{pathAndLength[0]} first, last {pathAndLength[pathAndLength.Count - 1]}");

                pathFromEntrance = new List<Vector2>(rM.occupiedDictionary[preExistingAdjPaths[pathAndLength[0].x]].pathFromEntrance)
                {
                    coord//adds coord on end
                };

                //update others
                for (int i = 1; i < pathAndLength.Count; i++)
                {
                    if (pathFromEntrance.Count + 1 < pathAndLength[i].y)
                    {
                        rM.occupiedDictionary[preExistingAdjPaths[pathAndLength[i].x]].UpdateLength(coord);
                    }
                }

            }
            else
            {
                //not junction
                pathFromEntrance = new List<Vector2>(rM.occupiedDictionary[preExistingAdjPaths[0]].pathFromEntrance)
                {
                    coord//adds coord on end
                };
            }



        }
        else
        {
            pathFromEntrance = new List<Vector2>() { coord };
        }
        path.init();
    }
    public void CompleteBusinessConstruction()
    {

        pathFromEntrance = new List<Vector2>(rM.occupiedDictionary[preExistingAdjPaths[0]].pathFromEntrance)
        {
            coord
        };
        business = gameObject.AddComponent(typeof(Business)) as Business;
        business.oS = this;
        business.shopGUI = shop;
        
        GameObject temp = Instantiate(visualLayeredPrefabs[currentRoomHighlight], sR.transform);

        temp.GetComponent<ShopPosition>().changeOrder(sR.sortingOrder);
        //Debug.Log(1);

        sR.enabled = false;
        rM.AddBusiness(business, coord);
        business.visualPositions = temp.GetComponent<ShopPosition>();
        business.listBG = bGList;
        business.init();
        business.stockDetails.type = (stockType)currentRoomHighlight;
        business.ChangeEntranceDirection();
        rM.constructionTime = new TimeSpan(0, 0, 30);

    }
    public void skipPremium()
    {
        w = GameObject.FindGameObjectWithTag("Wallet").GetComponent<Wallet>();
        if (w.Premium == 3)
        {
            rM.constructionTime = new TimeSpan(0, 0, 1);
            w.Premium -= 3;
        }
        else
        {
            rM.constructionTime = new TimeSpan(0, 0, 30);
        }
    } 

    public void UpdateLength(Vector2 coordReqFrom)
    {
        //Debug.LogError($"{coord} 1");
        preExistingAdjPaths = rM.AdjacentPaths(coord);
        
        //Debug.LogError(preExistingAdjPaths.Count());


        List<Vector2Int> pathAndLength = new List<Vector2Int>();
        for (int i = 0; i < preExistingAdjPaths.Length; i++)
        {
            Vector2Int temp = new Vector2Int(i, rM.occupiedDictionary[preExistingAdjPaths[i]].pathFromEntrance.Count);
            //Debug.Log(temp);
            pathAndLength.Add(temp);
        }



        pathAndLength.Sort((a, b) => a.y.CompareTo(b.y));

        //Debug.LogWarning($"{pathAndLength[0]} first, last {pathAndLength[pathAndLength.Count - 1]}, length of {pathAndLength.Count}");

        pathFromEntrance = new List<Vector2>(rM.occupiedDictionary[preExistingAdjPaths[pathAndLength[0].x]].pathFromEntrance)
        {
            coord//adds coord on end
        };
        //Debug.LogWarning($"{coord} 2");
        //update others
        for (int i = 1; i < pathAndLength.Count; i++)
        {
            //Debug.LogWarning($"{coord} 3");

            if (pathFromEntrance.Count + 1 < pathAndLength[i].y)
            {
                //Debug.LogWarning($"{coord} 4");

                if (coord != coordReqFrom)
                {
                    //Debug.LogWarning($"{coord} 5");

                    rM.occupiedDictionary[preExistingAdjPaths[pathAndLength[i].x]].UpdateLength(coord);
                }
               
            }
        }
    }


    public void confirmHoveredBusiness()
    {
        sR.sprite = workSprites[0];

        //sR.enabled = false;
        cV.gameObject.SetActive(true);
        rM.StartConstruction(coord,(businessTypes)currentRoomHighlight);

        cV.gameObject.SetActive(true);

        Destroy(chooseBuildingCanvas);
    }

    public void ShowBusinessUI(bool to)
    {
        uiOpen = to;
        businessCanvasOwner.SetActive(uiOpen);


    }
    public void ShowBusinessUI()
    {
        ShowBusinessUI(!uiOpen);
    }




}
