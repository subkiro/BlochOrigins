using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Unit : MonoBehaviour
{

    private float moveSpeed = 0.3f;
    public bool isActing;
    public Transform PlayerModel;
    private void Awake()
    {
        PlayerModel = this.transform.GetChild(0);
    }

    public void Move(Tools.Directions direction)
    {
        if (isActing) return;
        Debug.Log("Player Moved to " + direction.ToString());

        Sequence s = DOTween.Sequence();
        s.OnStart(() => isActing = true);
        s.Join(this.transform.DOMove(
                    direction == Tools.Directions.FORWORD ? Vector3.forward :
                    direction == Tools.Directions.BACK ? Vector3.back :
                    direction == Tools.Directions.LEFT ? Vector3.left :
                    Vector3.right, moveSpeed).SetRelative().SetEase(Ease.InFlash));


        s.Join(PlayerModel.DOLocalJump(Vector3.zero,1,1, moveSpeed).SetEase(Ease.InFlash));
        s.Join(PlayerModel.DOPunchScale(new Vector3(0, 0, 1), moveSpeed, 1, .2f).SetEase(Ease.InFlash));
        s.Append(PlayerModel.DOPunchScale(new Vector3(0, 0, -.3f), moveSpeed/2, 1, .2f).SetEase(Ease.InFlash));
        s.OnComplete(() => isActing = false);
    }




}
