using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip step;
    public AudioClip coin;
    public AudioClip matchStarted;


    public AudioSource source;

    private void OnStep(Unit player)
    {
        source.PlayOneShot(step);

    }
    private void OnClaim(Unit arg0, SpecialEvent arg1)
    {
        source.PlayOneShot(coin);
    }

    private void OnStateChanged(StateManager.State state)
    {
        switch (state)
        {
            case StateManager.State.Menu:
                break;
            case StateManager.State.PlayerRound:
                break;
            case StateManager.State.NpcRound:
                break;
            case StateManager.State.GameStarted:
                source.PlayOneShot(matchStarted);
                break;
            case StateManager.State.GameEnded:
                break;
            case StateManager.State.Dice:
                break;
            default:
                break;
        }
    }


    private void OnEnable()
    {
        Unit.OnStepStarted += OnStep;
        StateManager.OnStateChanged += OnStateChanged;
        SpecialEventManager.OnEventClaimed += OnClaim;
    }

    

    private void OnDisable()
    {
        Unit.OnStepStarted -= OnStep;
        StateManager.OnStateChanged -= OnStateChanged;
        SpecialEventManager.OnEventClaimed -= OnClaim;
    }
}
