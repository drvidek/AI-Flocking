using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopup : MonoBehaviour
{
    float _yDist = 0.5f;
    float _offsetMax = 2f;
    string _points;
    Vector3 _dir;

    public string Points { set { _points = value; } }

    Text[] myText;
    
    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponentsInChildren<Text>();
        _dir = transform.position - GameObject.Find("Player").transform.position;
        StartCoroutine(Life());
    }
    
    IEnumerator Life()
    {
        foreach (Text text in myText)
        {
            text.text = _points;
        }

        transform.localScale *= Mathf.Min(1.5f, Mathf.Max(float.Parse(_points) / 100f, 0.4f));

        transform.position += new Vector3(Random.Range(-_offsetMax, _offsetMax), Random.Range(-_offsetMax, _offsetMax), 0);

        while (_yDist > 0)
        {
            transform.position += _dir.normalized * _yDist; //(Vector3)Vector2.up * _yDist;
            _yDist = MathExt.Approach(_yDist,0, Time.deltaTime);
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
