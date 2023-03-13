using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private BattleHUD _battleHUD = null;
    [SerializeField] private TechSelector _techSelector = null;
    [SerializeField] private ItemSelector _itemSelector = null;
    [SerializeField] private UI_Switch _uiSwitch = null;

    public BattleHUD BattleHUD
        => _battleHUD;
    public TechSelector TechSelector
        => _techSelector;
    public ItemSelector ItemSelector
        => _itemSelector;
    public UI_Switch UISwitch
        => _uiSwitch;

    public void ActivateBattleHUD(bool value)
        {
            _battleHUD.SetActivePlayerUi(value);
            _battleHUD.SetActiveEnemyUi(value);

            _battleHUD.SetActiveSelector(value);
            _battleHUD.SetActiveTechSelector(false);
        }

    public void ActivateSwitch(bool value)
    {
        _uiSwitch.gameObject.SetActive(value);
    }
}