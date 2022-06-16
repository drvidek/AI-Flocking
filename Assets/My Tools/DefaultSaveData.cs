using System.IO;
using UnityEngine;
using UnityEditor;

public static class DefaultSaveData
{
    static string pathSave = Path.Combine(Application.streamingAssetsPath, "Saves/Save.txt");
    static string pathContinue = Path.Combine(Application.streamingAssetsPath, "Saves/Continue.txt");
    public static int saveSlots = 3;

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

        PlayerPrefs.DeleteKey("FirstLoadKeys");
        PlayerPrefs.DeleteKey("FirstLoadSettings");
        PlayerPrefs.DeleteKey("FirstLoadScores");
    }
}
