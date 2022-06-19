using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BlochOrigins/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    public string PlayerID;
    public string PlayerName;
    public GameObject modelPrefab;
    public GameObject UnitContainerPrefab;
   

    public Unit CreatePlayer(Transform Container, bool isNpc)
    {

        GameObject unitContainer = Instantiate(UnitContainerPrefab, Container);

        GameObject model = Instantiate(modelPrefab, unitContainer.transform);

        Unit playerUnit = unitContainer.GetComponent<Unit>();

        model.transform.localScale = new Vector3(1, Random.Range(0.7f, 1f), 1);
        playerUnit.Init(this.PlayerID, this.PlayerName, model.transform,isNpc);

        return playerUnit;
    }



}

