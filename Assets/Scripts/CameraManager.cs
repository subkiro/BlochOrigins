using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public CinemachineBrain Brain;
    public Animator CameraStateAnimator;

    private void Awake()
    {
        instance = this;
        CameraStateAnimator = GetComponent<Animator>();
    }


    public enum CameraStates { GamePlay_Normal, GamePlay_Zoom, TopDown,Menu }

    public void SetCamraOnStateChanged(StateManager.State state) {

        switch (state)
        {

            case StateManager.State.PlayerRound:
                CameraStateAnimator.SetTrigger(CameraStates.GamePlay_Normal.ToString());
                Brain.ActiveVirtualCamera.LookAt=TurnController.instance.PlayerUnit.transform;
                break;
            case StateManager.State.NpcRound:
                CameraStateAnimator.SetTrigger(CameraStates.GamePlay_Normal.ToString());
                Brain.ActiveVirtualCamera.LookAt = TurnController.instance.NpcUnit.transform;                
                break;           
            case StateManager.State.Menu:
                CameraStateAnimator.SetTrigger(CameraStates.Menu.ToString());
                break;
            case StateManager.State.GameStarted:
                CameraStateAnimator.SetTrigger(CameraStates.TopDown.ToString());
                break;
            case StateManager.State.GameEnded:
                break;
            case StateManager.State.Dice:
                break;
        }


    }


    public void SetCameraState(CameraStates state, Transform lootAt = null) {
       
        CameraStateAnimator.SetTrigger(state.ToString());
        if (lootAt != null)
            Brain.ActiveVirtualCamera.LookAt = lootAt;

    }

    private void OnEnable()
    {
        StateManager.OnStateChanged += SetCamraOnStateChanged;
    }

    private void OnDisable()
    {
        StateManager.OnStateChanged -= SetCamraOnStateChanged;
    }


}
