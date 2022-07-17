using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class MessageMatchEnd : PopUp
{
    public RectTransform MainInfoUI;
    public CanvasGroup MainInfoGroup;
    public Button ReturnToMainMenu;
    public UnityAction OnComplete;
    public UnityAction OnCompleteAction;
    public TMP_Text totalPointsText, totalPointsNpcText;

    public PlayerInfoPanel player;
    public PlayerInfoPanel playerNPC;


    public override void Show(UnityAction OnComplete)
    {
        this.OnComplete = OnComplete;
       
    }

    public void SetData(UnityAction OnCompleteAction)
    {

        int p1_diamonds = GameManager.instance.playerInfo.Diamonds;
        int p1_gold = GameManager.instance.playerInfo.Golds;
        int p1_rewinds = GameManager.instance.playerInfo.Rewinds;


        int p2_diamonds = GameManager.instance.playerInfoNpc.Diamonds;
        int p2_gold = GameManager.instance.playerInfoNpc.Golds;
        int p2_rewinds = GameManager.instance.playerInfoNpc.Rewinds;


        player.Init(GameManager.instance.playerInfo);
        playerNPC.Init(GameManager.instance.playerInfoNpc);

        int p1_sumPoins = p1_gold + p1_diamonds * 2 + (p1_rewinds > 0 ? 1 : 0);
        int p2_sumPoins = p2_gold + p2_diamonds * 2 + (p2_rewinds > 0 ? 1 : 0);

        totalPointsText.text = p1_sumPoins.ToString();
        totalPointsNpcText.text = p2_sumPoins.ToString();

        int winner = (p2_sumPoins>p1_sumPoins)?-1: (p2_sumPoins == p1_sumPoins)? 0: 1;

        switch (winner)
        {
            case -1: //winner is NPC
                playerNPC.HilightOutline.DOFade(1, .5f);
                player.HilightOutline.DOFade(0, 0);
                break;
            case 0: //draw
                playerNPC.HilightOutline.DOFade(0, 0);
                player.HilightOutline.DOFade(0, 0);
                break;
            case 1://winner is Player
                playerNPC.HilightOutline.DOFade(0, 0);
                player.HilightOutline.DOFade(1, .5f);
                break;
         
        }

        this.OnCompleteAction = OnCompleteAction;
        ReturnToMainMenu.onClick.AddListener(ReturnToMainMenuAction);
      

    }
    public void ReturnToMainMenuAction() {
        OnCompleteAction?.Invoke();
        OnComplete?.Invoke();
    }
   
}