using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private float _updateSpeed = 0.5f;
    [SerializeField] private Information _information = new Information();
    
    public Information Informations
        => _information;

    [System.Serializable]
    public struct Information
    {
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private TextMeshProUGUI level;
        [Space]
        [SerializeField] private Slider HpSlider;
        [SerializeField] private Slider StaSlider;
        [Space]
        [SerializeField] private TextMeshProUGUI HpTextValue;
        [SerializeField] private TextMeshProUGUI StaTextValue;

//Get and Set Battle HUD
        public void SetInformations(Zori zori)
        {
            name.text = zori.Nickname;
            level.text ="Level" + " " + zori.Level.ToString();

            HpSlider.minValue = 0;
            HpSlider.maxValue = zori.Stats.Hp;
            HpSlider.value = zori.Stats.Hp;

            StaSlider.minValue = 0;
            StaSlider.maxValue = zori.Stats.Sta;
            StaSlider.value = zori.Stats.Sta;

            HpTextValue.text = HpSlider.value.ToString() + " / " + HpSlider.maxValue.ToString();
            StaTextValue.text = StaSlider.value.ToString() + " / " + StaSlider.maxValue.ToString();
        }

//Update the HP value in the HUD
        public void UpdateHp(int newValue)
        {
            HpSlider.value = newValue;
            //HpSlider.value = Mathf.MoveTowards(HpSlider.value, newValue, 0.5f * Time.deltaTime);
            HpTextValue.text = HpSlider.value.ToString() + " / " + HpSlider.maxValue.ToString(); 
        }

//Update the Stamina value in the HUD
        public void UpdateStamina(int newValue)
        {
            StaSlider.value = newValue;
            //StaSlider.value = Mathf.MoveTowards(StaSlider.value, newValue, 0.5f * Time.deltaTime);
            StaTextValue.text = StaSlider.value.ToString() + " / " + StaSlider.maxValue.ToString();
        }

        private IEnumerator SmoothUpdate()
        {
            return null;
        }
    }
}
