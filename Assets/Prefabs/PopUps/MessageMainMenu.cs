using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MessageMainMenu : PopUp
{
    public RectTransform MainInfoUI;
    public CanvasGroup MainInfoGroup;
    public Button StartGameButton,  LeftButton, RightButton;
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
        StartGameButton.onClick.AddListener(SelectStartGame);

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
