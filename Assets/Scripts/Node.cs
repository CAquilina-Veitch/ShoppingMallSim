using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct nodeData
{
    public Vector2 nodeCoord;
    public Node node;
    public Vector2[] path;
}
public class Node : MonoBehaviour
{

    public Vector2 coord;
    public OccupiedSpace tile;
    public bool isEntrance;

    public List<OccupiedSpace> ownedTiles = new List<OccupiedSpace>();

    public List<Node> nodes;
    public Dictionary<Node,Vector2[]> nodePaths = new Dictionary<Node, Vector2[]>();

    public Path path;

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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
