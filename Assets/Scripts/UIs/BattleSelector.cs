using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BattleSelector : MonoBehaviour
{
    [SerializeField] private Buttons[] _buttons = new Buttons[4];

    private BattleSystem _battleSystem = null;

    [System.Serializable]
    public struct Buttons
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI cost;

        private obj_Techs tech;

        public obj_Techs Tech
            => tech;

        public obj_Techs SetTech(obj_Techs newTech)
        {
            tech = newTech;
            
            name.text = tech.Information.Name;
            //Set Icon
            cost.text = tech.Information.Stamina.ToString();

            return tech;
        }
    
        public void Clear()
        {
            tech = null;
            
            name.text = string.Empty;
            //Set Icon
            cost.text = "00";
        }
    }

    public Buttons[] ActionBtn
        => _buttons;

    public BattleSystem SetBattleSystem(BattleSystem battleSystem)
        => _battleSystem = battleSystem;

    public void Action(int i)
    {
        obj_Techs tech = _buttons[i].Tech;

        if(tech == null)
            return;

        _battleSystem.SetPlayerTech(tech);
    }
}