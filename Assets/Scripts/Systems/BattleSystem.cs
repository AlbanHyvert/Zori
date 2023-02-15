using Monster.Enum;
using System;
using System.Collections;
using System.IO.Pipes;
using UnityEngine;

public enum BattleState
{
    StartBattle,
    ActionSelect,
    MoveSelection,
    Busy,
    ResolveTurn,
    PartyScreen,
    BattleOver
}

public enum BattleAction
{
    Tech,
    Switch,
    Item,
    Run
}

public class BattleSystem : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private BattleHUD playerHUD = null;
    [SerializeField] private BattleHUD enemyHUD = null;
    [Space(10)]
    [Header("Units")]
    [SerializeField] private ActiveMonster playerUnit = null;
    [SerializeField] private ActiveMonster enemyUnit = null;
    [Space(10)]
    [Header("Dialogue Box")]
    [SerializeField] private DialBox dialogBox = null;
    [Space(10)]
    [Header("Priorities")]
    [SerializeField] private int _switchPriority = 10;
    [SerializeField] private int _itemPriority = 15;
    [SerializeField] private int _runPriority = 20;
    [Space(10)]
    BattleState state = BattleState.StartBattle;
    BattleAction _playerActionState = BattleAction.Tech;
    BattleAction _enemyActionState = BattleAction.Tech;

    Monsters _switchPlayerMonster = null;
    Monsters _switchEnemyMonster = null;

    int _playerPriority = 0;
    int _enemyPriority = 0;

    bool _playerTurnEnded = false;
    bool _enemyTurnEnded = false;

    TeamHolder playerTeam = null;
    Monsters wildZori = null;

    private event Action<bool> OnBattleOver;

    private void Start()
    {
        StartBattle(Player.Instance.Inventory.TeamHolder, Player.Instance.Encounter.WildMonster);
    }

    #region START BATTLE
    private void StartBattle(TeamHolder zoriTeam, Monsters wildZori)
    {
        HUD.Instance.ActivateDialbox(true);

        this.wildZori = wildZori;
        playerTeam = zoriTeam;

        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        //Setup Monsters
        playerUnit.SetMonster(playerTeam.GetHealthyZori(), true);
        enemyUnit.SetMonster(wildZori, true);
        
        //Setup HUD
        playerHUD.Player.Init(playerUnit.CurMonster);
        enemyHUD.Enemy.Init(enemyUnit.CurMonster);

        //Setup Techs
        for (int i = 0; i < HUD.Instance.Selector.ActionBtn.Length; i++)
        {
            if (i > playerUnit.CurMonster.Techs.Length)
                break;

            if (playerUnit.CurMonster.Techs[i] == null)
                break;

            HUD.Instance.Selector.ActionBtn[i].SetTech(playerUnit.CurMonster.Techs[i]);
        }

        dialogBox.AddDialogue($"A wild {enemyUnit.CurMonster.Nickname} appeared!");

        yield return new WaitForSeconds(dialogBox.ReturnDuration());

        ActionSelect();
    }
    #endregion START BATTLE

    #region ACTION SELECTION
    private void ActionSelect()
    {
        //Set State to Action Selection
        state = BattleState.ActionSelect;
        dialogBox.AddDialogue("What will you do?");

        HUD.Instance.ActivateBattleHUD(true);
    }

    public void OnTechSelected(int newTech)
    {
        _playerActionState = BattleAction.Tech;

        state = BattleState.ResolveTurn;

        playerUnit.SetTech(playerUnit.CurMonster.Techs[newTech]);

        StartCoroutine(ResolveTurn(_playerActionState));
    }

    public void OnSwitchSelected(Monsters newZori)
    {
        state = BattleState.ResolveTurn;

        _switchPlayerMonster = newZori;

        StartCoroutine(ResolveTurn(_playerActionState));
    }

    public void OnActionButtonPress(BattleAction action)
    {
        _playerActionState = action;
    }
    #endregion ACTION SELECTION

    #region MOVE SELECTION

    #endregion MOVE SELECTION

    #region RESOLVE TURN

    private void EnemyTech()
    {
        _enemyActionState = BattleAction.Tech;
        state = BattleState.ResolveTurn;

        enemyUnit.SetTech(enemyUnit.CurMonster.Techs[0]);
    }
    
    private IEnumerator ResolveTurn(BattleAction playerAction)
    {
        switch (playerAction)
        {
            case BattleAction.Tech:
                _playerActionState = BattleAction.Tech;
                _playerPriority = playerUnit.TechUsed.Priority;
                break;
            case BattleAction.Switch:
                _playerActionState = BattleAction.Switch;
                _playerPriority = _switchPriority;
                break;
            case BattleAction.Item:
                _playerActionState = BattleAction.Item;
                _playerPriority = _itemPriority;
                break;
            case BattleAction.Run:
                _playerActionState = BattleAction.Run;
                _playerPriority = _runPriority;
                break;
        }

        CheckPriority();

        return null;
    }

    private void CheckPriority()
    {
        if (_playerPriority > _enemyPriority)
        {
            PlayerTurn();

            return;
        }
        else if(_playerPriority < _enemyPriority)
        {
            EnemyTurn();

            return;
        }

        int playerSpeed = playerUnit.CurMonster.Stats.Speed;
        int enemySpeed = enemyUnit.CurMonster.Stats.Speed;
        
        if(playerUnit.CurMonster.Stats.Speed >= enemyUnit.CurMonster.Stats.Speed)
        {
            PlayerTurn();
        }
        else if (playerUnit.CurMonster.Stats.Speed < enemyUnit.CurMonster.Stats.Speed)
        {
            EnemyTurn();
        }
        else if (playerSpeed == enemySpeed)
        {
            int rdm = UnityEngine.Random.Range(0, 100);

            if(rdm <= 50)
            {
                PlayerTurn();
                return;
            }
            else
            {
                EnemyTurn();
                return;
            }
        }
    }

    //Deal Damage to the target by the given Source
    IEnumerator RunTech(ActiveMonster source, ActiveMonster target, obj_Techs tech)
    {
        //Update Source Stamina
        source.CurMonster.Cost(tech.Information.Stamina);

        //yield return playerHUD.UpdateStam();

        dialogBox.AddDialogue($"{source.CurMonster.Nickname} used {tech.Information.Name}!");

        yield return new WaitForSeconds(dialogBox.ReturnDuration());

        //Temp variable for the damage
        int dmg = DealDamage(source, target);

        //Deal damage to the Target
        target.CurMonster.Damage(dmg);

        //yield return enemyHUD.UpdateHP();

        //yield return ShowDamageDetails(damageDetails);

        //Check KO Status
        if (target.CurMonster.Affliction.Equals(e_Afflictions.KO))
        {
            dialogBox.AddDialogue($"{target.CurMonster.Nickname} is KO!");

            yield return new WaitForSeconds(dialogBox.ReturnDuration());

            CheckForBattleOver(target);
        }

        CheckNextTurn();
    }
    #endregion RESOLVE TURN

    #region BUSY
    
    private void PlayerTurn()
    {
        switch (_playerActionState)
        {
            case BattleAction.Tech:
                StartCoroutine(RunTech(playerUnit, enemyUnit, playerUnit.TechUsed));
                break;
            case BattleAction.Switch:
                SwitchZori(playerUnit, _switchPlayerMonster);
                break;
            case BattleAction.Item:
                break;
            case BattleAction.Run:
                break;
        }

        _playerTurnEnded = true;
    }

    private void EnemyTurn()
    {
        switch (_enemyActionState)
        {
            case BattleAction.Tech:
                StartCoroutine(RunTech(enemyUnit, playerUnit, enemyUnit.TechUsed));
                break;
            case BattleAction.Switch:
                SwitchZori(enemyUnit, _switchEnemyMonster);
                break;
            case BattleAction.Item:
                break;
            case BattleAction.Run:
                break;
        }

        _enemyTurnEnded = true;
    }
    
    //Switch
    private IEnumerator SwitchZori(ActiveMonster unit, Monsters newMonster)
    {
        unit.DestroyModel();

        yield return new WaitForSecondsRealtime(1);

        unit.SetMonster(newMonster);

        CheckNextTurn();
    }

    private void CheckNextTurn()
    {
        if(_playerTurnEnded == true)
        {
            EnemyTurn();
        }

        if(_enemyTurnEnded == true)
        {
            PlayerTurn();
        }

        if (_playerTurnEnded == true && _enemyTurnEnded == true)
        {
            ActionSelect();
        }
    }
    #endregion BUSY

    #region PARTY SCREEN

    #endregion PARTY SCREEN

    #region BATTLE OVER
    private void BattleOver(bool value)
    {
        state = BattleState.BattleOver;
        OnBattleOver(value);
    }

    private void CheckForBattleOver(ActiveMonster faintedUnit)
    {
        if (faintedUnit.IsPlayer)
        {
            Monsters nextZori = playerTeam.GetHealthyZori();

            if (nextZori != null)
            {
                //OpenPartyScreen();
            }
            else
            {
                BattleOver(false);
            }
        }
        else
        {
            BattleOver(true);
        }
    }
    #endregion BATTLE OVER

    #region Calculate Damage Done
    private static int DealDamage(ActiveMonster activeSender, ActiveMonster activeReceiver)
    {
        float typeMult = TypeChart.GetEffectiveness(activeSender.TechUsed.Information.Type, activeReceiver.CurMonster.Base.Types[0]) *
                                TypeChart.GetEffectiveness(activeSender.TechUsed.Information.Type, activeReceiver.CurMonster.Base.Types[1]);

        float modifier = CheckStab(activeSender.TechUsed, activeSender.CurMonster.Base.Types) * typeMult;

        float checkStatsDiff = CheckStatsDiff(activeSender.TechUsed, activeSender.CurMonster, activeReceiver.CurMonster);

        int dmg = Mathf.FloorToInt(((((((2 * activeSender.CurMonster.Level) / 5) + 2) * activeSender.TechUsed.Information.Power * checkStatsDiff) / 50 + 2) * modifier));

        return dmg;
    }

    public static int HealValue()
    {
        throw new System.NotImplementedException();
    }

    //---------------Check extra Damage---------------
    //Check the difference between stats and return the value
    private static float CheckStatsDiff(obj_Techs tech, Monsters sender, Monsters receiver)
    {
        switch (tech.Extra.Style)
        {
            case e_Styles.PHYSIC:
                return (float)sender.Stats.Atk / (float)receiver.Stats.Def;
            case e_Styles.SPECIAL:
                return (float)sender.Stats.SpeAtk / (float)receiver.Stats.SpeDef;
        }

        return 1;
    }

    //Check bonus between the tech and the pokemon type
    private static float CheckStab(obj_Techs Tech, e_Types[] zoriTypes)
    {
        for (int i = 0; i < zoriTypes.Length; i++)
        {
            if (Tech.Information.Type == zoriTypes[i])
            {
                return 1.5f;
            }
        }

        return 1f;
    }
    #endregion Calculate Damage Done
}