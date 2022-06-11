using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    private int _score;
    public string Score { get { return _score.ToString(); } }
    [SerializeField] private int _combo = 1;
    public string Combo { get { return _combo.ToString() + ":" + _comboMeter.ToString(); } }
    [SerializeField] private float _comboMeter;
    [SerializeField] private float _comboMeterMax = 40;

    private string _scoreString = "SCORE: ";
    private string _comboString = "X";

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _comboText;
    [SerializeField] private Animator _comboTextAnim;
    [SerializeField] private Image _comboBar;

    public static string scorePopupPath = "Prefabs/Score Popup";


    private void Start()
    {
        UpdateScoreText();
        UpdateComboBar();
        _comboTextAnim = _comboText.GetComponent<Animator>();
        _comboBar.fillAmount = 0;
    }

    private void Update()
    {
        UpdateComboBar();
    }

    public void IncreaseScore(int points, Vector2 position)
    {
        int _pointWorth = points * _combo;
        _score += _pointWorth;
        UpdateScoreText();
        GameObject popupPrefab = Resources.Load(scorePopupPath) as GameObject;
        ScorePopup _popup = Instantiate(popupPrefab, new Vector3(position.x,position.y,-2), new Quaternion(0,0,0,0)).GetComponent<ScorePopup>();
        _popup.Points = _pointWorth.ToString();
    }

    public void IncreaseComboMeter(float amount)
    {
        _comboMeter += amount;
        if (_comboMeter >= _comboMeterMax)
        {
            _combo += 1;
            _comboMeter = 0;
            _comboBar.fillAmount = _comboMeter;
            _comboTextAnim.SetTrigger("ComboIncrease");
            UpdateComboText();
        }
    }

    public void SetComboFromLoad(int combo, float meterFill)
    {
        _combo = combo;
        _comboMeter = meterFill;
        _comboBar.fillAmount = _comboMeter / _comboMeterMax;
        UpdateComboText();
    }

    public void ResetScore()
    {
        _score = 0;
    }

    public void ResetCombo()
    {
        Debug.Log("Reset triggered");
        _combo = 1;
        _comboMeter = 0;
        UpdateComboText();
        _comboTextAnim.SetTrigger("ComboBreak");

    }

    private void UpdateScoreText()
    {
        _scoreText.text = _scoreString + _score.ToString();
    }

    private void UpdateComboText()
    {
        _comboText.text = _comboString + _combo.ToString();
    }

    private void UpdateComboBar()
    {
        float _fill = _comboMeter / _comboMeterMax;
        _comboBar.fillAmount = MathExt.Approach(_comboBar.fillAmount, _fill, Time.deltaTime);
    }
}
