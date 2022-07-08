using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class DiceController : MonoBehaviour
{
    public GameObject NormalDice;
    public GameObject SpecialDice;
    public GameObject CristalEffect;
    public Camera UICamera;
    public TMP_Text DiceNumber;
    public static DiceController instance;
    private void Awake()
    {
        instance = this;
    }

    public void SpawnCristal(Unit player) {
        StateManager.instance.SetState(StateManager.State.Dice);

        this.transform.DOMove(player.transform.position+new Vector3(0,3,0), 0);
        GameObject temp = Instantiate(CristalEffect, this.transform);
        GameObject dice = Instantiate(NormalDice, this.transform);
        dice.transform.localScale = Vector3.zero;
        DiceNumber.DOScale(0, 0); ;
        Sequence s = DOTween.Sequence();

        Vector3 position = UICamera.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height/2,10));
        s.Append(DiceNumber.DOScale(0,0.5f));
        s.Append(temp.transform.DOScale(0, 0.3f).SetEase(Ease.OutBack).SetDelay(3));
        s.AppendCallback(() => Destroy(temp));
        s.Append(dice.transform.DOScale(Vector3.one, .3f));
        s.Append(dice.transform.DOShakeRotation(3, 90, 10, 90,false));
        s.Append(dice.transform.DORotate(OpenDice(), 1).SetEase(Ease.OutBack));    
        s.Append(dice.transform.DOScale(Vector3.zero, 0.3f));
        s.Append(DiceNumber.DOScale(1, 0.5f));
        s.AppendCallback(()=>Destroy(dice));
        s.AppendCallback(() => StateManager.instance.SetState((player.playerID == TurnController.instance.PlayerUnit.playerID) ? StateManager.State.PlayerRound : StateManager.State.NpcRound));

    }

    public Vector3 OpenDice() {

        int random = Random.Range(1, 6);
        DiceNumber.text = random.ToString();
        switch (random)
        {
           
            case 1:
                return new Vector3(0, 90, 180);
            case 2:
                return new Vector3(0, 90, 90);
            case 3:
                return new Vector3(0, 180, 180);
            case 4:
                return new Vector3(0, 360, 0);
            case 5:
                return new Vector3(-90, 0, 0);
            case 6:
                return new Vector3(0, 90, 0);
                default: return new Vector3(0, 0, 0);
        }
    }
}
