using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PlayerInfoPanel : MonoBehaviour
{
    public Unit Player;
    public TMP_Text DiamondText, GoldText,DisplayNametext,RewindText;
    public int m_diamonds,m_golds,m_rewinds;
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
        set { m_rewinds += value; RewindText.text = m_rewinds.ToString(); }
        get { return m_rewinds; }
    }
    public void SetupInfoPanel(Unit player) {
        Player = player;

    }

    private void GetUpdateStats(Unit player, SpecialEvent specialEvent )
    {
        if (this.Player != player) { return; }
        switch (specialEvent.eventType)
        {

            case SpecialEvent.SpecialEventType.Gold:
                Golds = specialEvent.Amount;
                break;
            case SpecialEvent.SpecialEventType.Diamond:
                Diamonds = specialEvent.Amount;
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
