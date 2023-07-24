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
    public List<Business> allBusinesses = new List<Business>();

    [SerializeField] RoomManager rM;
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
        //shortestPath = FindShortestPath(new Vector2Int(0,2)).ToList();

        //connectedNodes = ConnectedNodeIDs(2);
    }

    public Vector2[] pathToBusiness(Vector2 to, Vector2 from)
    {
        Debug.Log("aaaaa");
        /*Business b = rM.occupiedDictionary[to].business;
        nodeData toNodeInfo = rM.occupiedDictionary[to].nodeInfo;
        nodeData fromNodeInfo = rM.occupiedDictionary[from].nodeInfo;*/
        int toNodeID = NodePointID(to);
        int fromNodeID = NodePointID(from);

        Vector2Int fromto = new Vector2Int(fromNodeID,toNodeID);
        int[] nodesInOrder = FindShortestPath(fromto);
        ////////////////////////////////make this turn set of nodes into path of vectors
        List<Vector2> positionPath = new List<Vector2>();
        Vector2[] posPath = new Vector2[0];
        foreach (int node in nodesInOrder)
        {
            //Vector2[] array = positionPath.ToArray();

            posPath = posPath.AddArrayToArrayEnd(rM.occupiedDictionary[nodePoints[node]].nodeInfo.path);
        }
        string a = "";
        foreach(Vector2 v in positionPath)
        {
            a += $"{v}";
        }
        Debug.Log(a);





        return new Vector2[1];
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("AAAAAAAAAA");
            pathToBusiness(Vector2.right * 3, Vector2.zero);
        }
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
                Debug.Log(distances[routes[currentRoute].ToArray().Last()]);
                Debug.Log(Vector2.Distance(nodePoints[routes[currentRoute].ToArray().Last()], nodePoints[conn[i]]));
                float dist = distances[routes[currentRoute].ToArray().Last()] + Vector2.Distance(nodePoints[routes[currentRoute].ToArray().Last()], nodePoints[conn[i]]);


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

    public int NodePointID(Vector2 coord)
    {
        if (nodePoints.Contains(coord))
        {
            return nodePoints.IndexOf(coord);
        }
        else
        {
            nodePoints.Add(coord);
            return nodePoints.IndexOf(coord);

        }
    }


}



