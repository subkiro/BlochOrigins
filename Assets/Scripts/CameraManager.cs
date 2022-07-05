using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public CinemachineVirtualCamera TPS;
    public CinemachineBrain Brain;
    public bool isFPS = true; 

    private void Awake()
    {
        instance = this;
    }



    public void SetCamraOnStateChanged(StateManager.State state) {

        switch (state)
        {
 
            case StateManager.State.PlayerRound:
                TPS.LookAt = TurnController.instance.PlayerUnit.transform;
                break;
            case StateManager.State.NpcRound:
                TPS.LookAt = TurnController.instance.NpcUnit.transform;
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
