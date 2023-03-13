using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_BattleInformation : MonoBehaviour
{
    [Header("Information")]
    [SerializeField] private TextMeshProUGUI _name = null;
    [SerializeField] private TextMeshProUGUI _level = null;

    [Space, Header("Sliders")]
    [SerializeField] private Slider _hpSlider = null;
    [SerializeField] private Slider _staSlider = null;
    [SerializeField] private float _speed = 2f;

    [Space, Header("Slider Text")]
    [SerializeField] private TextMeshProUGUI _hpText = null;
    [SerializeField] private TextMeshProUGUI _staText = null;

    private bool _hasUpdated = false;
    private int newHp = 0;
    private int newStamina = 0;

    public bool HasUpdated
        => _hasUpdated;

    public void Init(Monsters zori)
    {
        _name.text = zori.Nickname;
        _level.text = zori.Level.ToString();

        _hpSlider.minValue = 0;
        _hpSlider.maxValue = zori.Stats.MaxHp;
        _hpSlider.value = zori.Hp;

        _staSlider.minValue = 0;
        _staSlider.maxValue = zori.Stats.MaxStamina;
        _staSlider.value = zori.Stamina;

        _hpText.text = ((int)_hpSlider.value).ToString() + "/" + ((int)_hpSlider.maxValue).ToString();
        _staText.text = ((int)_staSlider.value).ToString() + "/" + ((int)_staSlider.maxValue).ToString();

        zori.OnUpdateHealth += Health;
        zori.OnUpdateStamina += Stamina;

        _hasUpdated = false;
    }

    private void Health(int value)
    {
        _hasUpdated = false;

        newHp = value;

        Debug.Log("new hp " + newHp);

        GameManager.Instance.OnUpdateHUD += UpdateHp;
    }

    private void Stamina(int value)
    {
        _hasUpdated = false;

        newStamina = value;

        GameManager.Instance.OnUpdateHUD += UpdateStamina;
    }

    private void UpdateHp(float time)
    {
        float startValue = _hpSlider.value;
        float endValue = newHp;

        float sliderValue = Mathf.Lerp(startValue, endValue, _speed);

        Debug.Log("Hp: " + sliderValue.ToString() + "/" + "newHp: " + endValue);

        _hpSlider.value = (int)sliderValue;

        float median = endValue - _hpSlider.value;

        Debug.Log("median: " + median);

        if(median < 0.5f)
        {
            _hpSlider.value = newHp;
            _hasUpdated = true;

            GameManager.Instance.OnUpdateHUD -= UpdateHp;
        }

        _hpText.text = ((int)_hpSlider.value).ToString() + "/" + ((int)_hpSlider.maxValue).ToString();
    }

    private void UpdateStamina(float time)
    {
        float startValue = _staSlider.value;
        float endValue = newStamina;

        float sliderValue = Mathf.Lerp(startValue, endValue, time);

        _staSlider.value = (int)sliderValue;

        if(Mathf.Approximately(sliderValue, endValue))
        {
            _staSlider.value = newStamina;
            _hasUpdated = true;

            GameManager.Instance.OnUpdateHUD -= UpdateStamina;
        }

        _staText.text = ((int)_staSlider.value).ToString() + "/" + ((int)_staSlider.maxValue).ToString();
    }
}