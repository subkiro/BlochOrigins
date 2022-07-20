using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public ListOfObjects listOfObjects;
    public PlayerInfoPanel playerInfo, playerInfoNpc;
    public  bool useDebug = false;
    public Transform Base;
    public CanvasGroup InGamePanelUI;
    [SerializeField] GameObject ArrowControler;

    public static UnityAction<Unit> OnPlayerSelected;

    private int SelectedPlayerIndex;
    private int StageSelected = 0;
    private Unit SelectedPlayer;

    bool isShowing = false;
    // Start is called before the first frame update

    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // SetupGame();
       
        SetUpBase();
    }


    private void SetupGame()
    {
        StateManager.instance.SetState(StateManager.State.GameStarted);
       if(SelectedPlayer!=null)  Destroy(SelectedPlayer);
        InGamePanelUI.DOFade(1, .5f).SetDelay(2);
        InitLevel(StageSelected);
        SpecialEventManager.instance.InitEvents(LevelManager.instance.grid);
        InitTurnController(InitPlayers(SelectedPlayerIndex, false), InitPlayers(Random.Range(0,5),true));
        DG.Tweening.DOVirtual.DelayedCall(2, () => TurnController.instance.ChangeTurn());

        
    }
    public void SetUpBase()
    {
        if (SelectedPlayer != null) Destroy(SelectedPlayer.gameObject);
       

        StageSelected = 0;  
        StateManager.instance.SetState(StateManager.State.Menu);
        ShowPlayer(0);
        InGamePanelUI.DOFade(0, .5f);
        MessageMainMenu message = PopUpManager.instance.Show<MessageMainMenu>(PrefabManager.Instance.MessageMainMenu, withBlur: false);

    }



    void InitLevel(int level) {

        LevelManager.instance.GenerateLevel(listOfObjects.AllLevels[level], listOfObjects.AllPlates);
    }
    Unit  InitPlayers(int playerIndex, bool isNPC = false) {


        PlayerSO playerData = listOfObjects.AllPlayerModels[playerIndex];

        Unit  player = playerData.CreatePlayer(LevelManager.instance.transform, isNPC);
        player.SpawnPlayer(LevelManager.instance.GetPlayerStartPosition(player));

        //SetupInfoPanel
        if (isNPC) {
            playerInfoNpc.SetupInfoPanel(player);
        }
        else {
            playerInfo.SetupInfoPanel(player);
            Instantiate(ArrowControler);
            ArrowIndicator.instance.Init(player);

        }

        return player;
    }
    void InitTurnController(Unit Player, Unit NpcPlayer) {

        if(Player ==null || NpcPlayer==null ) { Debug.Log("InitTurnController - Some of the player is NULL"); return; }
        TurnController.instance.Init(Player, NpcPlayer);
    }


    public int SelectStage(int stageCounter) {
       return StageSelected = (StageSelected + stageCounter >= listOfObjects.AllLevels.Count) ? 0 : (StageSelected + stageCounter < 0) ? listOfObjects.AllLevels.Count - 1 : StageSelected + stageCounter;
    }
    public void SelectRight() {
        if (isShowing) return;
        ++SelectedPlayerIndex;
        if (SelectedPlayerIndex >= listOfObjects.AllPlayerModels.Count) SelectedPlayerIndex = 0;       
        ShowPlayer(SelectedPlayerIndex);
    }
    public void SelectLeft() {
        if (isShowing) return;
        --SelectedPlayerIndex;
        if (SelectedPlayerIndex < 0) SelectedPlayerIndex = listOfObjects.AllPlayerModels.Count - 1;
        ShowPlayer(SelectedPlayerIndex);
    }
    public void SelectStartGame() {
        Destroy(SelectedPlayer.gameObject);
        SetupGame();
    }

    public async void ShowPlayer(int playerIndex)
    {


        isShowing = true;




        if (SelectedPlayer != null)
        {
            Task task = SelectedPlayer.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).SetId(this).AsyncWaitForCompletion();
            await task;
            Destroy(SelectedPlayer.gameObject);


        }


        PlayerSO playerData = listOfObjects.AllPlayerModels[playerIndex];
        Unit player = playerData.CreatePlayer(Base, false);
        player.transform.DORotate(new Vector3(0, -180, 0), 0);
        SelectedPlayer = player;

        Task task2 = SelectedPlayer.transform.DOScale(1, 0.5f).From(0).SetEase(Ease.InBack).SetId(this).AsyncWaitForCompletion();
        await task2;

        isShowing = false;
        OnPlayerSelected?.Invoke(SelectedPlayer);
    }
    public void GameEnded() {



        MessageMatchEnd message = PopUpManager.instance.Show<MessageMatchEnd>(PrefabManager.Instance.MessageGameEnd, withBlur: false);
        message.SetData(SetUpBase);


    }


    public void OnStateChanged(StateManager.State state) {
        switch (state)
        {
            case StateManager.State.Menu:
                InGamePanelUI.DOFade(0, 0);
                break;
            case StateManager.State.PlayerRound:
            case StateManager.State.NpcRound:
            case StateManager.State.GameStarted:
            case StateManager.State.Dice:
                InGamePanelUI.DOFade(1, 1);
                break;
            case StateManager.State.GameEnded:
                InGamePanelUI.DOFade(0, 0);
                GameEnded();
                break;
        }
    }


    private void OnEnable()
    {
        StateManager.OnStateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        StateManager.OnStateChanged -= OnStateChanged;   
    }
}
