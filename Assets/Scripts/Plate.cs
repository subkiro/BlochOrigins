using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Plate : MonoBehaviour
{
    public GenericGrid<GridObject> grid;
    public int x, y;
    public string ID;
    public Tools.FloorType floorType;
    public Color OriginalColor;
    public bool isActivePlate = false;

    public void Init(FloorPlateSO floorPlateSO, GenericGrid<GridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.ID = floorPlateSO.ID;
        this.floorType = floorPlateSO.floorType;
        OriginalColor = GetPlateColor();
        

    }



    public void ToggleColor(Color color, bool isRewin) {

        


        if (isRewin)
        {
            SetPlateColor(OriginalColor);
            isActivePlate = false;
        }
        else {
            SetPlateColor(color);
            isActivePlate = true;
            

        }
            

        

    }

    public void Reset()
    {
        SetPlateColor(OriginalColor);
        isActivePlate = false;
    }

    public Color GetPlateColor() {

        if (TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
        {

            if (mesh.materials.Length >= 2)
                return mesh.materials[1].color;
            else
                return mesh.material.color;
        }

        return Color.white;
    }


    public void SetPlateColor(Color color,float speed = 1f)
    {
        
        if (TryGetComponent<MeshRenderer>(out MeshRenderer mesh))
        {

            if (mesh.materials.Length >= 2)
              
               mesh.materials[1].DOColor(color,speed);
            else
               mesh.material.DOColor(color, speed);
        }

        
    }


    public void OnStateChanged(StateManager.State state) {
        Reset();
    }

    private void OnEnable()
    {
        StateManager.OnStateChanged += OnStateChanged;
    }
    private void OnDisable()
    {
        StateManager.OnStateChanged -= OnStateChanged;
    }
}
