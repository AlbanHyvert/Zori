using System.Collections;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textZone = null;
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
    private BattleState _currentState;

#region PROPERTIES
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

    public struct Priorities
    {
        public int switchPrio;
        public int itemPrio;
        public int RunPrio;
    }

    private void Start()
    {
        DialogueManager.Instance.AddTextBox(_textZone);

        DialogueManager.Instance.StartDialogue("A battle is about to start !");

        SetState(new StartBattleState());
    }

#region MOVE SELECTION
    public void ChooseTech(int value)
    {
        obj_Techs tech = _playerUnit.Monster.Techs[value];

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

    public void ChooseRun()
    {
        SetPActionType(ActionType.RUN);

        SetPlayerPrio(_playerPriorities.RunPrio);

        SetState(new ResolveTurnState());
    }
#endregion MOVE SELECTION

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

#region BATTLE OVER
    public IEnumerator Victory()
    {
        DialogueManager.Instance.StartDialogue("You have won !");

        while(DialogueManager.Instance.IsTyping) yield return null;

        DialogueManager.Instance.StartDialogue(_playerUnit.Monster.Nickname + " " + "has gained" + " " + _enemyUnit.Monster.Base.GivenXp.ToString() + " !");

        while(DialogueManager.Instance.IsTyping) yield return null;

        LoadingSceneManager.Instance.LoadLevelAsync("EncounterScene");
    }

    public IEnumerator Lost()
    {
        DialogueManager.Instance.StartDialogue("You have lost !");

        while(DialogueManager.Instance.IsTyping) yield return null;

        DialogueManager.Instance.StartDialogue("You do not have any healthy zori anymore.");

        while(DialogueManager.Instance.IsTyping) yield return null;

        LoadingSceneManager.Instance.LoadLevelAsync("EncounterScene");
    }

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
    
    public enum ActionType
    {
        NONE,
        MOVE,
        ITEM,
        SWITCH,
        RUN,
        AFFLICTED
    }
}
