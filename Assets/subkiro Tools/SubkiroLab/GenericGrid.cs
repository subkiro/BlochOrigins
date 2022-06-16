using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericGrid<TGridObject>
{
    public int width = 5;
    public int height = 5;
    public float cellSizeX,cellSizeY;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;

   




    //constuctor
    public GenericGrid(int width, int height, float cellSizeX,float cellSizeY, Vector3 originPosition, Func<GenericGrid<TGridObject>,int,int,TGridObject> createGridObject, bool ShowGrid = true)
    {
        this.width = width;
        this.height = height;
        this.cellSizeX = cellSizeX;
        this.cellSizeY = cellSizeY;
        this.originPosition = originPosition;
        gridArray = new TGridObject[this.width, this.height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this,x,y);
            }
        }

    }

   
    public void SetGridObject(int x, int y, TGridObject value) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            gridArray[x, y] = value;
           
           
        }
       
    }

    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x* cellSizeX, y* cellSizeX)  + originPosition;
    }
    
    
    private void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x  / cellSizeX);
        y = Mathf.FloorToInt((worldPosition - originPosition).y/ cellSizeY);
    }
    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);

    }


    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];

        }
        else {
            return default(TGridObject);
        }
    }


    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
}
