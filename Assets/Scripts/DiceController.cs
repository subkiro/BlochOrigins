using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class DiceController : MonoBehaviour
{
    public GameObject NormalDice;
    public GameObject SpecialDice;
    public Camera UICamera;
    public TMP_Text DiceNumber;
    public static DiceController instance;
    private GameObject diceNormal,diceSpecial;
    public int DiceResult;
    public int AvaliableMovements;
    private void Awake()
    {
        instance = this;
        diceNormal = Instantiate(NormalDice, this.transform);
        diceNormal.transform.localScale = Vector3.zero;
        diceSpecial = Instantiate(SpecialDice, this.transform);
        diceSpecial.transform.localScale = Vector3.zero;
    }

    public void TestThrowDice() {
        ThrowDice(TurnController.instance.GetCurrentUnit());
        
    }

    public void ThrowDice(Unit player) {
        StateManager.instance.SetState(StateManager.State.Dice);
        if (TurnController.instance.IsNpc(player))
        {
            ThrowDiceNpc(player);
        }
        else {
            ThrowDicePlayer(player);
           }
      
    }


    public void ThrowDiceNpc(Unit player) {
        AnimateDice(player);
    }
    public void ThrowDicePlayer(Unit player)
    {
        MessageYesOrNo message = PopUpManager.instance.Show<MessageYesOrNo>(PrefabManager.Instance.MessageYESorNo, withBlur: false);
        message.SetData("Select Dice", "Choose the dice you want to throw", () => AnimateDice(player,true), () => AnimateDice(player), NoButtonText: "Normal",YesButtonText: "Special");


    }
    public void AnimateDice(Unit player,bool isDiceSpecial = false) {
        
        StateManager.instance.SetState(StateManager.State.Dice);
        Transform dice = (isDiceSpecial) ? diceSpecial.transform : diceNormal.transform;

       

        Sequence s = DOTween.Sequence();

        s.Join(DiceNumber.DOScale(0, 0));
        s.Join(dice.transform.DOScale(Vector3.one, .3f));
        s.Join(dice.transform.DOShakeRotation(3, 90, 10, 90,false));
        s.Append(dice.transform.DORotate(OpenDice(isDiceSpecial), 1).SetEase(Ease.OutBack));
        s.AppendInterval(.5f);
        s.Append(dice.transform.DOScale(Vector3.zero, 0.3f));
        s.Append(DiceNumber.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack));
        s.AppendCallback(() => StateManager.instance.SetState((TurnController.instance.IsNpc(player)) ? StateManager.State.NpcRound : StateManager.State.PlayerRound));

    }

    public Vector3 OpenDice(bool isDiceSpecial) {

        DiceResult = Random.Range((isDiceSpecial) ? 5 : 1, (isDiceSpecial)?10:6);
        TurnController.instance.DiceResult = DiceResult;
        DiceNumber.text = DiceResult.ToString();
        switch (DiceResult)
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
            case 7:
                return new Vector3(0, 360, 0);
            case 8:
                return new Vector3(0, 90, 90);
            case 9:
                return new Vector3(0, 180, 180);
            case 10:
                return new Vector3(0, 90, 180);
            default: return new Vector3(0, 0, 0);
        }
    }

    public void UpdateDiceText(int value) {
        DiceNumber.text = $"{value}/{DiceResult}";
    }
   

    private void OnEnable()
    {
        TurnController.OnStepExecuted += UpdateDiceText;
    }

    private void OnDisable()
    {
        TurnController.OnStepExecuted -= UpdateDiceText;
    }
}
