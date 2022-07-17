using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class SpecialEvent: MonoBehaviour {
    public TMP_Text[] amountText;
    public RectTransform Container;
    public int Amount;
    public SpecialEventType eventType;
    public int x,y;

    public SpecialEvent Init(Plate plate,int _amount = 0, SpecialEventType _eventType = SpecialEventType.None) {
        this.Amount = _amount;
        this.eventType = _eventType;

        //PositionOfEvent
        this.x = plate.x;
        this.y = plate.y;

        foreach (TMP_Text item in amountText)
        {
            item.text = "x" + _amount;
        }

        this.Container.DORotate(new Vector3(90, 0, 0),0 ).SetId(this);
        return this;
    }

    public void OnEarnAnimate(Unit player, SpecialEvent specialEvent) {

        if (this == specialEvent) {
            Sequence s = DOTween.Sequence();
            s.SetId(this);
            s.Join(this.Container.DOMoveY(5, 0.5f).SetEase(Ease.OutCubic));
            s.Join(this.Container.DOScale(0, 0.3f).SetEase(Ease.InBack));
            s.OnComplete(() =>
            {
                Destroy(this.gameObject);
            });
           
        }
            
    }


    public enum SpecialEventType { None,Gold, Diamond,Rewinds }





    private void OnEnable()
    {
        SpecialEventManager.OnEventClaimed += OnEarnAnimate;
    }

    private void OnDisable()
    {
        SpecialEventManager.OnEventClaimed -= OnEarnAnimate;
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
        SpecialEventManager.OnEventClaimed -= OnEarnAnimate;

    }
}


