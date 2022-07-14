using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Unit : MonoBehaviour
{

    //PlayerSO info
    public string playerID;
    public string playerName;
    private Transform PlayerModel;
    public bool isNpc;
    private float moveSpeed = 0.3f;
    private bool isActing;
    public int playersAvaliableSteps = 0;
    public int SpecialDiceCounter = 2;
    public void Init(string _playerID,string _playerName,Transform _modelTransform,bool _isNPC)
    {
        this.playerID = _playerID;
        this.playerName = _playerName;
        this.PlayerModel = _modelTransform;
        this.isNpc = _isNPC;
    }

    #region PUBLIC FUNCTIONS
    public void OnStepFinish(bool isRewind)
    {

        GridObject gridObject = LevelManager.instance.grid.GetGridObject((int)this.transform.localPosition.x, (int)this.transform.localPosition.z);
        if (gridObject == null) { FallInSpace(); return; }

        // Debug.Log($"Im on: {gridObject.GetPlate().floorType}  x: {gridObject.x} y: {gridObject.y}");



        switch (gridObject.GetPlate().floorType)
        {

            case Tools.FloorType.FINISH:
                Finish();
                break;
            case Tools.FloorType.EMPTY:
                FallInSpace();
                break;
            case Tools.FloorType.WALKABLE:
            case Tools.FloorType.START:
                gridObject.GetPlate().ToggleColor(Color.red, isRewind);
                SpecialEventManager.instance.ClaimSpecialEvent(this, gridObject.GetPlate());
                break;


        }
    }

    internal void ResetSpecialDice()
    {
        SpecialDiceCounter = 2;
    }

    public GridObject GetPlayersGridObject()
    {
        return LevelManager.instance.grid.GetGridObject((int)this.transform.localPosition.x, (int)this.transform.localPosition.z);
    }
    public bool CanPlayerMove(Tools.Directions direction)
    {

        Vector3 movePosTarget = direction == Tools.Directions.FORWORD ? Vector3.forward :
                    direction == Tools.Directions.BACK ? Vector3.back :
                    direction == Tools.Directions.LEFT ? Vector3.left :
                    Vector3.right;



        Vector3 finalPosition = this.transform.position + movePosTarget;
        GridObject gridObject = LevelManager.instance.grid.GetGridObject((int)finalPosition.x, (int)finalPosition.z);

        if (gridObject == null) { return false; }

        if (this.isNpc)
        {

            if (this.transform.localPosition + movePosTarget == TurnController.instance.PlayerUnit.transform.localPosition && playersAvaliableSteps == 1) return false;
        }
        else {
            if (this.transform.localPosition + movePosTarget == TurnController.instance.NpcUnit.transform.localPosition && TurnController.instance.GetAvaliableSteps() == 1) return false;
        }
       


        switch (gridObject.GetPlate().floorType)
        {


            case Tools.FloorType.NONWOKABLE:

                return false;
            case Tools.FloorType.WALKABLE:
            case Tools.FloorType.START:
                if (gridObject.GetPlate().isActivePlate)
                    return false;
                else
                    return true;
            default:
                return true;


        }
    }
    public void OnTurnChanged(Unit player)
    {
        if (isActing) return;
        if (this == player)
        {
            playersAvaliableSteps = DiceController.instance.DiceResult;
            SpecialDiceCounter = (SpecialDiceCounter <= 0) ? 0 : SpecialDiceCounter - 1;
            //BounceJump();
        }

    }

    #endregion

    #region TWEENS

    public Sequence Move(Tools.Directions direction, bool isRewind)
    {
        if (isActing)
        {
            DOTween.Kill(this, true);
        }

        isActing = true;

        Vector3 movePosTarget = direction == Tools.Directions.FORWORD ? Vector3.forward :
                    direction == Tools.Directions.BACK ? Vector3.back :
                    direction == Tools.Directions.LEFT ? Vector3.left :
                    Vector3.right;


        Vector3 rotationTarget = direction == Tools.Directions.FORWORD ? new Vector3(0, 0, 0) :
                    direction == Tools.Directions.BACK ? new Vector3(0, 180, 0) :
                    direction == Tools.Directions.LEFT ? new Vector3(0, -90, 0) :
                    direction == Tools.Directions.RIGHT ? new Vector3(0, 90, 0) :
                    new Vector3(0, 180, 0);


        Vector3 finalPosition = this.transform.position + movePosTarget;





        Unit opponent = this.playerID == TurnController.instance.PlayerUnit.playerID ? TurnController.instance.NpcUnit : TurnController.instance.PlayerUnit;
        bool isOverlapingPlayers = (finalPosition == opponent.transform.localPosition);




        if (isRewind) OnStepFinish(isRewind);

        Sequence s = DOTween.Sequence();

        s.SetId(this);
        s.Join(this.transform.DOMove(movePosTarget, moveSpeed).SetRelative().SetEase(Ease.InFlash));
        s.Join(this.transform.DOLocalRotate(rotationTarget, moveSpeed).SetEase(Ease.InFlash));
        s.Join(PlayerModel.DOLocalJump(isOverlapingPlayers ? new Vector3(0, opponent.PlayerModel.localScale.y, 0) : Vector3.zero, 1, 1, moveSpeed).SetEase(Ease.InFlash));
        s.Join(PlayerModel.DOPunchScale(new Vector3(0, 1, 0), moveSpeed, 1, .2f).SetEase(Ease.InFlash));
        s.Append(PlayerModel.DOPunchScale(new Vector3(0, -.3f, 0), moveSpeed / 2, 1, .2f).SetEase(Ease.InFlash));
        s.OnComplete(() => {

            playersAvaliableSteps = (isRewind) ? playersAvaliableSteps + 1 : playersAvaliableSteps - 1;
            
            isActing = false;
            if (!isRewind) OnStepFinish(isRewind);
        });
        return s;
    }

    public Tween FallInSpace()
    {
        Sequence s = DOTween.Sequence();
        s.SetId(this);
        s.Append(this.transform.DOLocalMoveY(-3, .3f).SetEase(Ease.Linear));
        s.Join(this.transform.DOScale(0, 0.3f).SetEase(Ease.OutSine));
        s.OnComplete(() => {
            isActing = false;
            SpawnPlayer(LevelManager.instance.GetPlayerStartPosition(this));
            TurnController.instance.ChangeTurn();
        });
        return s;
    }

    public Tween Finish()
    {

        StateManager.instance.SetState(StateManager.State.GameEnded);

        Sequence s = DOTween.Sequence();
        s.SetId(this);
        s.Join(this.transform.DOLocalMoveY(.5f, 2).SetEase(Ease.InOutSine));
        s.Join(this.PlayerModel.DOShakePosition(2, 0.05f).SetEase(Ease.InOutSine));
        s.Append(this.transform.DOLocalMoveY(2, .5f).SetEase(Ease.OutBack));
        s.Append(this.transform.DOScale(0, 0.3f).SetEase(Ease.OutFlash));
        s.OnComplete(() => {
            isActing = false;
            SpawnPlayer(LevelManager.instance.GetPlayerStartPosition(this));
            TurnController.instance.ChangeTurn();
        });

        return s;
    }

    public Tween BounceJump()
    {
        Sequence s = DOTween.Sequence();

        s.SetId(this);
        s.Join(PlayerModel.DOLocalJump( Vector3.zero, 1, 1, moveSpeed).SetEase(Ease.InFlash));
        s.OnComplete(() => {
            isActing = false;       
        });
        return s;
    }

    public Tween SpawnPlayer(Vector3 spawnPos)
    {

        Sequence s = DOTween.Sequence();
        s.SetId(this);
        s.Join(this.transform.DOLocalMove(spawnPos, 0).SetEase(Ease.InExpo).SetDelay(Random.Range(0, 2)));
        s.Join(this.transform.DOScale(1, .3f).From(0).SetEase(Ease.OutBounce));
        s.OnComplete(() => isActing = false);

        return s;
    }
    #endregion

    #region PATHFINDING MOVEMENT
    //NPC MOVEMENT
    public async void NpcMovePathFinding()
    {
        while(this.playersAvaliableSteps > 0) {


            List<GridObject> path =  TurnController.instance.GetNearestEventPath(this);

            if (path.Count==0)
            {
                Debug.Log($"Path is null");
                break;
            }

            int avaliableSteps = (TurnController.instance.GetAvaliableSteps() + 1 < path.Count)? TurnController.instance.GetAvaliableSteps() + 1 : path.Count;
            Debug.Log($"Avaliable Steps {avaliableSteps-1}");

            for (int i = 0; i < avaliableSteps; i++)
            {
                if (i == 0) continue; //this is my position
                Task movement;
                path[i]?.GetPlate().ToggleColor(color: Color.yellow, false);
                int fromX = this.GetPlayersGridObject().GetPlate().x;
                int fromY = this.GetPlayersGridObject().GetPlate().y;
                int toX = path[i].GetPlate().x;
                int toY = path[i].GetPlate().y;
                movement = Move(GetDirectionToMove(fromX, fromY, toX, toY), false).AsyncWaitForCompletion();
           
                await movement;

            }
        }

        await Task.Delay(2000);
        TurnController.instance.ChangeTurn();





    }
    public Tools.Directions GetDirectionToMove(int fromX,int fromY, int toX,int toY)
    {
        Tools.Directions direction ;
        if (fromX == toX && fromY < toY)
        {         
            direction = Tools.Directions.FORWORD;
        }
        else if (fromX == toX && fromY > toY)
        {
            direction = Tools.Directions.BACK;
        }
        else if (fromX < toX && fromY == toY)
        {
            direction = Tools.Directions.RIGHT;
        }
        else if(fromX > toX && fromY == toY)
        {
            direction = Tools.Directions.LEFT;
        }
        else {

            direction = Tools.Directions.FORWORD;
        }
        Debug.Log($"player currentPos: {fromX},{fromY} -- targetPos: {toX},{toY}  direction: {direction}");
        return direction;
    }
   
    #endregion

    private void OnEnable()
    {
        TurnController.OnTurnChanged += OnTurnChanged;
    }
    private void OnDisable()
    {
        TurnController.OnTurnChanged -= OnTurnChanged;

    }
}
