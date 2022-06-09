using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    private int _score;
    public int Score { get { return _score; } }

    private string _scoreString;

    [SerializeField] private Text _scoreText;

    public static string scorePopupPath = "Prefabs/Score Popup";


    private void Start()
    {
        _scoreString = _scoreText.text;
        UpdateScoreText();
    }

    public void IncreaseScore(int points, Vector2 position)
    {
        _score += points;
        UpdateScoreText();
        GameObject popupPrefab = Resources.Load(scorePopupPath) as GameObject;
        ScorePopup _popup = Instantiate(popupPrefab, new Vector3(position.x,position.y,-2), new Quaternion(0,0,0,0)).GetComponent<ScorePopup>();
        _popup.Points = points.ToString();
    }

    private void UpdateScoreText()
    {
        _scoreText.text = _scoreString + _score.ToString();
    }
}
