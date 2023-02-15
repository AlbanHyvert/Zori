using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private PlayerHUD _player = new PlayerHUD();
    [SerializeField] private EnemyHUD _enemy = new EnemyHUD();
    [Space]
    [SerializeField] private GameObject _uiActSelector = null;
    [SerializeField] private GameObject _uiTechSelector = null;
    [Space]
    [SerializeField] private GameObject _uiPlayer = null;
    [SerializeField] private GameObject _uiEnemy = null;

    public PlayerHUD Player
        => _player;
    public EnemyHUD Enemy
        => _enemy;

    public void SetActiveSelector(bool value)
        => _uiActSelector.SetActive(value);
    public void SetActiveTechSelector(bool value)
        => _uiTechSelector.SetActive(value);
    public void SetActivePlayerUi(bool value)
        => _uiPlayer.SetActive(value);
    public void SetActiveEnemyUi(bool value)
        => _uiEnemy.SetActive(value);

#region Structs
    [System.Serializable]
    public struct PlayerHUD
    {
        [Header("Information")]
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Level;

        [Space, Header("Sliders")]
        public Slider HpSlider;
        public Slider StaSlider;

        [Space, Header("Slider Text")]
        public TextMeshProUGUI HpText;
        public TextMeshProUGUI StaText;

        private int _hp;
        private int _stamina;

        public void Init(Monsters zori)
        {
            Name.text = zori.Nickname;
            Level.text = zori.Level.ToString();

            HpSlider.minValue = 0;
            HpSlider.maxValue = zori.Stats.MaxHp;
            HpSlider.value = zori.Hp;

            StaSlider.minValue = 0;
            StaSlider.maxValue = zori.Stats.MaxStamina;
            StaSlider.value = zori.Stamina;

            HpText.text = ((int)HpSlider.value).ToString() + " / " + HpSlider.maxValue.ToString();
            StaText.text = ((int)StaSlider.value).ToString() + " / " + StaSlider.maxValue.ToString();

            _hp = zori.Hp;
            _stamina = zori.Stamina;
            
            zori.OnUpdateHealth += Hp;
            zori.OnUpdateStamina += Stamina;
        }

        private void Hp(int value)
        {
            _hp = value;
            GameManager.Instance.OnUpdateHUD += UpdateHp;
        }

        private void Stamina(int value)
        {
            _stamina = value;
            GameManager.Instance.OnUpdateHUD += UpdateStamina;
        }

        private void UpdateHp(float time)
        {
            HpSlider.value = Mathf.Lerp(HpSlider.value, _hp, time);
            HpText.text = ((int)HpSlider.value).ToString() + " / " + HpSlider.maxValue.ToString();

            if(Mathf.Approximately(HpSlider.value, _hp))
            {
                HpSlider.value = _hp;
                HpText.text = ((int)HpSlider.value).ToString() + " / " + HpSlider.maxValue.ToString();
                GameManager.Instance.OnUpdateHUD -= UpdateHp;
            }
        }

        private void UpdateStamina(float time)
        {
            StaSlider.value = Mathf.Lerp(StaSlider.value, _stamina, time);
            StaText.text = ((int)StaSlider.value).ToString() + " / " + StaSlider.maxValue.ToString();

            if(Mathf.Approximately(StaSlider.value, _stamina))
            {
                StaSlider.value = _stamina;
                StaText.text = ((int)StaSlider.value).ToString() + " / " + StaSlider.maxValue.ToString();
                GameManager.Instance.OnUpdateHUD -= UpdateStamina;
            }
        }
    }

    [System.Serializable]
    public struct EnemyHUD
    {
        [Header("Information")]
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Level;

        [Space, Header("Sliders")]
        public Slider HpSlider;
        public Slider StaSlider;

        [Space, Header("Slider Text")]
        public TextMeshProUGUI HpText;
        public TextMeshProUGUI StaText;

        private int _hp;
        private int _stamina;

        public void Init(Monsters zori)
        {
            Name.text = zori.Nickname;
            Level.text = zori.Level.ToString();

            HpSlider.minValue = 0;
            HpSlider.maxValue = zori.Stats.MaxHp;
            HpSlider.value = zori.Hp;

            StaSlider.minValue = 0;
            StaSlider.maxValue = zori.Stats.MaxStamina;
            StaSlider.value = zori.Stamina;

            HpText.text = ((int)HpSlider.value).ToString() + " / " + HpSlider.maxValue.ToString();
            StaText.text = ((int)StaSlider.value).ToString() + " / " + StaSlider.maxValue.ToString();

            _hp = zori.Hp;
            _stamina = zori.Stamina;

            zori.OnUpdateHealth += Hp;
            zori.OnUpdateStamina += Stamina;
        }

        private void Hp(int value)
        {
            _hp = value;
            GameManager.Instance.OnUpdateHUD += UpdateHp;
        }

        private void Stamina(int value)
        {
            _stamina = value;
            GameManager.Instance.OnUpdateHUD += UpdateStamina;
        }

        private void UpdateHp(float time)
        {
            HpSlider.value = Mathf.Lerp(HpSlider.value, _hp, time);
            HpText.text = ((int)HpSlider.value).ToString() + " / " + HpSlider.maxValue.ToString();

            if(Mathf.Approximately(HpSlider.value, _hp))
            {
                HpSlider.value = _hp;
                HpText.text = ((int)HpSlider.value).ToString() + " / " + HpSlider.maxValue.ToString();
                GameManager.Instance.OnUpdateHUD -= UpdateHp;
            }
        }

        private void UpdateStamina(float time)
        {
            StaSlider.value = Mathf.Lerp(StaSlider.value, _stamina, time);
            StaText.text = ((int)StaSlider.value).ToString() + " / " + StaSlider.maxValue.ToString();

            if(Mathf.Approximately(StaSlider.value, _stamina))
            {
                StaSlider.value = _stamina;
                StaText.text = ((int)StaSlider.value).ToString() + " / " + StaSlider.maxValue.ToString();
                GameManager.Instance.OnUpdateHUD -= UpdateStamina;
            }
        }
    }
#endregion Structs
}