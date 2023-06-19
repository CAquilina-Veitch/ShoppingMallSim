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
    public void pathAdd(GameObject path, Vector2 CO)
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
        GameObject bongo = Instantiate(building, coord.isoCoordToWorldPosition(), Quaternion.identity, transform);
        //bongo.GetComponent<SpriteRenderer>().sprite = roomSprites[(int)RT];
        bongo.name = $" construction happneing at {coord}";
        bongo.GetComponent<OccupiedSpace>().coord = coord;
        bongo.GetComponent<OccupiedSpace>().rM = this;
        occupiedDictionary.Add(coord, bongo);
    }
    struct Room
    {
       public businessTypes type; 
    }

    Dictionary<Vector2, GameObject> occupiedDictionary = new Dictionary<Vector2, GameObject>();
    Dictionary<Vector2, GameObject> pathDictionary = new Dictionary<Vector2, GameObject>();
    List<Vector2> entrances = new List<Vector2>();

    public void changeEntrance(Vector2 coord, bool isEntrance)
    {
        if (isEntrance)
        {
            entrances.Add(coord);
        }
        else
        {
            entrances.Remove(coord);
        }
    }

    void Start()
    {
        cam = Camera.main;
        newRoom(new Vector2(0,0));
        newRoom(new Vector2(1, 0));
        newRoom(new Vector2(2, 0));
        newRoom(new Vector2(0, 1));
        newRoom(new Vector2(1, 1));
        newRoom(new Vector2(2, 1));
        newRoom(new Vector2(1, 2));
        newRoom(new Vector2(0, 2));
        newRoom(new Vector2(2, 2));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 clickedTile = coordToTileCenterPos(cam.ScreenToWorldPoint(Input.mousePosition)).worldToIsoCoord();

            if (draft.coord == clickedTile)
            {
                TryBuild(clickedTile);
            }
            else
            {
                if (checkEmpty(clickedTile))
                {
                    draft.moveDraft(clickedTile);
                }
                else
                {
                    draft.draftVisibility(false);
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

    public void updatePaths(Vector2 coord)
    {
        foreach(Vector2 c in allAdjPaths(coord))
        {
            occupiedDictionary[c].GetComponent<OccupiedSpace>().switchPathIsEntrance(checkAdjacentIsEmpty(c));
        }
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





}

public static class GlobalFunctions
{
    public static Vector3 isoCoordToWorldPosition(this Vector2 coord)
    {
        return new Vector3(coord.x-coord.y,0.5f*(coord.x+coord.y));
    }
    public static Vector3 worldToIsoCoord(this Vector2 pos)
    {
        return new Vector3(pos.x/2+pos.y,pos.y-pos.x/2);
    }
}
