using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class RecordGameState
{
    static string pathPlayer = Path.Combine(Application.streamingAssetsPath, "Saves/PlayerSave.txt");
    static int saveSlots = 3;


    //  KEY
    //  | = new parent (player, flock0-3)
    //  - = new entry (individual agent)
    //  : = break in struct (transform.pos):(transform.up)


    public static void WriteSaveFile(Transform player, string flockString)
    {
        List<string> files = GetSaveFiles();

        StreamWriter saveWrite = new StreamWriter(pathPlayer, false);

        // get the current gamestate save string
        string _saveTransform = player.position.ToString() + ":" + player.up.ToString() + flockString;
        //write that to the text file
        saveWrite.WriteLine(_saveTransform);
        int i = 0;
        while (i < saveSlots - 1)
        {
            saveWrite.WriteLine(files[i]);
            i++;
        }

        saveWrite.Close();
    }

    [MenuItem("My Tools/Default savedata")]
    public static void DefaultSaveFiles()
    {
        List<string> files = GetSaveFiles();

        StreamWriter saveWrite = new StreamWriter(pathPlayer, false);

        int i = 0;
        while (i < saveSlots + 1)
        {
            saveWrite.WriteLine("(0.0, 0.0, 0.0):(0.0, 0.0, 0.0)||||");
            i++;
        }

        saveWrite.Close();
    }

    public static void WriteContinueFile(Transform player)
    {
        List<string> files = GetSaveFiles();

        StreamWriter saveWrite = new StreamWriter(pathPlayer, true);

        // get the current gamestate save string
        string _saveTransform = player.position.ToString() + ":" + player.up.ToString();
        //write that to the 4th line of the text file
        saveWrite.WriteLine(_saveTransform);

        saveWrite.Close();
    }

    public static string[] ReadSaveFile(int file)
    {
        string[] loadString;
        List<string> files = GetSaveFiles();

        if (files[file] != null)
        {
            loadString = files[file].Split('|');
        }
        else loadString = new string[0];

        return loadString;
    }

    public static List<string> GetSaveFiles()
    {
        StreamReader saveRead = new StreamReader(pathPlayer);

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
