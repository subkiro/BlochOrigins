using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class FinishTurnButton : MonoBehaviour
{
    public Button Button;
    public RectTransform Container;
    private Tween tween;
    private void Start()
    {
        //init finish step button
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(FinishTurn);
    }


    public void OnStepExecuted(int stepCounter, Unit player)
    {
        if (!player.isNpc)
        {

            if (TurnController.instance.GetAvaliableSteps() == 0)
            {

                ShowFinishTurnButton(true);

            }
            else
            {
                ShowFinishTurnButton(false);
            }

        }
    }

    public void FinishTurn() {
        Button.interactable = false;
        TurnController.instance.ChangeTurn();
    }
    public void OnStateChange(StateManager.State state)
    {
        ShowFinishTurnButton(false);
    }
    public void ShowFinishTurnButton(bool show)
    {
        tween.Kill();
        if (show)
        {
            Button.interactable = true;
            tween = this.Container.DOPivotY(1, .5f).SetEase(Ease.OutBack).SetDelay(.5f) ;
        }
        else
        {
           tween = this.Container.DOPivotY(0, .2f);


        }
    }

    private void OnEnable()
    {
        TurnController.OnStepExecuted += OnStepExecuted;
        StateManager.OnStateChanged += OnStateChange;
    }

    

    private void OnDisable()
    {
        TurnController.OnStepExecuted -= OnStepExecuted;
        StateManager.OnStateChanged -= OnStateChange;

    }
}
