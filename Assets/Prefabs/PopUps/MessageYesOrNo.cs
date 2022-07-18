using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

public class MessageYesOrNo : PopUp
{
    public RectTransform MainInfoUI;
    public CanvasGroup MainInfoGroup;
    public TMP_Text MessageTextUI, TitleTextUI, NegativeButtonText, PositiveButtonText;
    public Button ExitButton, OverlayButton, NegativeButton, PositiveButton;
    public UnityAction OnComplete;
    public override void Show(UnityAction OnComplete)
    {
        this.OnComplete = OnComplete;
    }


    private string LocaYes, LocaNo, LocaTitle, LocaMessage;
    //Pack...
   
   

    public void SetData(string Tite, string Message, UnityAction OnYes, UnityAction OnNO, string NoButtonText = "No", string YesButtonText = "Yes")
    {

        TitleTextUI.text = Tite;
        MessageTextUI.text = Message;

        if (NegativeButtonText != null) NegativeButtonText.text = NoButtonText;
        if (PositiveButtonText != null) PositiveButtonText.text = YesButtonText;

       


        if (ExitButton != null)
        {

            if (OnNO != null)
            {
                ExitButton.gameObject.SetActive(true);

                ExitButton.onClick.AddListener(() =>
                {
                    OnNO?.Invoke();
                    OnComplete?.Invoke();
                });
            }
            else
            {
                ExitButton.gameObject.SetActive(false);

            }
        }


        if (OverlayButton != null) 
        {
            OverlayButton.gameObject.SetActive(true);

            if (OnNO != null)
            {

                OverlayButton.onClick.AddListener(() =>
                {
                    OnNO?.Invoke();
                    OnComplete?.Invoke();
                });
            }
            else {
                OverlayButton.onClick.AddListener(() =>
                {
                    OnComplete?.Invoke();
                });
            }
        }

        if (NegativeButton != null)
        {

            if (OnNO != null)
            {
                NegativeButton.gameObject.SetActive(true);

                NegativeButton.onClick.AddListener(() =>
                {
                    OnNO?.Invoke();
                    OnComplete?.Invoke();
                });
            }
            else
            {
                NegativeButton.gameObject.SetActive(false);

            }
        }


        if (PositiveButton != null)
        {

            if (OnYes != null)
            {
                PositiveButton.gameObject.SetActive(true);

                PositiveButton.onClick.AddListener(() =>
                {
                    OnYes?.Invoke();
                    OnComplete?.Invoke();
                });
            }
            else
            {
                PositiveButton.gameObject.SetActive(false);

            }
        }
    }


    private void OnEnable()
    {
        StateManager.OnStateChanged += OnStateChanged;
    }
    private void OnDisable()
    {
        StateManager.OnStateChanged -= OnStateChanged;
    }
    private void OnStateChanged(StateManager.State state)
    {
        switch (state)
        {
    
            case StateManager.State.GameEnded:
                OnComplete?.Invoke();
                break;
        }
    }
}
