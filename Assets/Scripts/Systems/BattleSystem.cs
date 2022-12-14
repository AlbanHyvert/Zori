using UnityEngine;
using System;

public class BattleSystem : BattleStateMachine
{
    [SerializeField] private ActiveMonster _activePlayer = null;
    [SerializeField] private ActiveMonster _activeEnemy = null;

#region Variables
    //private Npc _npc = null;
    private Player _player = null;

    public ActiveMonster ActivePlayer
        => _activePlayer;
    public ActiveMonster ActiveEnemy
        => _activeEnemy;

    private obj_Techs _allyTech = null;
    private obj_Techs _enemyTech = null;

    private int _allyActionPrio = 0;
    private int _enemyActionPrio = 0;
#endregion Variables

#region Properties
    public obj_Techs AllyTech
        => _allyTech;
    public obj_Techs EnemyTech
        => _enemyTech;
    
    public int AllyActionPrio
        => _allyActionPrio;
    public int EnemyActionPrio
        => _enemyActionPrio;
#endregion Properties

    public bool AllyTurnEnded {get; set;}
    public bool EnemyTurnEnded {get; set;}
    public bool PlayerHasWon {get; set;}

    private event Action _onActionSelected;
    public event Action OnActionSelected
    {
        add
        {
            _onActionSelected -= value;
            _onActionSelected += value;
        }
        remove
        {
            _onActionSelected -= value;
        }
    }

    private void Start()
    {
        _player = Player.Instance;

        SetNewMonsters(_activePlayer, _player.Inventory.TeamHolder.GetHealthyZori());
        SetNewMonsters(_activeEnemy, _player.Encounter.WildMonster);

        _allyActionPrio = 0;
        _enemyActionPrio = 0;

        AllyTurnEnded = false;
        EnemyTurnEnded = false;
        PlayerHasWon = false;

        HUD.Instance.Selector.SetBattleSystem(this);
        HUD.Instance.UISwitch.SetBattleSystem(this);

        SetState(new StartBattleState(this));
    }

    public void SetPlayerTech(obj_Techs tech)
    {
        if(tech == null)
            return;
        
        _allyTech = tech;

        SetAllyActionPrio(_allyTech.Priority);

        if(_onActionSelected != null)
            _onActionSelected();
    }
    public void SetEnemyTech(obj_Techs tech)
    {
        _enemyTech = tech;

        SetEnemyActionPrio(_enemyTech.Priority);
    }

    public void SetAllyActionPrio(int value)
        => _allyActionPrio = value;
    public void SetEnemyActionPrio(int value)
        => _enemyActionPrio = value;

    public void SetNewMonsters(ActiveMonster activeMonster, Monsters monsters)
    {
       switch (activeMonster.IsPlayer)
       {
        case true:
            activeMonster.SetMonster(monsters);
            break;

        case false:
            activeMonster.SetMonster(monsters);
            break;
       }
    }
}