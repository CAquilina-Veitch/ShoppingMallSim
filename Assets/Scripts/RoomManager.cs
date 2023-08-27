using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public enum businessTypes {Construction, Clothes, Groceries, Videogames, Books, Shoes};
public enum constructionType { Path, Business, Parking};

[Serializable]
public struct ConstructionTimePacket
{
    public ConstructionTimePacket(ConstructionTimePacketData d)
    {
        _details = d._details;
        coord = d.coord.FloatArrayToVector();
        isPath = d.isPath;
        businessType = d.businessType;
        timeIn = d.timeIn;
        timeOut = d.timeOut;

    }
    public string _details;
    public Vector2 coord;
    public bool isPath;
    public businessTypes businessType;
    public DateTime timeIn;
    public DateTime timeOut;
}
[Serializable]
public struct ConstructionTimePacketData
{
    public ConstructionTimePacketData(ConstructionTimePacket d)
    {
        _details = d._details;
        coord = d.coord.VectorToFloatArray();
        isPath = d.isPath;
        businessType = d.businessType;
        timeIn = d.timeIn;
        timeOut = d.timeOut;

    }
    public string _details;
    public float[] coord;
    public bool isPath;
    public businessTypes businessType;
    public DateTime timeIn;
    public DateTime timeOut;
}
public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject building;
    [SerializeField] Wallet wallet;
    public int[] roomCost = {5, 25 , 15, 20, 10, 15};
    public DraftTile draft;
    [SerializeField] UnhiredWorkers UHWM;

    public GameObject businessWorkerUIPrefab;

    public Dictionary<Vector2, OccupiedSpace> occupiedDictionary = new Dictionary<Vector2, OccupiedSpace>();
    public Dictionary<Vector2, Path> pathDictionary = new Dictionary<Vector2, Path>();

    public Dictionary<Vector2, Business> businesses = new Dictionary<Vector2, Business>();
    public Vector2 currentlyOpenedInteractWindow = Vector2.one * -1;

    //float constructionTime = 1/4f;
    TimeSpan businessConstructionTime = new TimeSpan(0, 0, 5);
    TimeSpan pathConstructionTime = new TimeSpan(0, 0, 5);
    [SerializeField] CameraMove cM;
    public List<ConstructionTimePacket> currentConstructions = new List<ConstructionTimePacket>();

    [SerializeField] Progress progress;

    [SerializeField] Storage fileStorage;
    [SerializeField] GameObject clickyGuy;
    public List<GameObject> clickyGuys = new List<GameObject>();
    public TimeSpan[] ConstructionTimes
    {
        get
        {
            Debug.LogWarning($"{pathConstructionTime} {businessConstructionTime}");
            return new TimeSpan[2] { pathConstructionTime, businessConstructionTime };
        }
        set
        {
            Debug.LogWarning($"{value[0]} {value[1]}");

            pathConstructionTime = value[0];
            businessConstructionTime = value[1];

        }
    }


    private void Awake()
    {
        if (!fileStorage.FileExists())
        {
            newInitialRoom(new Vector2(0, 0));

        }
    }




    public void StartConstruction(Vector2 coordWhere, businessTypes bT)
    {
        //Debug.Log(1);
        if (currentConstructions.Any(x => x.coord == coordWhere))
        {
            //currentProcesses.First(x => x.coord == coordWhere);
            Debug.LogError($" somehow found {coordWhere}");

        }
        else
        {
            DateTime timeOut = DateTime.Now.Add(businessConstructionTime);
            businessConstructionTime *= 1.8f;
            ConstructionTimePacket temp = new ConstructionTimePacket() { _details = $"{coordWhere} - {bT}", timeIn = DateTime.Now, timeOut = timeOut, coord = coordWhere, isPath = false, businessType = bT };
            currentConstructions.Add(temp);
            SortPackets();
            occupiedDictionary[coordWhere].cV.SetPacket(temp);
        }
    }
    public void StartConstruction(Vector2 coordWhere, constructionType cT)
    {
        //Debug.Log(1);
        if (currentConstructions.Any(x => x.coord == coordWhere))
        {
            //currentProcesses.First(x => x.coord == coordWhere);
            Debug.LogError($" somehow found {coordWhere}");
        }
        else
        {
            DateTime timeOut = DateTime.Now.Add(pathConstructionTime);
            pathConstructionTime *= 1.4f;
            ConstructionTimePacket temp = new ConstructionTimePacket() { _details = $"{coordWhere} - {cT}", timeIn = DateTime.Now, timeOut = timeOut, coord = coordWhere, isPath = true };
            temp._details = $"{temp._details} - {temp.timeOut.ToString("HH:mm")}";
            currentConstructions.Add(temp);
            SortPackets();
            occupiedDictionary[coordWhere].cV.SetPacket(temp);
        }
    }
    public void SortPackets()
    {
        currentConstructions.OrderByDescending(x => x.timeOut);
    }
    IEnumerator UpdateEverySecond()
    {
        SortPackets();
        
        while (true)
        {
            if (currentConstructions.Count > 0)
            {
                Debug.Log($"{currentConstructions.Count} {currentConstructions[0].coord}   -=-= {currentConstructions[0].timeOut} out, now is {DateTime.Now}");
                for(int i = 0; i < currentConstructions.Count; i++)
                {
                    if (currentConstructions[i].timeOut < DateTime.Now)
                    {
                        List<ConstructionTimePacket> toRemove = new List<ConstructionTimePacket>();
                        foreach (ConstructionTimePacket ctp in currentConstructions)
                        {

                            if (ctp.timeOut < DateTime.Now)
                            {

                                if (ctp.isPath)
                                {
                                    occupiedDictionary[ctp.coord].CompletePathConstruction();
                                }
                                else
                                {
                                    occupiedDictionary[ctp.coord].CompleteBusinessConstruction();
                                }
                                toRemove.Add(ctp);
                            }
                        }
                        foreach (ConstructionTimePacket ctp in toRemove)
                        {
                            currentConstructions.Remove(ctp);
                        }

                    }
                }
                
            }
            yield return new WaitForSeconds(0.5f);
        }
        
    }
    IEnumerator spawnNewWorkers()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(20,70));
            //yield return new WaitForSeconds(1);
            if (UHWM.unhiredWorkers.Count < 4)
            {
                UHWM.collectWorker(GlobalFunctions.RandomNewWorker(0));
            }
            /*Path[] paths = GetComponentsInChildren<Path>();
            if (paths.Length > 0)
            {
                if ((UHWM.unhiredWorkers.Count + clickyGuys.Count) < 4)
                {
                    Vector2 coord = paths[UnityEngine.Random.Range(0, paths.Length)].oS.coord;
                    GameObject Temp = Instantiate(clickyGuy, coord.IsoCoordToWorldPosition() + new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), UnityEngine.Random.Range(-0.2f, 0.2f)), Quaternion.identity, transform);
                    clickyGuys.Add(Temp);
                }
            }*/
        }
    }



    public void pathAdd(Path path, Vector2 CO)
    {
        //Debug.Log("Added path at " + CO);


        pathDictionary.Add(CO, path);
    }

    public bool checkAdjacentHasPath(Vector2 coordinate)
    {
        //Debug.Log(21);
        if(coordinate.x < 0 || coordinate.y < 0)
        {
            //Debug.Log(26 +""+coordinate );
            return false;
        }
        //Debug.Log(22);
        Vector2[] Adj = { Vector2.left, Vector2.down, Vector2.right, Vector2.up };
        foreach(Vector2 v in Adj)
        {
            //Debug.Log(23+""+ pathDictionary.ContainsKey(coordinate + v)+ (coordinate + v));
            if (pathDictionary.ContainsKey(coordinate + v))
            {
                return true;
            }
        }
        return false;

    }
    public Vector2[] AdjacentPaths(Vector2 coordinate)
    {
        //Debug.Log(1);
        if (coordinate.x < 0 || coordinate.y < 0)
        {
            return new Vector2[0];
        }
        //Debug.Log(2);

        Vector2[] Adj = { Vector2.left, Vector2.down, Vector2.right, Vector2.up };
        List<Vector2> temp = new List<Vector2>();
        foreach (Vector2 v in Adj)
        {
            //Debug.Log(v);

            if (pathDictionary.ContainsKey(coordinate + v))
            {
                //Debug.Log($"{coordinate+v}fouund");

                temp.Add(coordinate + v);
            }
            else
            {
               // Debug.Log($"{coordinate+v} not found");
            }
        }
        return temp.ToArray();

    }

    public bool checkAdjacentIsEmpty(Vector2 coordinate)
    {
        if (coordinate.x < 0 || coordinate.y < 0)
        {
            return true;
        }
        Vector2[] Adj = { Vector2.left, Vector2.down, Vector2.right, Vector2.up };
        foreach (Vector2 v in Adj)
        {
            if (!occupiedDictionary.ContainsKey(coordinate + v))
            {
                return true;
            }
        }
        return false;

    }

    private Camera cam;
    public void newRoom(Vector2 coord)
    {
        GameObject tileObj = Instantiate(building, coord.IsoCoordToWorldPosition(), Quaternion.identity, transform);
        tileObj.name = $"{coord} construction";
        OccupiedSpace temp = tileObj.GetComponent<OccupiedSpace>();
        temp.coord = coord;
        temp.rM = this;
        temp.preExistingAdjPaths = AdjacentPaths(coord);
        //Debug.LogWarning(coord + "ADDED");
        occupiedDictionary.Add(coord, temp);
        temp.init();
    }    
    public void newInitialRoom(Vector2 coord)
    {
        GameObject tileObj = Instantiate(building, coord.IsoCoordToWorldPosition(), Quaternion.identity, transform);
        tileObj.name = $"{coord} construction";
        OccupiedSpace temp = tileObj.GetComponent<OccupiedSpace>();
        temp.coord = coord;
        temp.rM = this;
        temp.preExistingAdjPaths = AdjacentPaths(coord);
        //Debug.LogWarning(coord + "ADDED");
        occupiedDictionary.Add(coord, temp);
        temp.init();
        temp.InitialPathOnly();
    }
    struct Room
    {
       public businessTypes type; 
    }

    void Start()
    {
        cam = Camera.main;

        StartCoroutine(UpdateEverySecond());
        StartCoroutine(spawnNewWorkers());
    }

    void HandleTap(Vector2 touchPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        Vector2 clickedTile = coordToTileCenterPos(worldPosition).WorldToIsoCoord();

        if (checkEmpty(clickedTile))
        {
            //empty
            if (draft.coord == clickedTile)
            {
                TryBuild(clickedTile);
            }
            else
            {
                draft.moveDraft(clickedTile);
            }
        }
        else
        {
            //not empty
            draft.draftVisibility(false);


            if (businesses.ContainsKey(clickedTile))
            {
                cM.CenterCameraOnPosition(clickedTile.IsoCoordToWorldPosition());
                if (UHWM.selected.Count > 0)
                {
                    UHWM.TryDesignateSelectedWorkers(businesses[clickedTile]);
                    businesses[clickedTile].UpdateWorkerUI();
                }
                if (!occupiedDictionary[clickedTile].uiOpen)
                {
                    updateInteractWindows(clickedTile);
                }
                else
                {
                    //Debug.Log(333);
                    updateInteractWindows(clickedTile);
                    //hideInteractWindows();
                }
            }
            else
            {
                //Debug.LogError("businesses doesnt contain def for this coord");

            }
        }
    }


    // Update is called once per frame
    Vector2 touchStartPos;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                Vector2 touchEndPos = touch.position;
                Vector2 touchDelta = touchEndPos - touchStartPos;
                Debug.Log("Magnitude" + touchDelta.magnitude);
                if (touchDelta.magnitude < /* Your desired threshold here */ 0.5f)
                {
                    HandleTap(touch.position);
                }
            }
        }



        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleTap(Input.mousePosition);
        }
        

        // Controls





        /// Phone


    }
    public bool checkEmpty(Vector2 coord)
    {
        return !occupiedDictionary.ContainsKey(coord);
    }

    public void TryBuild(Vector2 clickedTile)
    {
        if (!occupiedDictionary.ContainsKey(clickedTile))
        {
            if (checkAdjacentHasPath(clickedTile) == true)
            {
                if(wallet.trySpend(false, roomCost[0]) == true)
                {
                    newRoom(clickedTile);
                    draft.draftVisibility(false);
                }
                else
                {
                    draft.flashRed();
                }
            }
            else
            {
                draft.flashRed();
            }
        }
        else
        {
            draft.flashRed();
        }
    }

    public void increaseCost(businessTypes bT)
    {
        roomCost[(int)bT] = (int)(roomCost[(int)bT] * 1.2f);
        
    }
       

    public Vector2 coordToTileCenterPos(Vector2 tapWorld)
    {
        /*Vector2 tapWorld = cam.ScreenToWorldPoint(tapPos);*/

        Vector2 r = tapWorld;

        r.x = Mathf.Round(tapWorld.x);
        r.y = Mathf.Round(tapWorld.y * 2) / 2;

        if ((r.x % 2 == 0 && r.y % 1 == 0) || (Mathf.Abs(r.x) % 2 == 1 && Mathf.Abs(r.y) % 1 == 0.5))
        {
            return r;
        }
        else
        {
            //it is not a centre point
            Vector2 d = tapWorld - r;

            Vector2 nm = Mathf.Abs(d.x) / 2 >= Mathf.Abs(d.y) ? Vector2.right : Vector2.up * 0.5f;
            d *= nm;
            //nd /= 2;

            Vector2 sol = tapWorld + d.normalized * new Vector2(1, 0.5f);
            sol.x = Mathf.Round(sol.x);
            sol.y = Mathf.Round(sol.y * 2) / 2;
            return sol;
        }
        
    }
    public Vector2[] allAdjPaths(Vector2 coord)
    {
        List<Vector2> temp = new List<Vector2>();
        Vector2[] Adj = { Vector2.left, Vector2.down, Vector2.right, Vector2.up };
        foreach (Vector2 v in Adj)
        {
            if (pathDictionary.ContainsKey(coord + v))
            {
                temp.Add(coord + v);
            }
        }
        return temp.ToArray();
    }

    public void AddBusiness(Business b, Vector2 coord)
    {
        if (!businesses.ContainsKey(coord))
        {
            businesses.Add(coord, b);
        }
    }

    public void hideInteractWindows()
    {
        updateInteractWindows(Vector2.positiveInfinity);
    }
    public void updateInteractWindows(Vector2 clickedTile)
    {
        Debug.LogWarning($" to {clickedTile} from {currentlyOpenedInteractWindow}");
        if (currentlyOpenedInteractWindow == clickedTile && clickedTile != Vector2.one*-1)
        {
            Debug.Log("closing it ");
            currentlyOpenedInteractWindow = Vector2.one * -1;
            businesses[clickedTile].Interact(false);
        }
        else
        {
            if (currentlyOpenedInteractWindow != Vector2.one * -1)
            {
                Debug.Log($"trying to close at {currentlyOpenedInteractWindow}");
                businesses[currentlyOpenedInteractWindow].Interact(false);
            }
            currentlyOpenedInteractWindow = clickedTile;
            businesses[clickedTile].UpdateWorkerUI();
            businesses[clickedTile].Interact(true);
            businesses[clickedTile].UpdateWorkerUI();
        }
    }
    public void LoadToCurrent(ProgressData data)
    {
        List<ConstructionTimePacketData> packets = data.currentConstructions;

        data.allOccupiedSpaces.Sort((x, y) => x.pathFromEntrance.Count.CompareTo(y.pathFromEntrance.Count));

        foreach(tileInfo tI in data.allOccupiedSpaces)
        {
            if (!packets.ContainsTile(tI, out ConstructionTimePacketData packet))
            {
                loadRoom(tI);
            }
            else
            {
                loadRoomConstruction(tI, packet);
            }
        }

    }

    void loadRoomConstruction(tileInfo _tileInfo,ConstructionTimePacketData cTPD)
    {
        Debug.LogWarning("Loading construction");
        Vector2 coord = _tileInfo.coord.FloatArrayToVector();

        GameObject tileObj = Instantiate(building, coord.IsoCoordToWorldPosition(), Quaternion.identity, transform);
        tileObj.name = $"{coord} construction";
        OccupiedSpace temp = tileObj.GetComponent<OccupiedSpace>();
        temp.coord = coord;
        temp.rM = this;
        temp.preExistingAdjPaths = AdjacentPaths(coord);
        occupiedDictionary.Add(coord, temp);
        temp.init();

        temp.currentRoomHighlight = _tileInfo.businessType;
        temp.LoadConstructionProcess(_tileInfo,cTPD);


    }


    public void loadRoom(tileInfo _tileInfo)
    {
        Vector2 coord = _tileInfo.coord.FloatArrayToVector();

        GameObject tileObj = Instantiate(building, coord.IsoCoordToWorldPosition(), Quaternion.identity, transform);
        tileObj.name = $"{coord} construction";
        OccupiedSpace temp = tileObj.GetComponent<OccupiedSpace>();
        temp.coord = coord;
        temp.rM = this;
        temp.preExistingAdjPaths = AdjacentPaths(coord);
        temp.preExistingAdjPaths = _tileInfo.preExistingTiles.FloatArrayToVector().ToArray();
        occupiedDictionary.Add(coord, temp);
        temp.init();

        temp.currentRoomHighlight = _tileInfo.businessType;
        temp.LoadConstruction(_tileInfo);




        


    }
    public int brandingStatus = 0;
    public void callIconSwitch1()
    {
        brandingStatus = 1;
        foreach (Vector2 v in businesses.Keys)
        {
            businesses[v].visualPositions.switchOne();
        }
    }
    public void callIconSwitch2()
    {
        brandingStatus = 2;
        foreach (Vector2 v in businesses.Keys)
        {
            businesses[v].visualPositions.switchTwo();
        }
    }

}


