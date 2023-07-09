using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public RoomManager rM;
    public OccupiedSpace oS;
    public Vector2 coord;
    public Node node;
    public Vector2[] pathFrom;
    public nodeData nodeInfo;
    
    int lengthBetweenNodes = 3;

    private void OnEnable()
    {
        
        init();
    }

    public void switchPathIsEntrance(bool to)
    {
        oS.pathEntranceSprite(to);
        rM.changeEntrance(coord, to);
    }

    public void init()
    {
        rM = GameObject.FindGameObjectWithTag("BuildingManager").GetComponent<RoomManager>();
        oS = GetComponent<OccupiedSpace>();
        coord = oS.coord;
        Debug.Log(1);





        pathFrom = oS.pathFrom;
        bool isEntrance = rM.checkAdjacentIsEmpty(coord);
        switchPathIsEntrance(isEntrance);


        bool willBeNode = pathFrom.Length == 0; // first path;
        if (pathFrom.Length > 0)
        {
            //this connects two existing paths;
            foreach (Vector2 p in pathFrom)
            {
                Debug.Log(rM.pathDictionary.ContainsKey(p));
                Debug.Log(p);
                Debug.Log(rM.pathDictionary[p]);
                Debug.Log(rM.pathDictionary[p].nodeInfo);
                Debug.Log(rM.pathDictionary[p].nodeInfo.path);
                Debug.Log(rM.pathDictionary[p].nodeInfo.path.Length);
                if (rM.pathDictionary[p].nodeInfo.path.Length > lengthBetweenNodes)
                {
                    willBeNode = true;
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


        }
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
