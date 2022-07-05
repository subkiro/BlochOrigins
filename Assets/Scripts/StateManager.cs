using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;
    public static event Action<State> OnStateChanged;
    private State CurrentState;


    private void Awake()
    {
        instance = this;
    }



    public void SetState(State state) {
        CurrentState = state;
        OnStateChanged?.Invoke(state);
    }

    public State GetState() => CurrentState;

    public enum State {
        Menu,
        PlayerRound,
        NpcRound,
        GameStarted,
        GameEnded
    }

}
