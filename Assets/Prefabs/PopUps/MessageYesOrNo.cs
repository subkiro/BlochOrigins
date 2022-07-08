using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
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
            ExitButton.gameObject.SetActive(true);

            if (OnNO != null)
            {
                ExitButton.onClick.AddListener(() =>
                {
                    OnNO?.Invoke();
                    OnComplete?.Invoke();
                });
            }
            else
            {
                ExitButton.onClick.AddListener(() =>
                {
                    OnComplete?.Invoke();
                });
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
            NegativeButton.gameObject.SetActive(true);

            if (OnNO != null)
            {
                NegativeButton.onClick.AddListener(() =>
                {
                    OnNO?.Invoke();
                    OnComplete?.Invoke();
                });
            }
            else
            {
                NegativeButton.onClick.AddListener(() =>
                {
                    OnComplete?.Invoke();
                });
            }
        }


        if (PositiveButton != null)
        {
            PositiveButton.gameObject.SetActive(true);

            if (OnYes != null)
            {
                PositiveButton.onClick.AddListener(() =>
                {
                    OnYes?.Invoke();
                    OnComplete?.Invoke();
                });
            }
            else
            {
                PositiveButton.onClick.AddListener(() =>
                {
                    OnComplete?.Invoke();
                });
            }
        }
    }

}
