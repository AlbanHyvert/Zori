using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TechSelector : MonoBehaviour
{
    [SerializeField] private BattleSystem _bSystem = null;
    [SerializeField] private ButtonData[] _buttons = new ButtonData[4];

    public ButtonData[] ButtonDatas
        => _buttons;
    
    public void OnTech(int i)
    {
        _bSystem.SetPlayerTech(_buttons[i].Tech);
    }

    [System.Serializable]
    public struct ButtonData
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text text;
        private TechBase tech;

        public TMP_Text Text
            => text;
        public TechBase Tech
            => tech;

        public TechBase SetTech(TechBase newtech)
        {
            return tech = newtech;
        }
    }
}