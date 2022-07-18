using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class MessageMainMenu : PopUp
{
    public RectTransform MainInfoUI;
    public CanvasGroup MainInfoGroup;
    public Button StartGameButton,  LeftButton, RightButton;
    public Button  LeftButtonStage, RightButtonStage;
    public TMP_Text StageCounterText;
    public TMP_Text playerName;
    public UnityAction OnComplete;
    public override void Show(UnityAction OnComplete)
    {
        this.OnComplete = OnComplete;
        SetData();
    }

    public void SetData() {

        LeftButton.onClick.AddListener(SelectLeft);
        RightButton.onClick.AddListener(SelectRight);

        StageCounterText.text = "0";
        LeftButtonStage.onClick.AddListener(()=>SelectStage(-1));
        RightButtonStage.onClick.AddListener(() => SelectStage(+1));


        StartGameButton.onClick.AddListener(SelectStartGame);

    }

    private void SelectStage(int value)
    {
        StageCounterText.text = GameManager.instance.SelectStage(value).ToString();
    }

    public void SelectRight() { GameManager.instance.SelectRight(); }
    public void SelectLeft() { GameManager.instance.SelectLeft(); }
    public void SelectStartGame() { GameManager.instance.SelectStartGame(); OnComplete.Invoke(); }

    private void OnPlayerSelected(Unit selecetedPlayer)
    {
        this.playerName.DOFade(1, 0.3f).From(0);
        this.playerName.text = selecetedPlayer.playerName;
    }

    private void OnEnable()
    {
        GameManager.OnPlayerSelected += OnPlayerSelected;
    }

   

    private void OnDisable()
    {
        GameManager.OnPlayerSelected -= OnPlayerSelected;
    }
}
