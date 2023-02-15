using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Switch : MonoBehaviour
{
    [SerializeField] private Buttons[] _buttons = new Buttons[4];

    private BattleSystem _battleSystem;

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
            monster = newMonster;
            
            name.text = monster.Nickname;
            //level.text = monster.Level.ToString();
            //Set Icon
            //stamina.text = monster.Stamina.ToString();

            return monster;
        }
    
        public void Clear()
        {
            monster = null;
            
            name.text = string.Empty;
            //level.text = string.Empty;
            //Set Icon
            //stamina.text = "00";
        }
    }

    public BattleSystem SetBattleSystem(BattleSystem battleSystem)
        => _battleSystem = battleSystem;

    public void OnSwitch(int i)
    {
         Monsters monster = _buttons[i].Monster;

        if(monster == null)
            return;

       // _battleSystem.SetNewMonsters(monster);
    }
}
