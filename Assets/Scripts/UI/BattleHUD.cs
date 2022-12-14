using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private float _updateSpeed = 0.5f;
    [SerializeField] private float _timeByLetters = 1;
    [SerializeField] private Information _information = new Information();
    
    public Information Informations
        => _information;

    [System.Serializable]
    public struct Information
    {
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private TextMeshProUGUI level;
        [Space]
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Slider _staSlider;
        [Space]
        [SerializeField] private TextMeshProUGUI _hpTextValue;
        [SerializeField] private TextMeshProUGUI _staTextValue;
        [Space]
        [SerializeField] private TextMeshProUGUI _dialBox;
        public Slider HpSlider
            => _hpSlider;
        public Slider StaSlider
            => _staSlider;
        
        public TextMeshProUGUI HpTextValue
            => _hpTextValue;
        public TextMeshProUGUI StaTextValue
            => _staTextValue;
        public TextMeshProUGUI Dialbox
            => _dialBox;

//Get and Set Battle HUD
        public void SetInformations(Zori zori)
        {
            name.text = zori.Nickname;
            level.text ="Level" + " " + zori.Level.ToString();

            HpSlider.minValue = 0;
            HpSlider.maxValue = zori.Stats.Hp;
            HpSlider.value = zori.Stats.Hp;

            _staSlider.minValue = 0;
            _staSlider.maxValue = zori.Stats.Sta;
            _staSlider.value = zori.Stats.Sta;

            _hpTextValue.text = HpSlider.value.ToString() + " / " + HpSlider.maxValue.ToString();
            _staTextValue.text = _staSlider.value.ToString() + " / " + _staSlider.maxValue.ToString();
        }
    }

    private int _hpValue = 0;
    private int _staValue = 0;
    private string _dialText = string.Empty;
    private float _time = 0;
    private int _characterIndex = 0;
    
    private void OnUpdateHp()
    {
        if(Mathf.Approximately(_information.HpSlider.value, _hpValue) == true)
        {
            GameManager.Instance.OnUpdateBSystem -= OnUpdateHp;
        }

        SmoothUpdateHp();
    }

    private void OnUpdateSta()
    {
        if(Mathf.Approximately(_information.StaSlider.value, _staValue) == true)
        {
            GameManager.Instance.OnUpdateBSystem -= OnUpdateSta;
        }

        SmoothUpdateSta();
    }

    public void UpdateHp(int newValue)
    {
        _hpValue = newValue;
        GameManager.Instance.OnUpdateBSystem += OnUpdateHp;
    }

    public void UpdateSta(int newValue)
    {
        _staValue = newValue;
        GameManager.Instance.OnUpdateBSystem += OnUpdateSta;
    }

    public void UpdateDialBox(string text)
    {
        _dialText = string.Empty;
        _information.Dialbox.text = string.Empty;
        _time = 0;
        _characterIndex = 0;

        _dialText = text;

        GameManager.Instance.OnUpdateBSystem += OnUpdateDial;
    }

    public void OnUpdateDial()
    {
        _time += _updateSpeed * Time.fixedDeltaTime;

        if(_dialText == _information.Dialbox.text && _time >= _timeByLetters)
        {
            _information.Dialbox.text = string.Empty;
            GameManager.Instance.OnUpdateBSystem -= OnUpdateDial;
        }

        int textSize = _dialText.Length;

        if(_time >= _timeByLetters && _characterIndex < textSize )
        {
            _information.Dialbox.text = _dialText.Substring(0, _characterIndex);
            _characterIndex++;
            _time = 0;
        }
    }

    private void SmoothUpdateHp()
    {
        _information.HpSlider.value = Mathf.Lerp(_information.HpSlider.value, _hpValue, _updateSpeed * Time.fixedDeltaTime);
        _information.HpTextValue.text = ((int)_information.HpSlider.value).ToString() + " / " + _information.HpSlider.maxValue.ToString();
    }

    private void SmoothUpdateSta()
    {
        _information.StaSlider.value = Mathf.Lerp(_information.StaSlider.value, _staValue, _updateSpeed * Time.fixedDeltaTime);
        _information.StaTextValue.text = ((int)_information.StaSlider.value).ToString() + " / " + _information.StaSlider.maxValue.ToString();
    }

}