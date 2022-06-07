using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class PlayerSave
{
    static string path = Path.Combine(Application.streamingAssetsPath, "Saves/Save.txt");

    public static void WriteSaveFile(Transform player)
    {
        List<string> files = GetSaveFiles();
        StreamWriter saveWrite = new StreamWriter(path, false);

        string _saveTransform = player.position.ToString() + ":" + player.up.ToString();

        saveWrite.WriteLine(_saveTransform);
        foreach(string file in files)
        {
            saveWrite.WriteLine(file);
        }

        saveWrite.Close();
    }

    public static void ReadSaveFile(Transform player, int file)
    {
        List<string> files = GetSaveFiles();
        string[] _loadTransform = files[file].Split(':');
        player.position = MathExt.StringToVector3(_loadTransform[0]);
        player.up = (Vector2)MathExt.StringToVector3(_loadTransform[1]);
    }

    public static List<string> GetSaveFiles()
    {
        StreamReader saveRead = new StreamReader(path);

        List<string> files = new List<string>();

        string readLine = "start";
        while ((readLine = saveRead.ReadLine()) != null)
        {
            files.Add(readLine);
        }

        saveRead.Close();

        return files; 
    }

    

}
