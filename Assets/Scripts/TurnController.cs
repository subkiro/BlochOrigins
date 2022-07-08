using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnController : MonoBehaviour
{

    public static TurnController instance;

    [SerializeField] public Unit PlayerUnit;
    [SerializeField] public Unit NpcUnit;
    [SerializeField] GameObject ArrowControler;
     private ActionRecorder _actionRecorder ;
    public static UnityAction<Unit> OnTurnChanged;


    private Unit m_currentTurnUnit;

    private void Awake()
    {
        instance = this;
        _actionRecorder = new ActionRecorder();
        Instantiate(ArrowControler);
    }

    public void SetTurn(Unit player) {

      
        m_currentTurnUnit = player;
        DiceController.instance.ThrowDice(player);

        OnTurnChanged?.Invoke(m_currentTurnUnit);
        ArrowIndicator.instance.Init(m_currentTurnUnit);
        _actionRecorder.Reset();

    }
    public void ChangeTurn()
    {
        m_currentTurnUnit = (m_currentTurnUnit.playerID == PlayerUnit.playerID) ? NpcUnit:PlayerUnit;

        SetTurn(m_currentTurnUnit);

    }
    public void Init(Unit _PlayerUnit, Unit _NpcUnit)
    {
        
        PlayerUnit = _PlayerUnit;
        NpcUnit = _NpcUnit;

    }


    public Unit GetCurrentUnit() => m_currentTurnUnit;
    public bool IsNpc(Unit player) {
        return (player.playerID != PlayerUnit.playerID);
    }
    public void OnMove(Tools.Directions direction)
    {
        if (m_currentTurnUnit.CanPlayerMove(direction)) {
            var action = new MoveAction(m_currentTurnUnit, direction);
            _actionRecorder.Record(action);
        }
       
       
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
