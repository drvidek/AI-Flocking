using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class HandleGameSaveFile
{
    static string pathSave = Path.Combine(Application.streamingAssetsPath, "Saves/Save.txt");
    static string pathContinue = Path.Combine(Application.streamingAssetsPath, "Saves/Continue.txt");

    public static int saveSlots = 3;


    //  KEY
    //  | = new parent (player, flock0-3)
    //  - = new entry (individual agent)
    //  : = break in struct (transform.pos):(transform.up)


    public static void WriteSaveFile(Transform player, string flockString)
    {
        List<string> files = ListSaveFiles();

        StreamWriter saveWrite = new StreamWriter(pathSave, false);

        // get the current gamestate save string
        string _saveTransform = player.position.ToString() + ":" + player.up.ToString() + flockString;
        //write that to the text file
        saveWrite.WriteLine(_saveTransform);
        int i = 0;
        if (files.Count > 0)
        {
            while (i < saveSlots - 1 && i < files.Count)
            {
                saveWrite.WriteLine(files[i]);
                i++;
            }
        }

        saveWrite.Close();

        WriteContinueFile(_saveTransform);
    }

    [MenuItem("My Tools/Default savedata")]
    public static void DefaultSaveFiles()
    {
        StreamWriter saveWrite = new StreamWriter(pathSave, false);

        int i = 0;
        while (i < saveSlots)
        {
            saveWrite.WriteLine("");
            i++;
        }
        saveWrite.Close();

        StreamWriter contWrite = new StreamWriter(pathContinue, false);
        contWrite.WriteLine("");
        contWrite.Close();
    }

    public static void WriteContinueFile(string fileString)
    {
        StreamWriter saveWrite = new StreamWriter(pathContinue, false);

        //write this to the file
        saveWrite.WriteLine(fileString);

        saveWrite.Close();
    }

    public static string[] ReadContinueFile()
    {
        string[] loadString;

        StreamReader saveRead = new StreamReader(pathContinue);
        string readLine;
        if ((readLine = saveRead.ReadLine()) != null && readLine != "")
        {
            loadString = readLine.Split('|');
        }
        else loadString = new string[0];

        return loadString;
    }

    public static string[] ReadSaveFile(int file)
    {
        string[] loadString;
        List<string> files = ListSaveFiles();

        Debug.Log(files[file]);

        if (files[file] != null)
        {
            loadString = files[file].Split('|');
        }
        else loadString = new string[0];

        return loadString;
    }

    public static List<string> ListSaveFiles()
    {
        StreamReader saveRead = new StreamReader(pathSave);

        List<string> files = new List<string>();

        string readLine;
        while ((readLine = saveRead.ReadLine()) != null && readLine != "")
        {
            files.Add(readLine);
        }

        saveRead.Close();

        return files;
    }



}
