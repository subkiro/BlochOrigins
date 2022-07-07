using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BlochOrigins/FloorPlateObj")]
public class FloorPlateSO : ScriptableObject
{
    public string ID;
    public GameObject model;
    public Tools.FloorType floorType;


    public Plate CreatePlate(GenericGrid<GridObject> grid,int x, int y) {
        GameObject tmp = Instantiate(model);
        
        Plate plate = tmp.AddComponent<Plate>();

        tmp.transform.SetParent(LevelManager.instance.FloorContainer);
        tmp.transform.localPosition = new Vector3(x, 0, y);
        plate.Init(this, grid, x,y); 

        return plate;
    }

    

}

