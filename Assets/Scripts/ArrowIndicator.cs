using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ArrowIndicator : MonoBehaviour
{
    public static ArrowIndicator instance;
    public GameObject ArrowModel;
    Vector3 mouseWorldPos,moveVelocity;
    Vector3 rotation;
    public Button FinishTurnButton;

    public Tools.Directions LookDirection;
    private void Awake()
    {
        instance = this;

        
    }


    public void Init(Unit player) {
        this.transform.SetParent(player.transform);
        this.transform.localPosition = Vector3.zero;

    }

    private void LateUpdate()
    {


        LookAtMouse();
    }


    void LookAtMouse()
    {
        
        mouseWorldPos = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVelocity = mouseWorldPos;

        Ray cameraRay = CameraManager.instance.Brain.OutputCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);

            rotation = new Vector3(pointToLook.x, ArrowModel.transform.position.y, pointToLook.z);

                this.transform.LookAt(rotation);


                this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y+90, 0);


            if (this.transform.eulerAngles.y < 45 || this.transform.eulerAngles.y > 315)
            {
                LookDirection = Tools.Directions.LEFT;
            }
            else if (this.transform.eulerAngles.y > 225 && this.transform.eulerAngles.y <= 315)
            {
                LookDirection = Tools.Directions.BACK;
            }
            else if (this.transform.eulerAngles.y > 135 && this.transform.eulerAngles.y <= 225)
            {
                LookDirection = Tools.Directions.RIGHT;
            }
            else if (this.transform.eulerAngles.y > 45 && this.transform.eulerAngles.y <= 135) {
                LookDirection = Tools.Directions.FORWORD;
            }

        }

    }


    public void OnStateChange(StateManager.State state) {

        switch (state)
        {
            case StateManager.State.PlayerRound:
                this.transform.localPosition = Vector3.zero;
                this.transform.DOScale(1, 0.5f).SetId(this).SetEase(Ease.OutBack).OnStart(() => { ArrowModel.gameObject.SetActive(true); });
                break;
            case StateManager.State.NpcRound:
                this.transform.DOScale(0, 0).SetId(this).OnComplete(() => { ArrowModel.gameObject.SetActive(false); });                
                break;
            default:
                this.transform.DOScale(0, 0.3f).SetId(this).SetEase(Ease.OutBack);
                break;
        }
    }

    public void OnStepExecuted(int stepCounter) {

        if (TurnController.instance.GetCurrentUnit() == null) {
            this.transform.DOScale(0, 0f).SetId(this);
            return;
        }
        
        if (!TurnController.instance.GetCurrentUnit().isNpc) {

            Debug.Log(TurnController.instance.GetCurrentUnit().playersAvaliableSteps);
            if (TurnController.instance.GetAvaliableSteps() == 0)
            {
                this.transform.DOScale(0, .5f).SetId(this);

            }
            else {
                this.transform.DOScale(1, .5f).SetId(this).SetEase(Ease.OutBack);
            }
        }
    }



  



    private void OnEnable()
    {
        StateManager.OnStateChanged += OnStateChange;
        TurnController.OnStepExecuted += OnStepExecuted;
    }

    private void OnDisable()
    {
        StateManager.OnStateChanged -= OnStateChange;
        TurnController.OnStepExecuted -= OnStepExecuted;

    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
