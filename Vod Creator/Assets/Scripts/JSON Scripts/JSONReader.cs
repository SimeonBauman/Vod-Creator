using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public class JSONReader
{
    public static string jsonPath = "Assets/VODs/test.json";
    public static POV vid;

    public static void readJSON()
    {
        string data = File.ReadAllText(jsonPath);
        vid = JsonUtility.FromJson<POV>(data);
        
        
    }

    public static void writeToJSON(string name, POVdata[] paths)
    {
        if(name != "") jsonPath = "Assets/VODs/" + name + ".json";
        POV pt = new POV();
        pt.players = paths;
        string temp = JsonUtility.ToJson(pt,true);
        File.WriteAllText(jsonPath, temp);
        
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
