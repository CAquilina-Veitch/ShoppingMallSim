using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DijkstraPathManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// Dictionary<Vector2, Node> nodes = new Dictionary<Vector2, Node>();
    /// </summary>



    public List<Vector2> nodePoints = new List<Vector2>();


    public List<Vector2> connections = new List<Vector2>();


    public List<Vector2> shortestPath = new List<Vector2>();


    private void Start()
    {
        shortestPath = FindShortestPath(new Vector2(0,2)).ToList();
    }

    Vector2[] FindShortestPath(Vector2 toFrom)
    {
        List<List<Vector2>> e;



        return new Vector2[1];
    }


}



