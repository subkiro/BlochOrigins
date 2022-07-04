using System.Collections;
using System;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;
    public State CurrentState;
    private void Awake()
    {
        instance = this;
    }

    public enum State {
        Menu,
        PlayerRound,
        NpcRound,
        GameStarted,
        GameEnded
    }

    

  

    private void OnEnable()
    {
        TurnController.OnTurnChanged += (x) =>
        {

            CurrentState = TurnController.instance.PlayerUnit == x ? State.PlayerRound : State.NpcRound;
            Debug.Log(CurrentState);
        };
    }
}
