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
    public void chooseBusiness()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
