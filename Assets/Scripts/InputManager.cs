using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public static UnityAction<Tools.Directions> OnMove;
    public bool espcapePressed = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {


            switch (StateManager.instance.GetState())
            {
                case StateManager.State.Menu:
                    EscapeCheck(true);
                    break;
                case StateManager.State.PlayerRound:
                    GameControlMovements();
                    EscapeCheck(false);
                    break;
                case StateManager.State.NpcRound:
                    EscapeCheck(false);
                    break;
                case StateManager.State.GameStarted:
                    break;
                case StateManager.State.GameEnded:
                    break;
                case StateManager.State.Dice:
                
                    break;          
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

    private void EscapeCheck(bool Quit)
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Quit)
        {
            if (!espcapePressed)
            {
                espcapePressed = true;
                MessageYesOrNo message = PopUpManager.instance.Show<MessageYesOrNo>(PrefabManager.Instance.MessageYESorNo, withBlur: false);
                message.SetData(Tite: "Exit", Message: "Do you want to leave the game?", () =>
                {
                    SceneManager.LoadScene(0);
                    espcapePressed = false;
                }, () =>
                {
                    espcapePressed = false;
                });
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Quit)
        {
            Application.Quit(); 

        }
    }


}
