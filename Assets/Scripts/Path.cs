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
    

    public void switchPathIsEntrance(bool to)
    {
        oS.pathEntranceSprite(to);
        rM.changeEntrance(coord, to);
    }
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
        


        bool willBeNode = prexisting.Length == 0; // first path, nothing is connected, make it node;

        //if its an entrance make a node;
        bool isEntrance = rM.checkAdjacentIsEmpty(coord);
        switchPathIsEntrance(isEntrance);
        willBeNode = isEntrance ? true : willBeNode;


        if()


        //check if its a junction of 2+ already existing paths;

        //if it is, make it a node - add connections;

        // if it isnt, just add it to the path;










        /*


                if (nodeInfo.path.Length > 0)
                {
                    //this connects two existing paths;
                    foreach (Vector2 p in nodeInfo.path)
                    {
                        //check each of the paths
                        Debug.Log(rM.pathDictionary.ContainsKey(p));
                        Debug.Log(this.gameObject + "checking out "+p);
                        Debug.Log(rM.pathDictionary[p]);
                        Debug.Log(rM.pathDictionary[p].nodeInfo);
                        Debug.Log(rM.pathDictionary[p].nodeInfo.path);
                        Debug.Log(rM.pathDictionary[p].nodeInfo.path.Length);
                        if ()
                        {

                        }
                        else
                        {
                            if (nodeInfo.path == null)
                            {
                                nodeInfo = rM.pathDictionary[p].nodeInfo;
                                nodeInfo.path = nodeInfo.path.addToArrayEnd(coord);
                            }
                            else
                            {
                                if (nodeInfo.path.Length> rM.pathDictionary[p].nodeInfo.path.Length)
                                {
                                    nodeInfo = rM.pathDictionary[p].nodeInfo;
                                    nodeInfo.path = nodeInfo.path.addToArrayEnd(coord);
                                }
                                else
                                {
                                    Debug.Log(123);
                                }
                            }
                            nodeInfo = rM.pathDictionary[p].nodeInfo;
                        }
                    }


                }*/


        if (willBeNode || isEntrance)
        {
            Debug.Log(1);
            makeNode(true);
        }

    }


    public bool IsNode()
    {
        return node != null;
    }
    public void makeNode(bool entrance)
    {
        if (!IsNode())
        {
            node = gameObject.AddComponent(typeof(Node)) as Node;
            oS.node = node;
            if (entrance)
            {
                node.isEntrance = entrance;
            }
        }
        else
        {
            Debug.LogError("isnode");
        }
    }



}
