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
    public static UnityAction<int> OnStepExecuted;

    private Unit m_currentTurnUnit;
    public int DiceResult;
    private void Awake()
    {
        instance = this;
        _actionRecorder = new ActionRecorder();
        Instantiate(ArrowControler);
    }

    public void StartTurn(Unit player) {

        StateManager.instance.SetState(player.isNpc ? StateManager.State.NpcRound : StateManager.State.PlayerRound);
        m_currentTurnUnit = player;
        OnTurnChanged?.Invoke(m_currentTurnUnit);
        ArrowIndicator.instance.Init(m_currentTurnUnit);
        _actionRecorder.Reset();

        if (m_currentTurnUnit.isNpc) {
            StartNpcMove();
        }

    }
    public void ChangeTurn()
    {
        if (m_currentTurnUnit == null) m_currentTurnUnit = PlayerUnit; // first start
        else
        m_currentTurnUnit = (m_currentTurnUnit.playerID == PlayerUnit.playerID) ? NpcUnit:PlayerUnit;


        StateManager.instance.SetState(m_currentTurnUnit.isNpc ? StateManager.State.NpcRound : StateManager.State.PlayerRound);

        DiceController.instance.ThrowDice(m_currentTurnUnit,()=> StartTurn(m_currentTurnUnit));

    }

    public void StartNpcMove() {

        // TurnController.instance.NpcUnit.NpcMovePathFinding(TurnController.instance.PlayerUnit.GetPlayersGridObject().GetPlate());
        TurnController.instance.NpcUnit.NpcMovePathFinding();

    }

    public List<GridObject> GetNearestEventPath(Unit Player)
    {
        List<GridObject> nearestPath=new List<GridObject>();
        int stepNeeded = 1000;

        foreach (var item in SpecialEventManager.instance.SpecialEvents)
        {

            List<GridObject> path = LevelManager.instance.FindPath(Player.GetPlayersGridObject().x, Player.GetPlayersGridObject().y, item.x, item.y);
            if (path == null)
            {
                Debug.Log($"Path is null");
                return null;
            }

            if (path.Count < stepNeeded) {
                stepNeeded = path.Count;
                nearestPath = path;
            }

        }

        return nearestPath;
    }




    public void Init(Unit _PlayerUnit, Unit _NpcUnit)
    {
        
        PlayerUnit = _PlayerUnit;
        NpcUnit = _NpcUnit;

    }
    public int GetAvaliableSteps() {
        int counter = DiceController.instance.DiceResult - GetActionCounterResults();
        Debug.Log("StepsLeft: " + counter);
        return counter;
    }
    public Unit GetCurrentUnit() => m_currentTurnUnit;
    public bool IsNpc(Unit player) {
        return (player.playerID != PlayerUnit.playerID);
    }
    public void OnMove(Tools.Directions direction)
    {
        if (m_currentTurnUnit.CanPlayerMove(direction) && GetAvaliableSteps()>0) {
           
            var action = new MoveAction(m_currentTurnUnit, direction);
            _actionRecorder.Record(action);
            OnStepExecuted?.Invoke(GetActionCounterResults());
        }
  
    }
    public void Rewind() {
       

      
            _actionRecorder.Rewind();
            OnStepExecuted?.Invoke(GetActionCounterResults());
        
    }
    public int GetActionCounterResults() {

       return (_actionRecorder.GetCount());
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
