using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public struct nodeData
{
    public Vector2 nodeCoord;
    public Node node;
    public Vector2[] path;
}
public class Node : MonoBehaviour
{
    public int id;

    public Vector2 coord;
    public OccupiedSpace tile;
    public bool isEntrance;

    public List<OccupiedSpace> ownedTiles = new List<OccupiedSpace>();

    public List<Node> nodes = new List<Node>();
    public Dictionary<Node,Vector2[]> nodePaths = new Dictionary<Node, Vector2[]>();
    public Dictionary<Node, nodeData> connectedNodes = new Dictionary<Node, nodeData>();
    public List<nodeData> nodeConnections = new List<nodeData>();

    public Path path;

    public RoomManager rM;

    private void OnEnable()
    {
        tile = GetComponent<OccupiedSpace>();
        path = GetComponent<Path>();
        coord = tile.coord;
    }

    public void UpdateOwnership()
    {

    }
    public void addTile(OccupiedSpace tile)
    {
        ownedTiles.Add(tile);
    }

    public void addConnection(nodeData to)
    {
        if (rM == null)
        {
            rM = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>();
        }

        if (!nodes.Contains(to.node))
        {
            nodes.Add(to.node);
        }
        if (!connectedNodes.ContainsKey(to.node))
        {
            to = to.MakeConnectionStartHere(coord);
            connectedNodes.Add(to.node, to);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
