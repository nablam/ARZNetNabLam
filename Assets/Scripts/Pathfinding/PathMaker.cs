using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMaker : MonoBehaviour {
    float segmentDistancePF;

    public bool isFinding { get; private set; }
    List<PathNode> created;
    Queue<PathNode> toVisit;
    PathNode HEAD1;// the node representing the final destination - algorithim creates path from this point

    float bestCost;
    bool targetFound;

    GridMap grid;

    GameObject PathStart;
    GameObject PatheEnd;
    List<GridPoint> Path_P1_GPs;

    List<Vector3> ListV3_MadeByPathMaker;
    public List<Vector3> GEtPath_V3() { return ListV3_MadeByPathMaker; }
    public List<GridPoint> GetPathGps() { return Path_P1_GPs; }
    void Start()
    {

        segmentDistancePF = GameSettings.Instance.SegmentSizeMaster;
       
    }

    //we must do a criss cross here 
    public void BuildPathsToPlayerarea(GridMap arggrid, GameObject argActual_Start_Object, GameObject argActual_End_Object)
    {
        grid = arggrid;
        PathStart = grid.GetClosestGridPoint(argActual_End_Object);
        PatheEnd = grid.GetClosestGridPoint(argActual_Start_Object);

        FindBestPath();
    }

    void FindBestPath()
    {
        Path_P1_GPs = new List<GridPoint>();
        ListV3_MadeByPathMaker = new List<Vector3>();
        StartCoroutine(GetPath(PathStart, PatheEnd));
    }

    IEnumerator GetPath(GameObject startPoint, GameObject targetPoint)
    {
        HEAD1 = null;
        //Debug.Log("Finding path...");
        isFinding = true;

        if (startPoint == null)
        {
            List<GameObject> points = grid.Get_ListOfMapGridPoints_GOS();
            Vector3 transPos = new Vector3(transform.position.x, grid.gridHeight, transform.position.y);
            foreach (GameObject p in points)
            {
                if (Vector3.Distance(p.transform.position, transPos) < segmentDistancePF)
                {
                    startPoint = p;
                    break;
                }
            }
        }

        created = new List<PathNode>();
        toVisit = new Queue<PathNode>();

        // target has not been found
        bestCost = -1.0f;
        targetFound = false;

        PathNode startNode = new PathNode(startPoint, segmentDistancePF, null, 0.0f);
        created.Add(startNode);
        toVisit.Enqueue(startNode);

        int count = 0;
        while (toVisit.Count > 0)
        {
            PathNode n = toVisit.Dequeue();
            PathNode target = n.Visit(created, toVisit, startPoint, targetPoint, bestCost, targetFound);
            if (target != null)
            {
                targetFound = true;
                HEAD1 = target;
            }
            if (targetFound)
            {
                HEAD1.setGP_bottext("ENDNODE");
                HEAD1.setGP_color(Color.cyan);
                bestCost = HEAD1.cost;
            }

            count++;

            // yield to frame every 500 nodes checked
            if (count >= 30000)
            {
                count = 0;
                yield return null;
            }
        }

        TraversePAthSolution();
        if(GameSettings.Instance.IsLeanPath)
        CleanPath();

        BuildPAthV3();

    }
    void BuildPAthV3() {
        for (int x = 0; x < Path_P1_GPs.Count; x++) {
            ListV3_MadeByPathMaker.Add(Path_P1_GPs[x].gameObject.transform.position);
        }
       // Path_P1_GPs = new List<GridPoint>();

    }
    /// <summary>
    /// at this point HEAD points to the last node in the path (it should be the players'sposition)
    /// traversing this linked list back to spawnpoint and increment the map's gridpoint at that pathnode
    /// this way the next time FindPAth() is invoked, it will avoid previously chosen pnathnodes
    /// </summary>
    void TraversePAthSolution()
    {
        Path_P1_GPs = new List<GridPoint>();
        PathNode currentNode = HEAD1;
        if (currentNode != null)
            while (currentNode.previousNode != null)
            {
                currentNode.setGP_bottext("p1");
                Path_P1_GPs.Add(currentNode.gridPointObj.GetComponent<GridPoint>());
                currentNode = currentNode.previousNode;
                currentNode.GetGPofThisNoe().Increment_TimesUsedInAfinalPath();        //////////////////--> is where a node on path1 is denoted;

            }
        Path_P1_GPs.Add(PathStart.GetComponent<GridPoint>());
        PathStart.GetComponent<GridPoint>().Increment_TimesUsedInAfinalPath();
    }
    void CleanPath()
    {
        List<int> Indextodelete = new List<int>();

        GridPoint Onegp = Path_P1_GPs[0];
        ////figureout direction of previous
        GridPoint TwoGP = Path_P1_GPs[1];
        ////figureout dirrection of prev 

        int firstDirection = Figureoutdirectin(Onegp, TwoGP);
       // saydirection(firstDirection);
        for (int x = 1; x < Path_P1_GPs.Count - 1; x++)
        {
            int newdirection = Figureoutdirectin(Path_P1_GPs[x], Path_P1_GPs[x + 1]);
           // saydirection(newdirection);
            if (firstDirection == newdirection)
            {
                Indextodelete.Add(x);
                continue;
            }
            firstDirection = newdirection;
        }

        if (GameSettings.Instance.IsLeanPath)
            for (int i = Indextodelete.Count - 1; i >= 0; i--)
            {
                Path_P1_GPs.RemoveAt(Indextodelete[i]);
            }
    }

    void saydirection(int x)
    {
        if (x == 0) { Debug.Log("going Forward"); }
        else
            if (x == 1) { Debug.Log("going righ"); }
        else
            if (x == 2) { Debug.Log("going down"); }
        else
            if (x == 3) { Debug.Log("going left"); }

    }
    //0 forward
    //1 right
    //2 back
    //3 left
    int Figureoutdirectin(GridPoint argOne, GridPoint argTwo)
    {
        int direction = 0;
        if (argTwo.gameObject == argOne.forwardGameObject) { direction = 0; }
        if (argTwo.gameObject == argOne.rightGameObject) { direction = 1; }
        if (argTwo.gameObject == argOne.backGameObject) { direction = 2; }
        if (argTwo.gameObject == argOne.leftGameObject) { direction = 3; }
        return direction;
    }


}
