using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnController : MonoBehaviour
{

    public static TurnController instance;

    [SerializeField] public Unit PlayerUnit;
    [SerializeField] public Unit NpcUnit;

    public static UnityAction<Unit> OnTurnChanged;
    private Unit m_currentTurnUnit;

    private void Awake()
    {
        instance = this;
    }

    public void SetTurn(Unit player) {
        m_currentTurnUnit = player;
        
        OnTurnChanged?.Invoke(m_currentTurnUnit);
    }
    public void ChangeTurn()
    {
        m_currentTurnUnit = (m_currentTurnUnit.playerID==PlayerUnit.playerID)?NpcUnit:PlayerUnit;
        SetTurn(m_currentTurnUnit);
       
    }
    public void Init(Unit _PlayerUnit, Unit _NpcUnit)
    {
        PlayerUnit = _PlayerUnit;
        NpcUnit = _NpcUnit;

        SetTurn(PlayerUnit);
    }



    public void OnMove(Tools.Directions direction)
    {
        m_currentTurnUnit?.Move(direction);
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
