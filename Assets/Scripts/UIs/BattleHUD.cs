using UnityEngine;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private GameObject _uiActSelector = null;
    [SerializeField] private GameObject _uiTechSelector = null;
    [Space]
    [SerializeField] private GameObject _uiPlayer = null;
    [SerializeField] private GameObject _uiEnemy = null;

    public void SetActiveSelector(bool value)
        => _uiActSelector.SetActive(value);
    public void SetActiveTechSelector(bool value)
        => _uiTechSelector.SetActive(value);
    public void SetActivePlayerUi(bool value)
        => _uiPlayer.SetActive(value);
    public void SetActiveEnemyUi(bool value)
        => _uiEnemy.SetActive(value);

    public void OnRun()
    {
        Player.Instance.ExitBattle();
        LoadingSceneManager.Instance.LoadLevelAsync("EncounterScene");
    }
}