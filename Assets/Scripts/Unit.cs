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
    public int movementCounter = 0;

    

    public void Init(string _playerID,string _playerName,Transform _modelTransform,bool _isNPC)
    {
        this.playerID = _playerID;
        this.playerName = _playerName;
        this.PlayerModel = _modelTransform;
        this.isNpc = _isNPC;
    }

    public Task Move(Tools.Directions direction,bool isRewind)
    {
        Debug.Log(direction);
        if (isActing) {
           DOTween.Kill(this, true);
          
        }

       
       
    

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
        s.OnStart(() => isActing = true);
        s.Join(this.transform.DOMove(movePosTarget, moveSpeed).SetRelative().SetEase(Ease.InFlash));
        s.Join(this.transform.DOLocalRotate(rotationTarget, moveSpeed).SetEase(Ease.InFlash));
        s.Join(PlayerModel.DOLocalJump(isOverlapingPlayers ? new Vector3(0, opponent.PlayerModel.localScale.y, 0) : Vector3.zero, 1, 1, moveSpeed).SetEase(Ease.InFlash));
        s.Join(PlayerModel.DOPunchScale(new Vector3(0, 1, 0), moveSpeed, 1, .2f).SetEase(Ease.InFlash));
        s.OnComplete(() => {
            if(!isRewind) OnStepFinish(isRewind);
        });
        Task task = s.AsyncWaitForCompletion();

        return task;
    }

    public void OnStepFinish(bool isRewind) {

        GridObject gridObject =   LevelManager.instance.grid.GetGridObject((int)this.transform.localPosition.x, (int)this.transform.localPosition.z);
        if (gridObject == null) { FallInSpace(); return; }
      
        Debug.Log($"Im on: {gridObject.GetPlate().floorType}  x: {gridObject.x} y: {gridObject.y}");
        


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
                break;
           

        }
    }  

    public Tween FallInSpace() {

        

        Sequence s = DOTween.Sequence();
        s.SetId(this);
        s.Append(this.transform.DOLocalMoveY(-3, .3f).SetEase(Ease.Linear));
        s.Join(this.transform.DOScale(0, 0.3f).SetEase(Ease.OutSine));
        s.OnComplete(() =>{
            TurnController.instance.ChangeTurn();
            SpawnPlayer(LevelManager.instance.GetPlayerStartPosition(this));
           
        });

        

        return s;
    }

    public Tween Finish() {

        StateManager.instance.SetState(StateManager.State.GameEnded);

        Sequence s = DOTween.Sequence();
        s.Join(this.transform.DOLocalMoveY(.5f, 2).SetEase(Ease.InOutSine));
        s.Join(this.PlayerModel.DOShakePosition(2, 0.05f).SetEase(Ease.InOutSine));
        s.Append(this.transform.DOLocalMoveY(2, .5f).SetEase(Ease.OutBack));
        s.Append(this.transform.DOScale(0, 0.3f).SetEase(Ease.OutFlash));
        s.OnComplete(() => {
            TurnController.instance.ChangeTurn();
            SpawnPlayer(LevelManager.instance.GetPlayerStartPosition(this));

        });

        return s;
    }

    public Tween Bounce()
    {

        Sequence s = DOTween.Sequence();
        s.SetId(this);
        s.Append(PlayerModel.DOPunchScale(new Vector3(0, -.3f, 0), moveSpeed / 2, 1, .2f).SetEase(Ease.InFlash));
        s.OnComplete(() => isActing = false);

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

    public GridObject GetPlayersGridObject() {
        return LevelManager.instance.grid.GetGridObject((int)this.transform.localPosition.x, (int)this.transform.localPosition.z);
    }
    public bool CanPlayerMove(Tools.Directions direction) {

        Vector3 movePosTarget = direction == Tools.Directions.FORWORD ? Vector3.forward :
                    direction == Tools.Directions.BACK ? Vector3.back :
                    direction == Tools.Directions.LEFT ? Vector3.left :
                    Vector3.right;



        Vector3 finalPosition = this.transform.position + movePosTarget;
        GridObject gridObject = LevelManager.instance.grid.GetGridObject((int) finalPosition.x,(int) finalPosition.z);

        if (gridObject == null) {  return false; }

        Debug.Log($"Im on: {gridObject.GetPlate().floorType}  x: {gridObject.x} y: {gridObject.y}");


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
    public void OnTurnChanged(Unit player) {
        if (isActing) return;
        if (this == player) {
            Bounce();
        }
        
    }








    //NPC MOVEMENT
    public async void NpcMovePathFinding(Plate moveToPlate)
    {
        List<GridObject> path = LevelManager.instance.FindPath(this.GetPlayersGridObject().x, this.GetPlayersGridObject().y, moveToPlate.x, moveToPlate.y);
        if (path == null)
        {
            Debug.Log($"Path is null");
            return;
        }


        for (int i = 1; i < path.Count; i++)
        {
            path[i]?.GetPlate().ToggleColor(color: Color.yellow, false);
            int fromX = this.GetPlayersGridObject().GetPlate().x;
            int fromY = this.GetPlayersGridObject().GetPlate().y;
            int toX = path[i].GetPlate().x;
            int toY = path[i].GetPlate().y;
            await Move(GetDirectionToMove(fromX, fromY, toX, toY), false);
        }
        

       


    }

    public Tools.Directions GetDirectionToMove(int fromX,int fromY, int toX,int toY)
    {
        Debug.Log($"player currentPos: {fromX},{fromY} -- targetPos: {toX},{toY}");

        if (fromX == toX && fromY < toY)
        {         
            return Tools.Directions.FORWORD;
        }
        if (fromX == toX && fromY < toY)
        {
            return Tools.Directions.RIGHT;
        }
        if (fromX < toX && fromY == toY)
        {
            return Tools.Directions.BACK;
        }
        if (fromX > toX && fromY == toY)
        {
            return Tools.Directions.LEFT;
        }

        return Tools.Directions.FORWORD;
    }









    private void OnEnable()
    {
        TurnController.OnTurnChanged += OnTurnChanged;
    }
    private void OnDisable()
    {
        TurnController.OnTurnChanged -= OnTurnChanged;

    }
}
