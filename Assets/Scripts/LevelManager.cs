using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public  class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GenericGrid<GridObject> grid;
    public ListOfObjects listOfObjects;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //grid = new GenericGrid<GridObject>(10, 10,10, 10, Vector3.zero, (GenericGrid<GridObject> g, int x, int y) => new GridObject(g,x,y));y
        grid = CreateGrid(listOfObjects.AllLevels[0]);
        FillGrid(grid, listOfObjects.AllLevels[0]);

    }


    public GenericGrid<GridObject>  CreateGrid(LevelSO level)
    {

        int x = level.X;
        int y = level.Y;

        GenericGrid<GridObject> tmpGrid = new GenericGrid<GridObject>(x, y, 10f, 10f, new Vector3(-x / 2, -y / 2, 0), (GenericGrid<GridObject> g, int x, int y) => new GridObject(g, x, y));
        Debug.Log($"X: {tmpGrid.width}, Y: {tmpGrid.height}");
        return tmpGrid;
    }
    public void FillGrid(GenericGrid<GridObject> grid, LevelSO level)
    {

        
        char[,] lvl = level.GetLevelArray();
        Debug.Log($"lvl array X: {lvl.GetLength(0)}");

        List<FloorPlateSO> plates = listOfObjects.AllPlates;



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

    }

}

public class GridObject
{
    GenericGrid<GridObject> grid;
    int x, y;
    Plate floorPlate;


    public void SetPlate(FloorPlateSO plateSO) {
        floorPlate = plateSO.CreatePlate(grid, x,y);
    }


    public GridObject(GenericGrid<GridObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

   


  

}
