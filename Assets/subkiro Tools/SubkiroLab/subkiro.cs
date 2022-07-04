using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Subkiro 
{
    public static int[] GetRandomArray(int wantedArrayLength, int arrayMaxLength)
    {

        List<int> l = new List<int>();
        int[] shuffeldAarray = new int[wantedArrayLength];

        for (int i = 0; i < arrayMaxLength; i++)
        {
            l.Add(i);
        }

        for (int i = 0; i < wantedArrayLength; i++)
        {
            int rand = Random.Range(0, l.Count);
            shuffeldAarray[i] = l[rand];
           // Debug.Log(shuffeldAarray[i]);
            l.RemoveAt(rand);
        }

        l.Clear();
        return shuffeldAarray;
    }

    //Create Text in World
    public static TextMesh CreateTextMesh(string text, Transform parent = null,Vector3 localPosition = default(Vector3), int fontSize = 40,float charSize = 0.1f) {
        
        
        GameObject gameObject = new GameObject("WorldText", typeof(TextMesh));
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.color = Color.white;
        textMesh.fontSize = fontSize;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.characterSize = charSize;


        return textMesh;
    
    }

    public static Vector3 GetMouseWorldPosition() {
        Vector3 world_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        world_pos.y = 0;
        return world_pos;
    }
   

}
