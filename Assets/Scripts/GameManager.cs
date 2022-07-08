using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    public ListOfObjects listOfObjects;
   
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
        InitTurnController(InitPlayers("P1"), InitPlayers("P2",true));




        DG.Tweening.DOVirtual.DelayedCall(2, () => TurnController.instance.SetTurn(TurnController.instance.PlayerUnit));

    }


    void InitLevel(int level) {
        LevelManager.instance.GenerateLevel(listOfObjects.AllLevels[0], listOfObjects.AllPlates);
    }

    Unit  InitPlayers(string playerID, bool isNPC = false) {

        PlayerSO playerData = listOfObjects.AllPlayerModels.Find(x => x.PlayerID == playerID);
        if (playerData == null) { Debug.Log("Player with ID: " + playerID + " Not Found"); return null; }

        Unit  player = playerData.CreatePlayer(LevelManager.instance.transform, isNPC);
        Debug.Log("Position: " + LevelManager.instance.GetPlayerStartPosition(player));
        player.SpawnPlayer(LevelManager.instance.GetPlayerStartPosition(player));    

        return player;
    }

    void InitTurnController(Unit Player, Unit NpcPlayer) {

        if(Player ==null || NpcPlayer==null ) { Debug.Log("InitTurnController - Some of the player is NULL"); return; }
        TurnController.instance.Init(Player, NpcPlayer);
    }

    
}
