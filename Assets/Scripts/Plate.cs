using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public GenericGrid<GridObject> grid;
    public int x, y;
    public string ID;
    public Tools.FloorType floorType;
    

    public void Init(FloorPlateSO floorPlateSO, GenericGrid<GridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.ID = floorPlateSO.ID;
        this.floorType = floorPlateSO.floorType;

     
    }

   



}
