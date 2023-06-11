using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum roomTypes { Entrance, StartRoom, Clothes, Groceries, Tech, Toys, Shoes};
public class RoomManager : MonoBehaviour
{
    [SerializeField] public Sprite clothes;
    [SerializeField] public Sprite[] roomSprites = new Sprite[5];
    [SerializeField] GameObject building;
    [SerializeField] GameObject buildings;
    public roomTypes currentRoom;
    public bool checkAdjacent(Vector2 mouseCoordinate)
    {
        if(mouseCoordinate.x < 0 || mouseCoordinate.y < 0)
        {
            return false;
        }
        try
        {
            if (roomDictionary[mouseCoordinate + Vector2.up] != null)
            {
                return true;
            }
        }
        catch
        {

        }
        try
        {
            if (roomDictionary[mouseCoordinate + Vector2.down] != null)
            {
                return true;
            }
        }
        catch
        {

        }
        try
        {
            if (roomDictionary[mouseCoordinate + Vector2.left] != null)
            {
                return true;
            }
        }
        catch
        {

        }
        try
        {
            if (roomDictionary[mouseCoordinate + Vector2.right] != null)
            {
                return true;
            }
        }
        catch
        {

        }
        return false;

    }

    private Camera cam;
    public void newRoom(roomTypes RT, Vector2 CO)
    {
        GameObject bongo = Instantiate(building, CO * new Vector2(2, 1), Quaternion.identity, buildings.transform);
        bongo.GetComponent<SpriteRenderer>().sprite = roomSprites[(int)RT];
        bongo.name = $"{RT} {CO}";
        roomDictionary.Add(CO, bongo);
    }
    struct Room
    {
       public roomTypes type; 
    }
    Dictionary<Vector2, GameObject> roomDictionary = new Dictionary<Vector2, GameObject>();


    void Start()
    {
        cam = Camera.main;
        newRoom(roomTypes.Entrance,new Vector2(0,0));
        newRoom(roomTypes.StartRoom, new Vector2(1, 0));
        newRoom(roomTypes.StartRoom, new Vector2(2, 0));

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mouseReal = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseCoordinate = new Vector2(Mathf.Floor(mouseReal.x/2),Mathf.Floor(mouseReal.y));
            try
            {
                if (roomDictionary[mouseCoordinate] == null)
                {
                    if(checkAdjacent(mouseCoordinate) == true)
                    {
                        newRoom(currentRoom, mouseCoordinate);
                    }
                }
            }
            catch
            {
                if (checkAdjacent(mouseCoordinate) == true)
                {
                    newRoom(currentRoom, mouseCoordinate);
                }
            }
            
        }
    }
}
