using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashText : MonoBehaviour
{
    private Text _mytext;
    private bool _visible;
    private float _timer;
    [SerializeField] private float _flashSpeed;

    void Start()
    {
        _mytext = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _flashSpeed)
        {
            _visible = !_visible;
            _timer -= _flashSpeed;
            _mytext.enabled = _visible;
        }
    }
}
