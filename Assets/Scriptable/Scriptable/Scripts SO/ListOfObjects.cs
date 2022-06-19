using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "BlochOrigins/ListObjects")]
public class ListOfObjects : ScriptableObject
{
    public List<LevelSO> AllLevels;
    public List<FloorPlateSO> AllPlates;
    public List<PlayerSO> AllPlayerModels;


}
