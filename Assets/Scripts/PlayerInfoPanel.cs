using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInfoPanel : MonoBehaviour
{
    public Unit Player;
    public TMP_Text DiamondText, GoldText, RewindText,DisplayNametext;
    public Image GoldIcon, DiamondIcon,RewindsIcon;
    private int m_diamonds,m_golds,m_rewinds;
    private string m_Name;

    public int Diamonds {
        set { m_diamonds += value; DiamondText.text = m_diamonds.ToString(); }
        get { return m_diamonds; }
    }
    public int Golds
    {
        set { m_golds += value; GoldText.text = m_golds.ToString(); }
        get { return m_golds; }
    }
    public int Rewinds
    {
        set { m_rewinds += value; if (m_rewinds <= 0) m_rewinds = 0; RewindText.text = m_rewinds.ToString(); }
        get { return m_rewinds; }
    }

    public string Name
    {
        set { m_Name = value; DisplayNametext.text = value; }
        get { return m_Name; }
    }
    public void SetupInfoPanel(Unit player) {
        Player = player;
        Name = player.isNpc ? "Npc player" : "You";
        Golds = 0;
        Golds = Diamonds = Rewinds = 0;

    }

    private void GetUpdateStats(Unit player, SpecialEvent specialEvent )
    {
        if (this.Player != player) { return; }
        switch (specialEvent.eventType)
        {

            case SpecialEvent.SpecialEventType.Gold:
                Golds = specialEvent.Amount;
                GoldIcon.rectTransform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 1, 0);
                break;
            case SpecialEvent.SpecialEventType.Diamond:
                Diamonds = specialEvent.Amount;
                DiamondIcon.rectTransform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 1, 0);
                break;
            case SpecialEvent.SpecialEventType.Rewinds:
                Rewinds = specialEvent.Amount;
                RewindsIcon.rectTransform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 1, 0);
                break;

        }

    }

    private void OnEnable()
    {
        SpecialEventManager.OnEventClaimed += GetUpdateStats;
    }

    private void OnDisable()
    {
        SpecialEventManager.OnEventClaimed -= GetUpdateStats;
    }
}
