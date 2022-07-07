using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DiceController : MonoBehaviour
{
    public GameObject NormalDice;
    public GameObject SpecialDice;
    public GameObject CristalEffect;


    public static DiceController instance;
    private void Awake()
    {
        instance = this;
    }

    public void SpawnCristal(Unit player) {
        StateManager.instance.SetState(StateManager.State.Dice);

        this.transform.DOMove(player.transform.position+new Vector3(0,3,0), 0);
        GameObject temp = Instantiate(CristalEffect, this.transform);

        temp.transform.DOScale(0, 0.3f).SetEase(Ease.OutBack).SetDelay(3).OnComplete(() => {
            Destroy(temp);
            StateManager.instance.SetState((player.playerID == TurnController.instance.PlayerUnit.playerID) ? StateManager.State.PlayerRound : StateManager.State.NpcRound);

        });
        
    }

    public void OpenDice() {


    }
}
