using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalScore
{

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
        _sm.SetComboFromLoad(combo,amount);
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
