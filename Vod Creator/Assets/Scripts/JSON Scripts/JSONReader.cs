using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public class JSONReader
{
    public static string jsonPath = "Assets/VODs/test.json";
    public static POV temp;

    public static void readJSON()
    {
        string data = File.ReadAllText(jsonPath);
        temp = JsonUtility.FromJson<POV>(data);
        Debug.Log(data);
        Debug.Log(temp.players[0].path);
    }

    public static void writeToJSON()
    {

    }
}

[System.Serializable]
public class POV
{
    public POVdata[] players;
    
}

[System.Serializable]
public class POVdata
{
    public string path;
    public string startTime;
}
