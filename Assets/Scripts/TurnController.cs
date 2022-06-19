using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{

    public static TurnController instance;

    [SerializeField] public Unit PlayerUnit;
    [SerializeField] public Unit NpcUnit;

    private void Awake()
    {
        instance = this;
    }

    public void Init(Unit _PlayerUnit, Unit _NpcUnit)
    {
        PlayerUnit = _PlayerUnit;
        NpcUnit = _NpcUnit;
    }

    public void OnMove(Tools.Directions direction)
    {
        PlayerUnit?.Move(direction);
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
