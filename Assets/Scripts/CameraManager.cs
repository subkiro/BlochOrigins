using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public CinemachineBrain Brain;
    public Animator CameraStateAnimator;

    public CinemachineVirtualCamera GamePlay_Normal, GamePlay_Npc,TopDown;

    private void Awake()
    {
        instance = this;
        CameraStateAnimator = GetComponent<Animator>();
        
    }


    public enum CameraStates { GamePlay_Normal, GamePlay_Npc,TopDown }

    public void SetCamraOnStateChanged(StateManager.State state) {

        switch (state)
        {

            case StateManager.State.PlayerRound:
                SetCameraState(CameraStates.GamePlay_Normal);
                break;
            case StateManager.State.NpcRound:
                SetCameraState(CameraStates.GamePlay_Npc);
                break;           
                 case StateManager.State.GameStarted:
                SetCameraState(CameraStates.TopDown);
            break;
            case StateManager.State.Dice:
                GamePlay_Normal.Follow = TurnController.instance.GetCurrentUnit().transform;
                GamePlay_Normal.LookAt = TurnController.instance.GetCurrentUnit().transform;
           
                break;
            case StateManager.State.Menu:

                break;
        }


    }


    public void SetCameraState(CameraStates state, Transform lootAt = null) {

        switch (state)
        {
            case CameraStates.GamePlay_Normal:
                GamePlay_Normal.Follow = TurnController.instance.PlayerUnit.transform;
                //GamePlay_Normal.LookAt = TurnController.instance.PlayerUnit.transform;
                CameraStateAnimator.SetTrigger(state.ToString());
                break;
            case CameraStates.GamePlay_Npc:
                GamePlay_Npc.Follow = TurnController.instance.NpcUnit.transform;
             //   GamePlay_Npc.LookAt = TurnController.instance.NpcUnit.transform;
                CameraStateAnimator.SetTrigger(state.ToString());
                break;
            case CameraStates.TopDown:
                TopDown.Follow = TurnController.instance.GetCurrentUnit().transform;
               // TopDown.LookAt = TurnController.instance.GetCurrentUnit().transform;
                break;
          
        }
       

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
