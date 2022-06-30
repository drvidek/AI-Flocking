using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopup : MonoBehaviour
{
    float _yDist = 0.5f;
    float _yDistMax = 0.5f;
    float _offsetMax = 2f;
    float _baseScore = 100f;
    string _points;
    Vector3 _dir;
    ScoreKeeper _scoreKeeper;

    public string Points { set { _points = value; } }

    Text[] myText;

    // Start is called before the first frame update
    public void Initialize(ScoreKeeper keeper)
    {
        transform.localScale = new Vector3(1, 1, 1) * Mathf.Min(1.5f, Mathf.Max(float.Parse(_points) / _baseScore, 0.4f));
        myText = GetComponentsInChildren<Text>();
        _yDist = _yDistMax;
        _dir = transform.position - GameObject.Find("Player").transform.position;
        _scoreKeeper = keeper;
        StartCoroutine(Life());
    }
    
    IEnumerator Life()
    {
        foreach (Text text in myText)
        {
            text.text = _points;
        }

        transform.position += new Vector3(Random.Range(-_offsetMax, _offsetMax), Random.Range(-_offsetMax, _offsetMax), 0);

        while (_yDist > 0)
        {
            transform.position += _dir.normalized * _yDist; //(Vector3)Vector2.up * _yDist;
            _yDist = MathExt.Approach(_yDist,0, Time.deltaTime);
            yield return null;
        }
        EndLife();
    }
    private void EndLife()
    {
        StopCoroutine(Life());
        _scoreKeeper.PopupPool.Release(this);
    }
}
