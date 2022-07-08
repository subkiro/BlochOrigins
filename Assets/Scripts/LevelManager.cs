using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public  class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GenericGrid<GridObject> grid;
    public Transform FloorContainer;
    private LevelSO m_CurrentLevel;
    private List<FloorPlateSO> m_AllPlates;
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
        Debug.Log($"X: {tmpGrid.width}, Y: {tmpGrid.height}");
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

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
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



}
