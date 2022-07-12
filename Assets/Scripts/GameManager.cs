using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    public ListOfObjects listOfObjects;
    public PlayerInfoPanel playerInfo, playerInfoNpc;
   
    // Start is called before the first frame update

    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        SetupGame();

       
    }

    // Update is called once per frame
    private void SetupGame()
    {

        InitLevel(0);
        SpecialEventManager.instance.InitEvents(LevelManager.instance.grid);
        InitTurnController(InitPlayers("P1"), InitPlayers("P2",true));
        DG.Tweening.DOVirtual.DelayedCall(2, () => TurnController.instance.ChangeTurn());

        
    }


    void InitLevel(int level) {
        LevelManager.instance.GenerateLevel(listOfObjects.AllLevels[level], listOfObjects.AllPlates);
    }

    Unit  InitPlayers(string playerID, bool isNPC = false) {


        PlayerSO playerData = listOfObjects.AllPlayerModels.Find(x => x.PlayerID == playerID);
        if (playerData == null) { Debug.Log("Player with ID: " + playerID + " Not Found"); return null; }

        Unit  player = playerData.CreatePlayer(LevelManager.instance.transform, isNPC);
        player.SpawnPlayer(LevelManager.instance.GetPlayerStartPosition(player));

        //SetupInfoPanel
        if (isNPC) { playerInfoNpc.SetupInfoPanel(player); }
        else { playerInfo.SetupInfoPanel(player); }

        return player;
    }

    void InitTurnController(Unit Player, Unit NpcPlayer) {

        if(Player ==null || NpcPlayer==null ) { Debug.Log("InitTurnController - Some of the player is NULL"); return; }
        TurnController.instance.Init(Player, NpcPlayer);
    }

   
}
