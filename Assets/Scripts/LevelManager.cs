using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;

public  class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GenericGrid<GridObject> grid;
    public Transform FloorContainer;
    private LevelSO m_CurrentLevel;
    private List<FloorPlateSO> m_AllPlates;



    //PathFinding
    private const int MOVE_STRAIGHT_COST = 10;

    private void Awake()
    {
        instance = this;
        DOTween.SetTweensCapacity(500, 50);
    }
    // Start is called before the first frame update
    public void GenerateLevel(LevelSO level,List<FloorPlateSO> AllPlates)
    {
        m_CurrentLevel = level;
        m_AllPlates = AllPlates;

        grid = CreateGrid(level);
        grid = FillGrid(grid, level);
        AnimateGirid(grid);

    }


    public GenericGrid<GridObject>  CreateGrid(LevelSO level)
    {

        int x = level.X;
        int y = level.Y;

        GenericGrid<GridObject> tmpGrid = new GenericGrid<GridObject>(x, y, 10f, 10f, new Vector3(-x / 2, -y / 2, 0), (GenericGrid<GridObject> g, int x, int y) => new GridObject(g, x, y));
        Debug.Log($"X: {tmpGrid.GridWidh()}, Y: {tmpGrid.GridHeight()}");
        return tmpGrid;
    }
    public GenericGrid<GridObject> FillGrid(GenericGrid<GridObject> grid, LevelSO level)
    {

        
        char[,] lvl = level.GetLevelArray();
        Debug.Log($"lvl array X: {lvl.GetLength(0)}");

        List<FloorPlateSO> plates = m_AllPlates;



        for (int x = 0; x < level.X; x++)
        {
            for (int y = 0; y < level.Y; y++)
            {
                char plateID = lvl[x,y];
                Debug.Log(plateID);

                FloorPlateSO plateSO = plates.FirstOrDefault(x => x.ID == plateID.ToString());
                grid.GetGridObject(x, y).SetPlate(plateSO);
                
            }
        }
        return grid;
    }
    public void AnimateGirid(GenericGrid<GridObject> grid) {

        for (int x = 0; x < grid.GridWidh(); x++)
        {
            for (int y = 0; y < grid.GridHeight(); y++)
            {

                float delayValue = (2 * x + y)*.1f;
                Plate floorPlate = grid.GetGridObject(x, y).GetPlate();
                if (floorPlate.floorType == Tools.FloorType.EMPTY) continue;

                Color brown = floorPlate.GetComponent<MeshRenderer>().materials[0].color;
                Color green = floorPlate.GetComponent<MeshRenderer>().materials[1].color;
               // floorPlate.GetComponent<MeshRenderer>().materials[0].DOColor(brown, Random.Range(.5f, 1f)).From(Color.white);
              //  floorPlate.GetComponent<MeshRenderer>().materials[1].DOColor(green, Random.Range(.2f, 1f)).From(Color.white).SetDelay(1);
               // floorPlate.transform.DOMoveY(0, Random.Range(1, 2)).From(10).SetEase(Ease.OutBounce).SetDelay(delayValue);

            }
        }

    }
    public Vector3 GetPlayerStartPosition(Unit player) {
        return m_CurrentLevel.GetPlatePositionByID(player.isNpc ? 'N' : 'S');
    }




    private List<GridObject> OpenList, CloseList;
    public List<GridObject> FindPath(int startX, int startY, int endX, int endY) {


        Debug.Log($"Grid Width: {grid.GridWidh()}, grid Height: {grid.GridHeight()}");


        GridObject startNode = grid.GetGridObject(startX,startY);
        GridObject endNode = grid.GetGridObject(endX, endY);

        Debug.Log($"startNode : {startNode.x},{startNode.y}");
        Debug.Log($"endNode : {endNode.x},{endNode.y}");

        startNode.GetPlate().SetPlateColor(color: Color.black);
        endNode.GetPlate().SetPlateColor(color: Color.red);

        OpenList = new List<GridObject> { startNode };
        CloseList = new List<GridObject> ();


        for (int x = 0; x < grid.GridWidh(); x++)
        {
            for (int y = 0; y < grid.GridHeight(); y++)
            {
                GridObject gridObject = grid.GetGridObject(x, y);
                gridObject.gCost = int.MaxValue;
                gridObject.CalculateFCost();
                gridObject.cameFrom = null;
            }
        }



        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();


        while (OpenList.Count > 0) {
            GridObject currenNode = GetLowestFCostNode(OpenList);
            if (currenNode == endNode) {

                //reach final node
                return CalculatedPath(endNode);
            }

            OpenList.Remove(currenNode);
            CloseList.Add(currenNode);

            foreach (var neighbourNode in GetNeighboursList(currenNode))
            {
                if (CloseList.Contains(neighbourNode)) continue;


                switch (neighbourNode.GetPlate().floorType)
                {

                    case Tools.FloorType.NONWOKABLE:
                    case Tools.FloorType.EMPTY:
                        Debug.Log($"Nonwokable : {neighbourNode.GetPlate().x},{neighbourNode.GetPlate().y}");
                        CloseList.Add(neighbourNode); 
                        continue;
                }


                if (neighbourNode.GetPlate().floorType != Tools.FloorType.WALKABLE)
                {
                   
                }
                int tentativeGCost = currenNode.gCost + CalculateDistanceCost(neighbourNode, endNode);
                if (tentativeGCost < neighbourNode.gCost) {
                    neighbourNode.cameFrom = currenNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!OpenList.Contains(neighbourNode)) {
                        OpenList.Add(neighbourNode);
                        Debug.Log($"OpenLists : {neighbourNode.GetPlate().x},{neighbourNode.GetPlate().y}");
                    }
                }
            }

        }

        //here we seacheche hole map and find NOTHING
        Debug.Log("NOTHING FOUND");
        return null;

    }
    private List<GridObject> GetNeighboursList(GridObject currnetNode) {
        List<GridObject> neighbourList = new List<GridObject>();

        if (currnetNode.x - 1 >= 0) {
            //left
            neighbourList.Add(grid.GetGridObject(currnetNode.x - 1, currnetNode.y));
        }
        if (currnetNode.x + 1 < grid.GridWidh()) {
            //Right
            neighbourList.Add(grid.GetGridObject(currnetNode.x + 1, currnetNode.y));
        
        }
        if (currnetNode.y -1 >=0)
        {
            //Down
            neighbourList.Add(grid.GetGridObject(currnetNode.x , currnetNode.y - 1));

        }

        if (currnetNode.y + 1 < grid.GridHeight())
        {
            //Down
            neighbourList.Add(grid.GetGridObject(currnetNode.x, currnetNode.y + 1));

        }

        return neighbourList;
    }
    private List<GridObject> CalculatedPath(GridObject endNode)
    {
       List<GridObject> path = new List<GridObject> ();
       path.Add(endNode);
        GridObject currentNode = endNode;
        while (currentNode.cameFrom!=null)
        {
            path.Add(currentNode.cameFrom);
            currentNode = currentNode.cameFrom;
        }

        path.Reverse();
        return path;
    }
    private int CalculateDistanceCost(GridObject a, GridObject b) {
        int xDistance = (int) MathF.Abs(a.x - b.x);
        int yDistance = (int) MathF.Abs(a.y - b.y);
        int remaining = (int) MathF.Abs(xDistance - yDistance);

        return MOVE_STRAIGHT_COST * remaining;
    }
    private GridObject GetLowestFCostNode(List<GridObject> pathNodeList) {
        GridObject lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) { 
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }

}


public class GridObject 
{
    GenericGrid<GridObject> grid;
    public int x, y;
    Plate floorPlate;


    public void SetPlate(FloorPlateSO plateSO) {
        floorPlate = plateSO.CreatePlate(grid, x,y);
        
    }
    public Plate GetPlate() => floorPlate;

   

    public GridObject(GenericGrid<GridObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }



    //Path Finding 

    public int gCost, hCost, fCost;
    public GridObject cameFrom;

    internal void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    
}
