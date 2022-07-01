using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnController : MonoBehaviour
{

    public static TurnController instance;

    [SerializeField] public Unit PlayerUnit;
    [SerializeField] public Unit NpcUnit;
    [SerializeField] private ActionRecorder _actionRecorder ;

    public static UnityAction<Unit> OnTurnChanged;
    private Unit m_currentTurnUnit;

    private void Awake()
    {
        instance = this;
        _actionRecorder = new ActionRecorder();
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

        var action = new MoveAction(m_currentTurnUnit,direction);
        _actionRecorder.Record(action);
       
    }


    public void Rewind() {
        _actionRecorder.Rewind();
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
