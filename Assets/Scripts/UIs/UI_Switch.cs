using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Switch : MonoBehaviour
{
    [SerializeField] private Buttons[] _buttons = new Buttons[4];

    public Buttons[] ActionButton
        => _buttons;

    [System.Serializable]
    public struct Buttons
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI level;
        [SerializeField] private TextMeshProUGUI stamina;

        private Monsters monster;

        public Monsters Monster
            => monster;

        public Monsters SetMonster(Monsters newMonster)
        {
            button.enabled = true;

            monster = newMonster;
            
            name.text = monster.Nickname;
            level.text = monster.Level.ToString();
            //Set Icon
            //stamina.text = monster.Stamina.ToString();

            return monster;
        }
    
        public void Clear()
        {
            monster = null;
            
            name.text = "- - - -";
            level.text = "00";
            button.enabled = false;
            //Set Icon
            //stamina.text = "00";
        }
    }
}
