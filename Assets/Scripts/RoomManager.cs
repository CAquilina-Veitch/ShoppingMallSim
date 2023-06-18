using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum businessTypes { Entrance, StartRoom, Clothes, Groceries, Tech, Toys, Shoes};
public enum constructionType { Path, Business, Parking};
public class RoomManager : MonoBehaviour
{
    [SerializeField] public Sprite[] roomSprites = new Sprite[5];
    [SerializeField] GameObject building;
    public businessTypes currentRoom;

    public void pathAdd(GameObject path, Vector2 CO)
    {
        pathDictionary.Add(CO, path);
    }

    public bool checkAdjacent(Vector2 mouseCoordinate)
    {
        if(mouseCoordinate.x < 0 || mouseCoordinate.y < 0)
        {
            return false;
        }
        Vector2[] Adj = { Vector2.left, Vector2.down, Vector2.right, Vector2.up };
        foreach(Vector2 v in Adj)
        {
            try
            {
                if (pathDictionary[mouseCoordinate + v] != null)
                {
                    return true;
                }
            }
            catch
            {

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
        occupiedDictionary.Add(coord, bongo);
    }
    struct Room
    {
       public businessTypes type; 
    }

    Dictionary<Vector2, GameObject> occupiedDictionary = new Dictionary<Vector2, GameObject>();
    Dictionary<Vector2, GameObject> pathDictionary = new Dictionary<Vector2, GameObject>();


    void Start()
    {
        cam = Camera.main;
        newRoom(new Vector2(0,0));
        newRoom( new Vector2(1, 0));
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
            coordToTileCenter(Input.mousePosition);


            Vector2 mouseReal = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseCoordinate = new Vector2(Mathf.Floor(mouseReal.x/2),Mathf.Floor(mouseReal.y));
            try
            {
                if (occupiedDictionary[mouseCoordinate] == null)
                {
                    if(checkAdjacent(mouseCoordinate) == true)
                    {
                        newRoom(mouseCoordinate);
                    }
                }
            }
            catch
            {
                if (checkAdjacent(mouseCoordinate) == true)
                {
                    newRoom( mouseCoordinate);
                }
            }
            
        }

        // Controls





        /// Phone


    }

    

    public Vector2 coordToTileCenter(Vector2 tapPos)
    {
        Vector2 sol = Vector2.zero;
        Vector2 tapWorld = cam.ScreenToWorldPoint(tapPos);
        //Input.GetTouch(0).position

        Vector2 temp = tapWorld;


        Vector2 r = tapWorld;

        r.x = Mathf.Round(tapWorld.x);
        r.y = Mathf.Round(tapWorld.y*2)/2;

        if ((r.x % 2 == 0 && r.y % 1 == 0) || (Mathf.Abs(r.x) % 2 == 1 && Mathf.Abs(r.y) % 1 == 0.5))
        {
            sol = r;
        }
        else
        {
            //it is not a centre point
            Vector2 d = tapWorld - r;

            Vector2 nm = Mathf.Abs(d.x)/2 >= Mathf.Abs(d.y) ? Vector2.right : Vector2.up*0.5f;
            d *= nm;
            //nd /= 2;

            sol = tapWorld + d.normalized * new Vector2(1, 0.5f);
            sol.x = Mathf.Round(sol.x);
            sol.y = Mathf.Round(sol.y * 2) / 2;
        }
        return sol;
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
