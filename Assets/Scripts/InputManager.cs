using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public static UnityAction<Tools.Directions> OnMove;
  

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (StateManager.instance.GetState() == StateManager.State.PlayerRound || StateManager.instance.GetState() == StateManager.State.NpcRound)


            switch (StateManager.instance.GetState())
            {
                case StateManager.State.Menu:
                    break;
                case StateManager.State.PlayerRound:
                    GameControlMovements();
                    break;
                case StateManager.State.NpcRound:
                    break;
                case StateManager.State.GameStarted:
                    break;
                case StateManager.State.GameEnded:
                    break;
                case StateManager.State.Dice:
                    break;          
            }









        //Test


        if (Input.GetKeyDown(KeyCode.Space))
        {

            CameraManager.instance.SetCameraState(CameraManager.CameraStates.TopDown);

        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CameraManager.instance.SetCameraState(CameraManager.CameraStates.GamePlay_Normal);
        }

       

    }



        
        private void GameControlMovements()
        {

        if (Input.GetKeyDown(KeyCode.W))
        {
                OnMove?.Invoke(Tools.Directions.FORWORD);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnMove?.Invoke(Tools.Directions.BACK);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnMove?.Invoke(Tools.Directions.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnMove?.Invoke(Tools.Directions.RIGHT);
        }

        //Mouse Controll
        if (Input.GetMouseButtonDown(0) && StateManager.instance.GetState() == StateManager.State.PlayerRound)
        {
            OnMove?.Invoke(ArrowIndicator.instance.LookDirection);
        }
        if (Input.GetMouseButtonDown(1))
        {
            TurnController.instance.Rewind();
        }
    }



}
