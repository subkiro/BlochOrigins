using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public Unit PlayerUnit;


    public void OnMove(Tools.Directions direction)
    {
        PlayerUnit.Move(direction);
    }

    private void OnEnable()
    {
        InputManager.OnMove += OnMove;
    }
    private void OnDisable()
    {
        InputManager.OnMove -= OnMove;
    }
}
