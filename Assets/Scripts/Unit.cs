using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Unit : MonoBehaviour
{

    //PlayerSO info
    public string playerID;
    public string playerName;
    private Transform PlayerModel;
    public bool isNpc;
    private float moveSpeed = 0.3f;
    private bool isActing;


    public void Init(string _playerID,string _playerName,Transform _modelTransform,bool _isNPC)
    {
        this.playerID = _playerID;
        this.playerName = _playerName;
        this.PlayerModel = _modelTransform;
        this.isNpc = _isNPC;
    }

    public void Move(Tools.Directions direction,bool isRewind)
    {
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
                gridObject.GetPlate().ToggleColor(Color.red, isRewind);
                break;
           

        }
    }  

    public Tween FallInSpace() {

        

        Sequence s = DOTween.Sequence();
        s.SetId(this);
        s.Append(this.transform.DOLocalMoveY(-10, .5f).SetEase(Ease.Linear));
        s.OnComplete(() =>{SpawnPlayer(LevelManager.instance.GetPlayerStartPosition(this)); });

        TurnController.instance.ChangeTurn();
        return s;
    }

    public Tween Finish() {

        CameraManager.instance.SetCameraState(CameraManager.CameraStates.GamePlay_Zoom, this.transform);

        Sequence s = DOTween.Sequence();
        s.Join(this.transform.DOLocalMoveY(1, 2).SetEase(Ease.InOutSine));
        s.Join(this.PlayerModel.DOShakePosition(2, 0.05f).SetEase(Ease.InOutSine));
        s.Append(this.transform.DOLocalMoveY(5, .5f).SetEase(Ease.OutBack));
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
    private void OnEnable()
    {
        TurnController.OnTurnChanged += OnTurnChanged;
    }
    private void OnDisable()
    {
        TurnController.OnTurnChanged -= OnTurnChanged;

    }
}
