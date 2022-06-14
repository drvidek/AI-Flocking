using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainbowText : MonoBehaviour
{
    private Text _mytext;
    private float _myHue;
    [SerializeField] private float _hueShiftSpd;
    private Color32 myColor;

    void Start()
    {
        _mytext = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _myHue += _hueShiftSpd * Time.deltaTime;
        if (_myHue > 1f)
            _myHue--;
        myColor = Color.HSVToRGB(_myHue, 0.15f,1f);
        _mytext.color = myColor;
    }
}
