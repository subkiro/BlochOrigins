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

    public void Move(Tools.Directions direction)
    {
        if (isActing) return;
       

      
        
        Unit opponent = this.playerID==TurnController.instance.PlayerUnit.playerID ? TurnController.instance.NpcUnit : TurnController.instance.PlayerUnit;


        Vector3 movePosTarget = direction == Tools.Directions.FORWORD ? PlayerModel.rotation * Vector3.forward :
                    direction == Tools.Directions.BACK ? PlayerModel.rotation * Vector3.back :
                    direction == Tools.Directions.LEFT ? PlayerModel.rotation * Vector3.left :
                    PlayerModel.rotation * Vector3.right;


        Vector3 rotationTarget = direction == Tools.Directions.FORWORD ? transform.eulerAngles :
                    direction == Tools.Directions.BACK ? transform.eulerAngles + new Vector3(0, 180, 0) :
                    direction == Tools.Directions.LEFT ? transform.eulerAngles + new Vector3(0, -90, 0) :
                    direction == Tools.Directions.RIGHT ? transform.eulerAngles + new Vector3(0, 90, 0) :
                    transform.eulerAngles;


        Vector3 finalPosition = this.transform.position + movePosTarget;




        bool isOverlapingPlayers = (finalPosition == opponent.transform.localPosition);
     

        Sequence s = DOTween.Sequence();
        s.OnStart(() => isActing = true);
        s.Join(this.transform.DOMove(movePosTarget, moveSpeed).SetRelative().SetEase(Ease.InFlash));
        s.Join(this.transform.DOLocalRotate(rotationTarget, moveSpeed).SetEase(Ease.InFlash));
        s.Join(PlayerModel.DOLocalJump(isOverlapingPlayers? new Vector3(0,opponent.PlayerModel.localScale.y,0): Vector3.zero, 1, 1, moveSpeed).SetEase(Ease.InFlash));
        s.Join(PlayerModel.DOPunchScale(new Vector3(0, 1, 0), moveSpeed, 1, .2f).SetEase(Ease.InFlash));
        s.OnComplete(() => {
            
            UnitPositionInGrid();
        });


    }




    public void UnitPositionInGrid() {

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
            default:
                Bounce();
                break;

        }
    }

    public Tween FallInSpace() {

        Sequence s = DOTween.Sequence();
        s.Append(this.transform.DOLocalMoveY(-10, .5f).SetEase(Ease.Linear));
        s.OnComplete(() =>
        {
        SpawnPlayer(LevelManager.instance.GetPlayerStartPosition(this));
            TurnController.instance.ChangeTurn();
    });
        return s;
    }

    public Tween Finish() {
        Sequence s = DOTween.Sequence();
        s.Join(this.transform.DOLocalMoveY(1, 2).SetEase(Ease.InOutSine));
        s.Join(this.PlayerModel.DOShakePosition(2, 0.05f).SetEase(Ease.InOutSine));
        s.Append(this.transform.DOLocalMoveY(5, .5f).SetEase(Ease.OutBack));
        s.OnComplete(() => {
            TurnController.instance.ChangeTurn();
            SpawnPlayer(LevelManager.instance.GetPlayerStartPosition(this));
        } );

        return s;
    }

    public Tween Bounce()
    {

        Sequence s = DOTween.Sequence();
        s.Append(PlayerModel.DOPunchScale(new Vector3(0, -.3f, 0), moveSpeed / 2, 1, .2f).SetEase(Ease.InFlash));
        s.OnComplete(() => isActing = false);

        return s;
    }
   
    public Tween SpawnPlayer(Vector3 spawnPos)
    {

        Sequence s = DOTween.Sequence();
        s.Join(this.transform.DOLocalMove(spawnPos, 2).From(spawnPos + new Vector3(0, 10, 0)).SetEase(Ease.InExpo).SetDelay(Random.Range(0, 2)));
        s.Append(this.transform.DOPunchScale(new Vector3(0, -.8f, 0), 0.2f / 2, 1, .2f).SetEase(Ease.InFlash));
        s.OnComplete(() => isActing = false);

        return s;
    }



}
