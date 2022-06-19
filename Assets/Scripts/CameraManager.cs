using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public CinemachineVirtualCamera TPS;
    public bool isFPS = true; 

    private void Awake()
    {
        instance = this;
    }



    public void SetCamraOnTurnChange(Unit player) {
    
            TPS.LookAt = player.transform;
            TPS.Follow = player.transform;    
    }

    


    private void OnEnable()
    {
        TurnController.OnTurnChanged += SetCamraOnTurnChange;
    }

    private void OnDisable()
    {
        TurnController.OnTurnChanged -= SetCamraOnTurnChange;
    }

    
}
