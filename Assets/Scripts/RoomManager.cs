using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum businessTypes {Construction, Clothes, Groceries, Videogames, Books, Shoes};
public enum constructionType { Path, Business, Parking};
public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject building;
    [SerializeField] Wallet wallet;
    public int[] roomCost = {5, 25 , 15, 20, 10, 15};
    public DraftTile draft;
    [SerializeField] UnhiredWorkers UHWM;

    public GameObject businessWorkerUIPrefab;

    public Dictionary<Vector2, OccupiedSpace> occupiedDictionary = new Dictionary<Vector2, OccupiedSpace>();
    public Dictionary<Vector2, Path> pathDictionary = new Dictionary<Vector2, Path>();
    Vector2 entrancePosition = Vector2.zero;
    public Dictionary<Vector2, Business> businesses = new Dictionary<Vector2, Business>();

    public void pathAdd(Path path, Vector2 CO)
    {
        Debug.Log("Added path at " + CO);
        pathDictionary.Add(CO, path);
    }

    public bool checkAdjacentHasPath(Vector2 coordinate)
    {
        Debug.Log(21);
        if(coordinate.x < 0 || coordinate.y < 0)
        {
            Debug.Log(26 +""+coordinate );
            return false;
        }
        Debug.Log(22);
        Vector2[] Adj = { Vector2.left, Vector2.down, Vector2.right, Vector2.up };
        foreach(Vector2 v in Adj)
        {
            Debug.Log(23+""+ pathDictionary.ContainsKey(coordinate + v)+ (coordinate + v));
            if (pathDictionary.ContainsKey(coordinate + v))
            {
                return true;
            }
        }
        return false;

    }
    public Vector2[] AdjacentPaths(Vector2 coordinate)
    {
        if (coordinate.x < 0 || coordinate.y < 0)
        {
            return new Vector2[0];
        }
        Vector2[] Adj = { Vector2.left, Vector2.down, Vector2.right, Vector2.up };
        List<Vector2> temp = new List<Vector2>();
        foreach (Vector2 v in Adj)
        {
            if (pathDictionary.ContainsKey(coordinate + v))
            {
                temp.Add(coordinate + v);
            }
        }
        return temp.ToArray();

    }



    public bool checkAdjacentIsEmpty(Vector2 coordinate)
    {
        if (coordinate.x < 0 || coordinate.y < 0)
        {
            return true;
        }
        Vector2[] Adj = { Vector2.left, Vector2.down, Vector2.right, Vector2.up };
        foreach (Vector2 v in Adj)
        {
            if (!occupiedDictionary.ContainsKey(coordinate + v))
            {
                return true;
            }
        }
        return false;

    }

    private Camera cam;
    public void newRoom(Vector2 coord)
    {
        GameObject tileObj = Instantiate(building, coord.isoCoordToWorldPosition(), Quaternion.identity, transform);
        tileObj.name = $"{coord} construction";
        OccupiedSpace temp = tileObj.GetComponent<OccupiedSpace>();
        temp.coord = coord;
        temp.rM = this;
        temp.preExistingAdjPaths = AdjacentPaths(coord);
        Debug.LogWarning(coord + "ADDED");
        occupiedDictionary.Add(coord, temp);
    }
    struct Room
    {
       public businessTypes type; 
    }



    public void SetEntrancePosition(Vector2 coord)
    {
        Debug.LogError($"CHANGING FROM {entrancePosition} to {coord}");
        entrancePosition = coord;
    }
    void Start()
    {
        cam = Camera.main;
        newRoom(new Vector2(0, 0));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)||Input.touchCount==1)
        {
            Vector2 clickedTile;
            if (Input.touchCount == 1) 
            {
                clickedTile = coordToTileCenterPos(cam.ScreenToWorldPoint(Input.GetTouch(1).position)).worldToIsoCoord();
            }
            else
            {   
                //// mouse controls
                clickedTile = coordToTileCenterPos(cam.ScreenToWorldPoint(Input.mousePosition)).worldToIsoCoord();
            }

            if (checkEmpty(clickedTile))
            {
                //empty
                if (draft.coord == clickedTile)
                {
                    TryBuild(clickedTile);
                }
                else
                {
                    draft.moveDraft(clickedTile);
                }
            }
            else
            {   
                //not empty
                draft.draftVisibility(false);

                
                if (businesses.ContainsKey(clickedTile))
                {
                    
                    Debug.Log(5);

                    if (occupiedDictionary[clickedTile].uiOpen)
                    {
                        if (UHWM.selected.Count > 0)
                        {
                            Debug.Log(6);

                            //try to move the workers to here
                            UHWM.TryDesignateSelectedWorkers(businesses[clickedTile]);
                            businesses[clickedTile].UpdateWorkerUI();
                        }
                    }
                    else
                    {
                        occupiedDictionary[clickedTile].ShowBusinessUI(true);
                    }
                    
                }
                else
                {
                    Debug.Log(4);
                }
                
                





            }

            

            


            
                        
        }

        // Controls





        /// Phone


    }
    public bool checkEmpty(Vector2 coord)
    {
        return !occupiedDictionary.ContainsKey(coord);
    }

    public void TryBuild(Vector2 clickedTile)
    {
        if (!occupiedDictionary.ContainsKey(clickedTile))
        {
            if (checkAdjacentHasPath(clickedTile) == true)
            {
                if(wallet.trySpend(false, roomCost[0]) == true)
                {
                    newRoom(clickedTile);
                    draft.draftVisibility(false);
                }
                else
                {
                    draft.flashRed();
                }
            }
            else
            {
                draft.flashRed();
            }
        }
        else
        {
            draft.flashRed();
        }
    }

    public void increaseCost(businessTypes bT)
    {
        roomCost[(int)bT] = (int)(roomCost[(int)bT] * 1.2f);
        
    }
       

    public Vector2 coordToTileCenterPos(Vector2 tapWorld)
    {
        /*Vector2 tapWorld = cam.ScreenToWorldPoint(tapPos);*/

        Vector2 r = tapWorld;

        r.x = Mathf.Round(tapWorld.x);
        r.y = Mathf.Round(tapWorld.y * 2) / 2;

        if ((r.x % 2 == 0 && r.y % 1 == 0) || (Mathf.Abs(r.x) % 2 == 1 && Mathf.Abs(r.y) % 1 == 0.5))
        {
            return r;
        }
        else
        {
            //it is not a centre point
            Vector2 d = tapWorld - r;

            Vector2 nm = Mathf.Abs(d.x) / 2 >= Mathf.Abs(d.y) ? Vector2.right : Vector2.up * 0.5f;
            d *= nm;
            //nd /= 2;

            Vector2 sol = tapWorld + d.normalized * new Vector2(1, 0.5f);
            sol.x = Mathf.Round(sol.x);
            sol.y = Mathf.Round(sol.y * 2) / 2;
            return sol;
        }
        
    }
    public Vector2[] allAdjPaths(Vector2 coord)
    {
        List<Vector2> temp = new List<Vector2>();
        Vector2[] Adj = { Vector2.left, Vector2.down, Vector2.right, Vector2.up };
        foreach (Vector2 v in Adj)
        {
            if (pathDictionary.ContainsKey(coord + v))
            {
                temp.Add(coord + v);
            }
        }
        return temp.ToArray();
    }

    public void FixedUpdate()
    {
        
    }
    public void AddBusiness(Business b, Vector2 coord)
    {
        if (!businesses.ContainsKey(coord))
        {
            businesses.Add(coord, b);
        }
    }



}


