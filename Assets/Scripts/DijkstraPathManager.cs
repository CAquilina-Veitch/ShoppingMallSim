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


    public List<int> shortestPath = new List<int>();

    public int[] connectedNodes;


    private void Start()
    {
        shortestPath = FindShortestPath(new Vector2Int(0,2)).ToList();

        //connectedNodes = ConnectedNodeIDs(2);
    }

    int[] FindShortestPath(Vector2Int fromTo)
    {
        

        List<int> unvisitedNodes = Enumerable.Range(0, nodePoints.Count).ToList();
        List<int> visitedNodes = new List<int>();

        void finishedNode(int node)
        {
            unvisitedNodes.Remove(node);
            visitedNodes.Add(node);
        }



        List<List<int>> routes = new List<List<int>>();
        List<float> routeDistance = new List<float>();
        Dictionary<int, float> distances = new Dictionary<int, float>();


        List<int> temp = new List<int>();
        temp.Add(fromTo.x);
        routes.Add(temp);
        finishedNode(fromTo.x);

        distances.Add(fromTo.x, 0);
        foreach (int node in unvisitedNodes)
        {
            distances.Add(node, Mathf.Infinity);
        }




        bool solved = false;

        //find what nodes are connected to a current node

        int currentRoute = fromTo.x;

        while (solved == false)
        {
            int[] conn = ConnectedNodeIDs(currentRoute);

            for (int i = 0; i < conn.Length; i++)
            {
                Debug.Log(distances[routes[currentRoute].ToArray().LastInt()]);
                Debug.Log(Vector2.Distance(nodePoints[routes[currentRoute].ToArray().LastInt()], nodePoints[conn[i]]));
                float dist = distances[routes[currentRoute].ToArray().LastInt()] + Vector2.Distance(nodePoints[routes[currentRoute].ToArray().LastInt()], nodePoints[conn[i]]);


                if (unvisitedNodes.Contains(conn[i]))
                {
                    distances.Add(conn[i], dist);
                    finishedNode(conn[i]);
                }
                else
                {
                    if (distances.ContainsKey(conn[i]))
                    {
                        if (distances[conn[i]] < dist)
                        {
                            distances[conn[i]] = dist;
                            //delete all paths that contain conn[i]
                        }
                    }

                }




                if(i == 0)
                {
                    routes[currentRoute].Add(conn[i]);
                }
                else
                {
                    routes.Add(routes[currentRoute]);
                    routes[routes.Count()].Add(conn[i]);
                }
            }

            solved = true;
        }




        //List<List<Vector2>> ;



        return new int[1];
    }

    //StepPathForward(int)

    int[] ConnectedNodeIDs(int nodeID)
    {
        List<int> connected = new List<int>();

        List<Vector2> from = new List<Vector2>();
        from.AddRange(connections.FindAll(s => s.x == nodeID).ToArray());
        foreach(Vector2 v in from)
        {
            connected.Add((int)v.y);
        }
        from = new List<Vector2>();
        from.AddRange(connections.FindAll(s => s.y == nodeID).ToArray());
        foreach (Vector2 v in from)
        {
            connected.Add((int)v.x);
        }
        return connected.ToArray();
    }





    /*Vector2[] ConnectionsToPoint(int i)
    {

    }*/


}



