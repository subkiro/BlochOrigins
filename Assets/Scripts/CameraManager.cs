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
                GamePlay_Normal.Follow = TurnController.instance.PlayerUnit.transform;
                GamePlay_Normal.LookAt = TurnController.instance.PlayerUnit.transform;
                CameraStateAnimator.SetTrigger(CameraStates.GamePlay_Normal.ToString());
                break;
            case StateManager.State.NpcRound:
                GamePlay_Npc.Follow = TurnController.instance.NpcUnit.transform;
                GamePlay_Npc.LookAt = TurnController.instance.NpcUnit.transform;
                CameraStateAnimator.SetTrigger(CameraStates.GamePlay_Npc.ToString());       
            break;           
                 case StateManager.State.GameStarted:
                 CameraStateAnimator.SetTrigger(CameraStates.TopDown.ToString());
            break;
            case StateManager.State.Dice:
                GamePlay_Normal.Follow = default;
                GamePlay_Normal.LookAt = default;
                GamePlay_Npc.Follow = default;
                GamePlay_Npc.LookAt = default;
                break;

        }


    }


    public void SetCameraState(CameraStates state, Transform lootAt = null) {
       
        CameraStateAnimator.SetTrigger(state.ToString());
       

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
