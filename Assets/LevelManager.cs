using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public  class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GenericGrid<FloorObj> grid;
    public GameObject Floor1_prefab;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        grid = new GenericGrid<FloorObj>(10, 10,10, 10, Vector3.zero, (GenericGrid<FloorObj> g, int x, int y) => new FloorObj(g,x,y));

    }


   

}

public class FloorObj
{
    int value;
    GenericGrid<FloorObj> grid;
    GameObject obj;

    public void AddValue(int addValue) {
        value += addValue;
    }

    public FloorObj(GenericGrid<FloorObj> grid, int x, int y) {
        this.grid = grid;
        obj= GameObject.Instantiate(LevelManager.instance.Floor1_prefab);
        obj.transform.SetParent(LevelManager.instance.transform);
        obj.transform.localPosition = new Vector3(x, 0, y);
        AddValue(2*x + y);
        Animate();
    }

    public void Animate() {
        Color brown = obj.GetComponent<MeshRenderer>().materials[0].color;
        Color green = obj.GetComponent<MeshRenderer>().materials[1].color;


        obj.GetComponent<MeshRenderer>().materials[0].DOColor(brown, .3f).From(Color.white);
        
        obj.GetComponent<MeshRenderer>().materials[1].DOColor(green, Random.Range(.2f, .3f)).From(Color.white).SetDelay(1);

        obj.transform.DOMoveY(0, Random.Range(1, 2)).From(10).SetEase(Ease.OutBounce).SetDelay(value*0.1f);

    }

}
