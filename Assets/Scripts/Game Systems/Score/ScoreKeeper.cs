using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
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
    [SerializeField] private AudioSource _comboUpSFX;
    [SerializeField] private AudioSource _comboBreakSFX;

    [SerializeField] private GameObject _scorePopup;
    private ObjectPool<ScorePopup> _popupPool;
    public ObjectPool<ScorePopup> PopupPool { get { return _popupPool; } }
    private List<ScorePopup> _popupsInUse = new List<ScorePopup>();

    public static string scorePopupPath = "Prefabs/Score Popup";


    private void Start()
    {
        _popupPool = new ObjectPool<ScorePopup>(() =>
        {
            return Instantiate(_scorePopup).GetComponent<ScorePopup>();
        }, popup =>
        {
            popup.gameObject.SetActive(true);
            _popupsInUse.Add(popup);
        }, popup =>
        {
            _popupsInUse.Remove(popup);
            popup.gameObject.SetActive(false);
        },
       popup =>
       {
           Destroy(popup.gameObject);
       }, false, 20
           );

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

        if (Settings.showScorePopup)
        {
            InitialisePopup(_pointWorth, position);
        }
    }

    void InitialisePopup(float points, Vector2 position)
    {
        ScorePopup _popup = _popupPool.Get();
        _popup.transform.position = new Vector3(position.x, position.y, -5);
        _popup.Points = points.ToString();
        _popup.Initialize(this);
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
            _comboUpSFX.Play();
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
        _comboBreakSFX.Play();
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
