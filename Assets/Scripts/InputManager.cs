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
       
        


        if (Input.GetKeyDown(KeyCode.W)) {
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



        if (Input.GetKeyDown(KeyCode.Space) )
        {
            CameraManager.instance.SetCameraState(CameraManager.CameraStates.TopDown);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CameraManager.instance.SetCameraState(CameraManager.CameraStates.GamePlay_Normal);
        }

        if (Input.GetMouseButtonDown(0) && StateManager.instance.GetState() == StateManager.State.PlayerRound) {
            OnMove?.Invoke(ArrowIndicator.instance.LookDirection); 
        }
        if (Input.GetMouseButtonDown(1))
        {
            TurnController.instance.Rewind();
        }




        //Test

        if (Input.GetKeyDown(KeyCode.M))
        {
            MessageYesOrNo message = PopUpManager.instance.Show<MessageYesOrNo>(PrefabManager.Instance.MessageYESorNo);
            message.SetData("Title", "Welcome to our new message", () => Debug.Log("OnYes"), () => Debug.Log("OnNo")); CameraManager.instance.SetCameraState(CameraManager.CameraStates.TopDown);
        }

    }




}
