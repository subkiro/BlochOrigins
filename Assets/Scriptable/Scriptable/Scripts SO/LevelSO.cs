using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BlochOrigins/LevelSO")]

public class LevelSO : ScriptableObject
{
    public int levelID;
    public bool isCompleted;
    [TextArea(15, 15)]
    public string level = "";
  
    private char[,] arrayList;
    private int maxLevelScore = 0;

    public int MaxLevelScore
    {
        get { return maxLevelScore; }
        set { maxLevelScore = value; }
    }



    public void SetCompleted() {
        isCompleted = true;
    }

    public int X {
        get {
            string[] lines = level.Split('\n');
            return lines.Length;
        }
    }
    public int Y
    {
        get
        {
            string[] lines = level.Split('\n');
            return lines[0].ToCharArray().Length;
        }
    }

    public char[,] GetLevelArray() {

        string[] lines = level.Split('\n');
        int x = lines.Length;      
        int y= lines[0].ToCharArray().Length;

       
       // Debug.Log($" x Lenght: {x}, y lenth {y}");

        arrayList = new char[x, y];
        
        for (int row = 0; row < x; row++)
        {
            
            char[] lineChars =  lines[row].ToCharArray();
           // Debug.Log($" Line: {row} : {lines[row]} Charline Length {lineChars.Length}");
            for (int colum = 0; colum < lineChars.Length; colum++)
            {
              //  Debug.Log(row + " " + colum);
                arrayList[row, colum] = lineChars[colum];
            }
        }
        
        //will implement later
        return arrayList;
    }
    public Vector3 GetPlatePositionByID(char ID) {


        for (int row = 0; row < arrayList.GetLength(0); row++)
        {

            
            for (int colum = 0; colum < arrayList.GetLength(1); colum++)
            {
                if (ID == arrayList[row, colum])
                    return new Vector3(row, 0, colum);
            }
        }

        return Vector3.zero;
    }

   
}
