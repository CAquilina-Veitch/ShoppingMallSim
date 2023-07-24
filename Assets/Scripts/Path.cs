using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public RoomManager rM;
    public OccupiedSpace oS;
    public Vector2 coord;
    public Node node;
    public nodeData nodeInfo;
   
    public void created()
    {

    }

    public void init()
    { 
        //figure out nodeInfo




        rM = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>();
        oS = GetComponent<OccupiedSpace>();
        coord = oS.coord;
        Debug.Log(1);




        ref Vector2[] prexisting = ref oS.preExistingAdjPaths;
        


        if (prexisting.Length > 1)
        {
            rM.NodePointID(coord);
            makeNode();
            //its a junction of 2+ already existing paths;

            // junction
            foreach (Vector2 touchingPath in prexisting)
            {
                nodeInfo = rM.pathDictionary[touchingPath].nodeInfo;
                nodeInfo.path = nodeInfo.path.addToArrayEnd(coord);
                rM.addConnection(nodeInfo.nodeCoord, coord);
                node.addConnection(nodeInfo);
            }


        
            nodeInfo = new nodeData { node = this.node, nodeCoord = coord, path = new Vector2[1] { coord } };
            oS.nodeInfo = nodeInfo;
        }
        else if(prexisting.Length == 0)
        {
            rM.NodePointID(coord);
            makeNode();
            // first path, nothing is connected, make it node, make it entrance;
            //entrance
            rM.SetEntrancePosition(coord);

            if (prexisting.Length == 0)
            {
                nodeInfo = new nodeData { node = this.node, nodeCoord = coord, path = new Vector2[1] { coord } };
                oS.nodeInfo = nodeInfo;
                return;//first ever path
            }
            /*                nodeInfo = rM.pathDictionary[prexisting[0]].nodeInfo;//gets the old path
                            nodeInfo.path = nodeInfo.path.addToArrayEnd(coord);//adds to end
            */
            rM.addConnection(nodeInfo.nodeCoord, coord);
            node.addConnection(nodeInfo);


            nodeInfo = new nodeData { node = this.node, nodeCoord = coord, path = new Vector2[1] { coord } };
            oS.nodeInfo = nodeInfo;
        }
        else 
        {
            // if it isnt, just add it to the path;
            nodeInfo = rM.pathDictionary[prexisting[0]].nodeInfo;//gets the old path
            nodeInfo.path = nodeInfo.path.addToArrayEnd(coord);//adds to end

            //add to node

            
            node = rM.pathDictionary[prexisting[0]].node;
            Debug.Log(node);
            Debug.Log(oS);
            node.addTile(oS);
        }
    }


    public bool IsNode()
    {
        return node != null;
    }
    public void makeNode()
    {
        Debug.LogWarning("NEW NODE");
        if (!IsNode())
        {
            node = gameObject.AddComponent(typeof(Node)) as Node;
            oS.node = node;
            node.id=rM.NodePointID(coord);
        }
        else
        {
            Debug.LogError("isalrdynode");
        }
    }



}
