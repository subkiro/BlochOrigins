using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding 
{

    public PathFinding(int width, int height) { 
    
    }
}


public abstract class PathNode  {
    public  int gCost, hCost, fCost;
    public PathNode cameFrom;

}
