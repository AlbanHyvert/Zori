using UnityEngine;

public class HUD : Singleton<HUD>
{
    [SerializeField] private DialBox _dialBox = null;
    [SerializeField] private BattleHUD _battleHUD = null;
    [SerializeField] private BattleSelector _battleSelector = null;
    [SerializeField] private UI_Switch _uiSwitch = null;

    public DialBox DialBox
        => _dialBox;
    public BattleHUD BattleHUD
        => _battleHUD;
    public BattleSelector Selector
        => _battleSelector;
    public UI_Switch UISwitch
        => _uiSwitch;

    protected override void Awake()
    {
        base.Awake();
        ActivateBattleHUD(false);
        ActivateDialbox(false);
    }

    public void ActivateDialbox(bool value)
        => _dialBox.gameObject.SetActive(value);
    
    public void ActivateBattleHUD(bool value)
        {
            _battleHUD.SetActivePlayerUi(false);
            _battleHUD.SetActiveOtherUi(false);

            _battleHUD.SetActiveSelector(false);
            _battleHUD.SetActiveTechSelector(false);
        }

    public void ActivateSwitch(bool value)
    {
        _uiSwitch.gameObject.SetActive(value);
    }
}