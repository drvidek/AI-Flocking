using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalScore
{
    private static int _maxHighScores = 25;
    public static int MaxHighScores { get { return _maxHighScores; } }
    public static List<string> HighScoreNames = new List<string>();
    public static List<int> HighScorePoints = new List<int>();
    public static string path = Path.Combine(Application.streamingAssetsPath, "Saves/HighScore.txt");
    public static string pathDefault = Path.Combine(Application.streamingAssetsPath, "Saves/HighScoreDefault.txt");

    public static void LoadHighScores()
    {
        HighScoreNames.Clear();
        HighScorePoints.Clear();
        StreamReader reader = new StreamReader(path);
        string newLine;
        while ((newLine = reader.ReadLine()) != null && newLine != "")
        {
            string[] highScoreEntry = newLine.Split(':');
            HighScoreNames.Add(highScoreEntry[0]);
            HighScorePoints.Add(int.Parse(highScoreEntry[1]));
        }
        reader.Close();
    }

    public static void DefaultHighscores()
    {
        HighScoreNames.Clear();
        HighScorePoints.Clear();
        StreamReader reader = new StreamReader(pathDefault);
        string newLine;
        while ((newLine = reader.ReadLine()) != null && newLine != "")
        {
            string[] highScoreEntry = newLine.Split(':');
            HighScoreNames.Add(highScoreEntry[0]);
            HighScorePoints.Add(int.Parse(highScoreEntry[1]));
        }
        reader.Close();
    }

    public static void SaveHighScores()
    {
        StreamWriter writer = new StreamWriter(path, false);
        //write the file

        for (int i = 0; i < _maxHighScores && i < HighScoreNames.Count; i++)
        {
            string newLine = HighScoreNames[i] + ":" + HighScorePoints[i].ToString();
            writer.WriteLine(newLine);
        }

        writer.Close();
    }

    public static int AddHighScore(string name, int score)
    {
        HighScorePoints.Add(score);
        int index = SortHighScores(score);
        HighScoreNames.Insert(index, name);
        if (HighScoreNames.Count > _maxHighScores)
        {
            HighScoreNames.RemoveAt(_maxHighScores);
            HighScorePoints.RemoveAt(_maxHighScores);
        }
        return index;
    }

    public static int SortHighScores(int score)
    {
        HighScorePoints.Sort();
        HighScorePoints.Reverse();
        int index = HighScorePoints.IndexOf(score);
        return index;
    }

    public static void IncreaseScore(int points, Vector2 position)
    {
        ScoreKeeper _sm = GameObject.Find("GameManager").GetComponent<ScoreKeeper>();
        _sm.IncreaseScore(points, position);
    }

    public static void ResetScore()
    {
        ScoreKeeper _sm = GameObject.Find("GameManager").GetComponent<ScoreKeeper>();
        _sm.ResetScore();
    }

    public static void IncreaseComboMeter(float amount)
    {
        ScoreKeeper _sm = GameObject.Find("GameManager").GetComponent<ScoreKeeper>();
        _sm.IncreaseComboMeter(amount);
    }

    public static void ResetCombo()
    {
        ScoreKeeper _sm = GameObject.Find("GameManager").GetComponent<ScoreKeeper>();
        _sm.ResetCombo();
    }

    public static void SetCombo(int combo, float amount)
    {
        ScoreKeeper _sm = GameObject.Find("GameManager").GetComponent<ScoreKeeper>();
        _sm.SetComboFromLoad(combo, amount);
    }

    public static string GetScore()
    {
        ScoreKeeper _sm = GameObject.Find("GameManager").GetComponent<ScoreKeeper>();
        return _sm.Score;
    }

    public static string GetCombo()
    {
        ScoreKeeper _sm = GameObject.Find("GameManager").GetComponent<ScoreKeeper>();
        return _sm.Combo;
    }

}
