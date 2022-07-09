using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BlochOrigins/FloorPlateObj")]
public class FloorPlateSO : ScriptableObject
{
    public string ID;
    public GameObject model;
    public Tools.FloorType floorType;
    public GameObject DebuText;


    public Plate CreatePlate(GenericGrid<GridObject> grid,int x, int y) {
        GameObject tmp = Instantiate(model);
      
        Plate plate = tmp.AddComponent<Plate>();
        tmp.transform.SetParent(LevelManager.instance.FloorContainer);
        tmp.transform.localPosition = new Vector3(x, 0, y);
        plate.Init(this, grid, x,y);


        GameObject debuText = Instantiate(DebuText, tmp.transform);
        debuText.transform.localPosition = new Vector3(0, 0, 0.01f);
        debuText.transform.localScale = new Vector3(0.1f, -0.1f, 0.1f);
        debuText.GetComponent<TMPro.TMP_Text>().text = $"{x},{y}";
        return plate;
    }

    

}

