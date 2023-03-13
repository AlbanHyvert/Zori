using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private HUD _hud = null;
    [SerializeField] private ActiveMonster _playerUnit = null;
    [SerializeField] private ActiveMonster _enemyUnit = null;
    [Space(10)]
    [SerializeField] private Priorities _playerPriorities = new Priorities();
    [SerializeField] private Priorities _enemyPriorities = new Priorities();

    private int _playerPriority = 0;
    private int _enemyPriority = 0;
    private TeamHolder _playerTeam = null;
    private TeamHolder _enemyTeam = null;
    private ActionType _playerAction = ActionType.NONE;
    private ActionType _enemyAction = ActionType.NONE;
    private BattleState _currentState = null;

#region PROPERTIES
    public HUD HUD 
        => _hud;

    public ActiveMonster PlayerUnit
    => _playerUnit;
    public ActiveMonster EnemyUnit
    => _enemyUnit;

    public ActionType PlayerAction
    => _playerAction;
    public ActionType EnemyAction
    => _enemyAction;

    public TeamHolder PlayerTeam
    => _playerTeam;
    public TeamHolder EnemyTeam
    => _enemyTeam;

    public int PlayerPriority
    => _playerPriority;
    public int EnemyPriority
    => _enemyPriority;

    public Priorities EnemyPriorities
    => _enemyPriorities;
#endregion PROPERTIES

    private void Start()
    {
        SetTeam();

        DialogueManager.Instance.StartDialogue("A battle is about to start !");

        SetState(new StartBattleState());
    }

    private void SetTeam()
    {
        _playerTeam = Player.Instance.TeamHolder;
    }

#region MOVE SELECTION
    public void ChooseTech(int value)
    {
        obj_Techs tech = HUD.TechSelector.ActionBtn[value].Tech;

        _playerUnit.SetTech(tech);

        SetPActionType(ActionType.MOVE);

        SetPlayerPrio(tech.Priority);

        SetState(new ResolveTurnState());
    }

    public void ChooseSwitch(int index)
    {
        Monsters monster = _playerTeam.Team[index];

        _playerUnit.SetSwitch(monster);

        SetPActionType(ActionType.SWITCH);

        SetPlayerPrio(_playerPriorities.switchPrio);

        SetState(new ResolveTurnState());
    }

    public void ChooseItem(int index)
    {
        obj_Item item = HUD.ItemSelector.ActionBtn[index].Item;

        _playerUnit.SetItem(item);

        SetPActionType(ActionType.ITEM);

        SetPlayerPrio(_playerPriorities.itemPrio);

        SetState(new ResolveTurnState());
    }

    public void ChooseRun()
    {
        SetPActionType(ActionType.RUN);

        SetPlayerPrio(_playerPriorities.RunPrio);

        SetState(new ResolveTurnState());
    }
#endregion MOVE SELECTION

#region BATTLE OVER
    public void GainXP(ActiveMonster unit)
    {
        _playerUnit.Monster.AddExperience(unit.Monster.Base.GivenXp);
    }
#endregion BATTLE OVER

#region  SET PROPERTIES
    public void SetPActionType(ActionType type)
    => _playerAction = type;
    public void SetEActionType(ActionType type)
    => _enemyAction = type;
    public void SetPlayerPrio(int value)
    => _playerPriority = value;
    public void SetEnemyPrio(int value)
    => _enemyPriority = value;
#endregion SET PROPERTIES
    
    public void SetState(BattleState state) 
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = state;

        if(_currentState == null) return;

        _currentState.SetManager(this);

        StartCoroutine(_currentState.Enter());
    }

    public void Reset() {
        // Reset Pokemon stats and HP
    }

    public enum ActionType
    {
        NONE,
        MOVE,
        ITEM,
        SWITCH,
        RUN,
        AFFLICTED
    }

    [System.Serializable]
    public struct Priorities
    {
        public int switchPrio;
        public int itemPrio;
        public int RunPrio;
    }
}
