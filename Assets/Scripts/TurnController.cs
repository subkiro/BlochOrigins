using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class TurnController : MonoBehaviour
{

    public static TurnController instance;

    [SerializeField] public Unit PlayerUnit;
    [SerializeField] public Unit NpcUnit;
    private ActionRecorder _actionRecorder ;
    public static UnityAction<Unit> OnTurnChanged;
    public static UnityAction<int,Unit> OnStepExecuted;


    private Unit m_currentTurnUnit;
    public int DiceResult;
    private void Awake()
    {
        instance = this;
        _actionRecorder = new ActionRecorder();

       
    }


    

    public void StartTurn(Unit player) {

        StateManager.instance.SetState(player.isNpc ? StateManager.State.NpcRound : StateManager.State.PlayerRound);

        OnTurnChanged?.Invoke(player);
        ArrowIndicator.instance.Init(player);
        _actionRecorder.Reset();

        if (player.isNpc) {
            DG.Tweening.DOVirtual.DelayedCall(1, () => StartNpcMove());
        }

    }

    public void ChangeTurn()
    {
        if (m_currentTurnUnit == null) m_currentTurnUnit = PlayerUnit; // first start
        else {
            m_currentTurnUnit = (m_currentTurnUnit.playerID == PlayerUnit.playerID) ? NpcUnit : PlayerUnit;
        }



        DiceController.instance.ThrowDice(m_currentTurnUnit, () =>
        {
            StartTurn(m_currentTurnUnit);
        });

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
                ChangeTurn();
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
      
        return counter;
    }

    public Unit GetCurrentUnit() => m_currentTurnUnit;
    public Unit GetOppositeUnit() => (m_currentTurnUnit.isNpc)? PlayerUnit: NpcUnit;
    public bool IsNpc(Unit player) {
        return (player.playerID != PlayerUnit.playerID);
    }
    public void OnMove(Tools.Directions direction)
    {
        if (m_currentTurnUnit == null) return;

        if (m_currentTurnUnit.CanPlayerMove(direction) && GetAvaliableSteps()>0) {
           
            var action = new MoveAction(m_currentTurnUnit, direction);
            _actionRecorder.Record(action);
        }
  
    }
    public void Rewind() {

        int rewindsAvaliable = (m_currentTurnUnit.isNpc) ? GameManager.instance.playerInfoNpc.Rewinds: GameManager.instance.playerInfo.Rewinds;
        if (rewindsAvaliable <= 0) return;


            _actionRecorder.Rewind();
            OnStepExecuted?.Invoke(GetActionCounterResults(), m_currentTurnUnit);
        
    }
    public int GetActionCounterResults() {
       return (_actionRecorder.GetCount());
    }


    public void OnGameEnded(StateManager.State state) {
        switch (state)
        {
            case StateManager.State.GameEnded:
                Destroy(PlayerUnit.gameObject);
                Destroy(NpcUnit.gameObject);
                break;
            
        }

    }


    private void OnEnable()
    {
        InputManager.OnMove += OnMove;
        StateManager.OnStateChanged += OnGameEnded;
    }
    private void OnDisable()
    {
        InputManager.OnMove -= OnMove;
        StateManager.OnStateChanged -= OnGameEnded;

    }
}
