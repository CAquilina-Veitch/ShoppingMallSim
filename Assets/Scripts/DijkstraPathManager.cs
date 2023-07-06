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

    public List<Business> activeBusinesses = new List<Business>();

    public void ChangeBusinessActivity(Business b, bool to)
    {
        if (to)
        {
            activeBusinesses.Add(b);
        }
        else
        {
            activeBusinesses.Remove(b);
        }
    }


    public List<Vector2> nodePoints = new List<Vector2>();


    public List<Vector2> connections = new List<Vector2>();


    public List<Vector2> shortestPath = new List<Vector2>();


    private void Start()
    {
        shortestPath = FindShortestPath(new Vector2(0,2)).ToList();
    }

    Vector2[] FindShortestPath(Vector2 toFrom)
    {
        List<Vector2> unvisitedNodes = nodePoints;
        List<Vector2> visitedNodes = new List<Vector2>();

        List<List<Vector2>> routes = new List<List<Vector2>>();

        




        //List<List<Vector2>> ;



        return new Vector2[1];
    }

    Vector2[] ConnectionsToPoint(int i)
    {
        return new Vector2[1];
    }


}



