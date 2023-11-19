using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Will handle creating and loading files, could add encrpytion but will just make it persistent for now
/// </summary>
public static class JSONManager 
{
    public static readonly string directory = "/SaveData/";
    public static readonly string fileExt = ".json";

    private static readonly string fileName = "HighScores"; //With the current design, there is only one file containing a list of player scores, so will mark this here

    static string file = fileName + fileExt;
    static string fullPath = Application.persistentDataPath + directory + file;

    static JSONManager()
    {
        CreateDirectorys();
    }

    //Create a directory
    public static void CreateDirectorys()
    {
        
        string dir = Application.persistentDataPath + directory;
        Directory.CreateDirectory(dir);
    }

    public static void SaveData(HighScores obj)
    {
        string dir = Application.persistentDataPath + directory;
        string file = fileName + fileExt;

        if (Directory.Exists(dir))
        {
            string json = JsonUtility.ToJson(obj, false);
            File.WriteAllText(dir + file, json);
            Debug.Log("FileSaved");
        }
    }

    public static HighScores LoadData()
    {
      
        HighScores so = new();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            so = JsonUtility.FromJson<HighScores>(json);
        }

        else
        {
            Debug.Log("Save file does not exist");
        }

        return so;

    }

    public static bool CheckIfSaveExits()
    {
        return File.Exists(fullPath);
    }



}
