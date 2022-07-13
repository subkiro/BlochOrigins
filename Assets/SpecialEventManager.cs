using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class SpecialEventManager: MonoBehaviour 
{
    public static SpecialEventManager instance;
    public GameObject GoldPrefab, DiamondPrefab,RewindPrefab; 
    public List<SpecialEvent> SpecialEvents;
    public static UnityAction<Unit, SpecialEvent> OnEventClaimed;
    private void Awake()
    {
        instance = this;
        SpecialEvents = new List<SpecialEvent>();
    }

    public void InitEvents(GenericGrid<GridObject> grid) {
    

        for (int x = 0; x < grid.GridWidth(); x++)
        {
            for (int y = 0; y < grid.GridHeight(); y++)
            {
                CreateEvent(grid.GetGridObject(x, y).GetPlate());
            }
        }
    }
    public void CreateEvent(Plate plate) {
        GameObject eventObj;
        SpecialEvent specialEvent;        
        switch (plate.ID)
        {
            case "G":
                eventObj = Instantiate(GoldPrefab, plate.transform.position, Quaternion.identity);
                eventObj.transform.SetParent(LevelManager.instance.FloorContainer);
                eventObj.transform.position += new Vector3(0, 0.1f, 0);
                specialEvent = eventObj.GetComponent<SpecialEvent>().Init(plate,Random.Range(1, 5), SpecialEvent.SpecialEventType.Gold);
                SpecialEvents.Add(specialEvent);
                break;
            case "D":
                eventObj = Instantiate(DiamondPrefab, plate.transform.position, Quaternion.identity);
                eventObj.transform.SetParent(LevelManager.instance.FloorContainer);
                eventObj.transform.position += new Vector3(0, 0.1f, 0);
                specialEvent = eventObj.GetComponent<SpecialEvent>().Init(plate, Random.Range(1, 3), SpecialEvent.SpecialEventType.Diamond);
                SpecialEvents.Add(specialEvent);
                break;
            case "R":
                eventObj = Instantiate(RewindPrefab, plate.transform.position, Quaternion.identity);
                eventObj.transform.SetParent(LevelManager.instance.FloorContainer);
                eventObj.transform.position += new Vector3(0, 0.1f, 0);
                specialEvent = eventObj.GetComponent<SpecialEvent>().Init(plate, Random.Range(1, 3), SpecialEvent.SpecialEventType.Rewinds);
                SpecialEvents.Add(specialEvent);
                break;
            default:
                break;
        }
    }
    public void CreateEventRandom(Plate plate)
    {
        GameObject eventObj;
        SpecialEvent specialEvent;
        switch (Random.Range(0,2))
        {
            case 0:
                eventObj = Instantiate(GoldPrefab, plate.transform.position, Quaternion.identity);
                specialEvent = eventObj.GetComponent<SpecialEvent>().Init(plate, Random.Range(1, 5), SpecialEvent.SpecialEventType.Gold);
                eventObj.transform.SetParent(LevelManager.instance.FloorContainer);
                SpecialEvents.Add(specialEvent);
          
                break;
            case 1:
                eventObj = Instantiate(DiamondPrefab, plate.transform.position, Quaternion.identity);
                specialEvent = eventObj.GetComponent<SpecialEvent>().Init(plate, Random.Range(1, 3), SpecialEvent.SpecialEventType.Diamond);
                eventObj.transform.SetParent(LevelManager.instance.FloorContainer);
                SpecialEvents.Add(specialEvent);
                break;
            case 2:
               
                break;
        }
    }
    public void ClaimSpecialEvent(Unit player,Plate plate) {

        SpecialEvent sp_event = null;

        for (int i = 0; i < SpecialEvents.Count; i++)
        {
            if ((SpecialEvents[i].x == plate.x) && (SpecialEvents[i].y == plate.y)) {
                sp_event = SpecialEvents[i];
                SpecialEvents.Remove(sp_event);
                break;
            }
        }

        if (sp_event != null) {
            OnEventClaimed?.Invoke(player, sp_event);
            
        }

    }

}


