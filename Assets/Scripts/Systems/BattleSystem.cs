using Monster.Enum;
using System;
using System.Collections;
using UnityEngine;

public enum eBattleState
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
    Tech = 0,
    Switch,
    Item,
    Run,
    Afflicted
}

public class BattleSystem : MonoBehaviour
{
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
    eBattleState state = eBattleState.StartBattle;
    BattleAction _playerActionState = BattleAction.Tech;
    BattleAction _enemyActionState = BattleAction.Tech;

    Monsters _switchPlayerMonster = null;
    Monsters _switchEnemyMonster = null;

    int _playerPriority = 0;
    int _enemyPriority = 0;

    int _playerAfflictedTurn = 0;
    int _enemyAfflictedTurn = 0;

    bool _playerTurnEnded = false;
    bool _enemyTurnEnded = false;

    TeamHolder playerTeam = null;
    Monsters wildZori = null;

    private event Action<bool> OnBattleOver;

    private void Start()
    {
        if(HUD.Instance == null)
        {
            Start();

            return;
        }

        dialogBox = HUD.Instance.DialBox;

        StartBattle(Player.Instance.TeamHolder, Player.Instance.Encounter.WildMonster);

        _playerAfflictedTurn = 0;
        _enemyAfflictedTurn = 0;
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
        HUD.Instance.BattleHUD.Player.Init(playerUnit.Monster);
        HUD.Instance.BattleHUD.Enemy.Init(enemyUnit.Monster);

        //Setup Techs
        SetUpTechs();

        dialogBox.AddDialogue($"A wild {enemyUnit.Monster.Nickname} appeared!");

        HUD.Instance.ActivateBattleHUD(true);

        yield return new WaitForSeconds(dialogBox.ReturnDuration());

        OpenPartyScreen();

        HUD.Instance.ActivateSwitch(false);

        ActionSelect();
    }
    #endregion START BATTLE

    #region ACTION SELECTION
    private void ActionSelect()
    {
        //Set State to Action Selection
        state = eBattleState.ActionSelect;
        dialogBox.AddDialogue("What will you do?");

        SetUpTechs();

        switch (playerUnit.Monster.Affliction)
        {
            case e_Afflictions.FREEZE:
                if (_playerAfflictedTurn == 0)
                {
                    _playerAfflictedTurn = 2;
                }
                
                _playerActionState = BattleAction.Afflicted;

                Debug.Log("You cant attack because you are " + e_Afflictions.FREEZE.ToString());
                return;
            case e_Afflictions.SLEEP:
                if (_playerAfflictedTurn == 0)
                {
                    _playerAfflictedTurn = 5;
                }

                _playerActionState = BattleAction.Afflicted;

                Debug.Log("You cant attack because you are " + e_Afflictions.SLEEP.ToString());
                return;
            case e_Afflictions.KO:
                Debug.Log("You cant attack because you are " + e_Afflictions.KO.ToString());
                return;
            default:
                break;
        }

        HUD.Instance.ActivateBattleHUD(true);
    }

    private void SetUpTechs()
    {
        //Clear all the buttons
        for (int i = 0; i < HUD.Instance.Selector.ActionBtn.Length; i++)
        {
            HUD.Instance.Selector.ActionBtn[i].Clear();
        }

        //Set the techs
        for (int i = 0; i < HUD.Instance.Selector.ActionBtn.Length; i++)
        {
            BattleSelector.Buttons button = HUD.Instance.Selector.ActionBtn[i];
            obj_Techs tech = playerUnit.Monster.Techs[i];

            if (tech == null) return;

            if(tech.Information.Stamina > playerUnit.Monster.Stamina)
            {
                button.Clear();
            }
            else
            {
                button.SetTech(tech);
            }
        }
    }

    private void EnemyTech()
    {
        switch (enemyUnit.Monster.Affliction)
        {
            case e_Afflictions.FREEZE:
                if (_enemyAfflictedTurn == 0)
                {
                    _enemyAfflictedTurn = 2;
                }

                _enemyActionState = BattleAction.Afflicted;

                Debug.Log("Enemy cant attack because they are " + e_Afflictions.FREEZE.ToString());
                return;
            case e_Afflictions.SLEEP:
                if (_enemyAfflictedTurn == 0)
                {
                    _enemyAfflictedTurn = 5;
                }

                _enemyActionState = BattleAction.Afflicted;

                Debug.Log("Enemy cant attack because they are " + e_Afflictions.SLEEP.ToString());
                return;
            case e_Afflictions.KO:
                Debug.Log("Enemy cant attack because they are " + e_Afflictions.KO.ToString());
                return;
            default:
                break;
        }

        _enemyActionState = BattleAction.Tech;

        enemyUnit.SetTech(enemyUnit.Monster.Techs[0]);
    }

    public void OnActionButtonPress(BattleAction action)
    {
        _playerActionState = action;
    }
    #endregion ACTION SELECTION

    #region MOVE SELECTION
    public void OnTechSelected(int newTech)
    {
        _playerActionState = BattleAction.Tech;

        state = eBattleState.ResolveTurn;

        playerUnit.SetTech(playerUnit.Monster.Techs[newTech]);

        HUD.Instance.BattleHUD.SetActiveSelector(false);
        HUD.Instance.BattleHUD.SetActiveTechSelector(false);

        ResolveTurn(_playerActionState);
    }

    public void OnSwitchSelected(int zoriIndex)
    {
        _playerActionState = BattleAction.Switch;

        state = eBattleState.ResolveTurn;

        _switchPlayerMonster = Player.Instance.TeamHolder.Team[zoriIndex];

        HUD.Instance.ActivateSwitch(false);

        ResolveTurn(_playerActionState);
    }
    
    private void OpenPartyScreen()
    {
        //Clear all set monsters
        for (int i = 0; i < HUD.Instance.UISwitch.ActionButton.Length; i++)
        {
            UI_Switch.Buttons buttons = HUD.Instance.UISwitch.ActionButton[i];

            buttons.Clear();
        }

        //Set monster team
        for (int i = 0; i < Player.Instance.TeamHolder.Team.Count; i++)
        {
            UI_Switch.Buttons buttons = HUD.Instance.UISwitch.ActionButton[i];
            Monsters monster = Player.Instance.TeamHolder.Team[i];

            if (monster != null)
            {
                buttons.SetMonster(monster);
            }
            else
            {
                buttons.Clear();
            }
        }

        HUD.Instance.ActivateSwitch(true);
    }
    #endregion MOVE SELECTION

    #region RESOLVE TURN
    
    private IEnumerator ResolveTurn(BattleAction playerAction)
    {
        if (state != eBattleState.ResolveTurn) return null;

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

        int playerSpeedBoost = playerUnit.Monster.StatsBoost.Speed;
        int enemySpeedBoost = enemyUnit.Monster.StatsBoost.Speed;

        int playerSpeed = Mathf.FloorToInt(playerUnit.Monster.Stats.Speed * playerUnit.Monster.StatsBoost.ReturnModificator(playerSpeedBoost));
        int enemySpeed = Mathf.FloorToInt(enemyUnit.Monster.Stats.Speed * enemyUnit.Monster.StatsBoost.ReturnModificator(enemySpeedBoost));
        
        if(playerUnit.Monster.Affliction == e_Afflictions.PARALYSIS)
        {
            playerSpeed = playerSpeed / 2;
        }

        if (enemyUnit.Monster.Affliction == e_Afflictions.PARALYSIS)
        {
            enemySpeed = enemySpeed / 2;
        }

        if (playerUnit.Monster.Stats.Speed >= enemyUnit.Monster.Stats.Speed)
        {
            PlayerTurn();
            return;
        }
        else if (playerUnit.Monster.Stats.Speed < enemyUnit.Monster.Stats.Speed)
        {
            EnemyTurn();
            return;
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
        CheckForStatsBoost(source, target, tech);

        //Update Source Stamina
        source.Monster.Cost(tech.Information.Stamina);

        //yield return playerHUD.UpdateStam();

        dialogBox.AddDialogue($"{source.Monster.Nickname} used {tech.Information.Name}!");

        yield return new WaitForSeconds(dialogBox.ReturnDuration());

        //Temp variable for the damage
        int dmg = DealDamage(source, target);

        //Deal damage to the Target
        target.Monster.Damage(dmg);

        if(dmg > 0 && target.Monster.Affliction == e_Afflictions.SLEEP)
        {
            target.Monster.SetAffliction(e_Afflictions.NONE);

            Debug.Log(target.Monster.Nickname + " " + "is waking up");
        }

        CheckForEffects(tech, target);

        //Check KO Status
        if (target.Monster.Affliction.Equals(e_Afflictions.KO))
        {
            dialogBox.AddDialogue($"{target.Monster.Nickname} is KO!");

            yield return new WaitForSeconds(dialogBox.ReturnDuration());

            CheckForBattleOver(target);
        }

        CheckNextTurn();
    }
    
    private IEnumerator RunAfterTurn(ActiveMonster playerUnit, ActiveMonster enemyUnit)
    {
        if (playerUnit.Monster.Affliction != e_Afflictions.NONE)
        {
            switch (playerUnit.Monster.Affliction)
            {
                case e_Afflictions.BURN:
                    int burnDmg = Mathf.FloorToInt(playerUnit.Monster.Stats.MaxHp * 0.05f);

                    playerUnit.Monster.Damage(burnDmg);
                    break;
                case e_Afflictions.POISON:
                    float percentValue = 0.05f * (1 + _playerAfflictedTurn);

                    int poisonDmg = Mathf.FloorToInt(playerUnit.Monster.Stats.MaxHp * percentValue);

                    playerUnit.Monster.Damage(poisonDmg);

                    if (playerUnit.Monster.Affliction == e_Afflictions.SLEEP)
                    {
                        playerUnit.Monster.SetAffliction(e_Afflictions.NONE);

                        Debug.Log(playerUnit.Monster.Nickname + " " + "is waking up");
                    }

                    _playerAfflictedTurn++;
                    break;
                default:
                    break;
            }
        }

        if (enemyUnit.Monster.Affliction != e_Afflictions.NONE)
        {
            switch (enemyUnit.Monster.Affliction)
            {
                case e_Afflictions.BURN:
                    int burnDmg = Mathf.FloorToInt(enemyUnit.Monster.Stats.MaxHp * 0.05f);

                    enemyUnit.Monster.Damage(burnDmg);
                    break;
                case e_Afflictions.POISON:
                    float percentValue = 0.05f * (1 + _enemyAfflictedTurn);

                    int poisonDmg = Mathf.FloorToInt(enemyUnit.Monster.Stats.MaxHp * percentValue);

                    enemyUnit.Monster.Damage(poisonDmg);

                    if (enemyUnit.Monster.Affliction == e_Afflictions.SLEEP)
                    {
                        enemyUnit.Monster.SetAffliction(e_Afflictions.NONE);

                        Debug.Log(enemyUnit.Monster.Nickname + " " + "is waking up");
                    }

                    _enemyAfflictedTurn++;
                    break;
                default:
                    break;
            }
        }

        return null;
    }
    #endregion RESOLVE TURN

    #region BUSY  
    private void PlayerTurn()
    {
        state = eBattleState.Busy;

        if(_playerActionState == BattleAction.Afflicted)
        {
            switch (playerUnit.Monster.Affliction)
            {
                case e_Afflictions.FREEZE:
                    if (_playerAfflictedTurn <= 0)
                        playerUnit.Monster.SetAffliction(e_Afflictions.NONE);
                    else
                        _playerAfflictedTurn--;
                    break;
                case e_Afflictions.SLEEP:
                    if (_playerAfflictedTurn <= 0)
                        playerUnit.Monster.SetAffliction(e_Afflictions.NONE);
                    else
                        _playerAfflictedTurn--;
                    break;
                default:
                    break;
            }

            CheckNextTurn();

            _playerTurnEnded = true;
        }

        switch (_playerActionState)
        {
            case BattleAction.Tech:
                StartCoroutine(RunTech(playerUnit, enemyUnit, playerUnit.TechUsed));
                break;
            case BattleAction.Switch:
                StartCoroutine(SwitchZori(playerUnit, _switchPlayerMonster));
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
        state = eBattleState.Busy;

        EnemyTech();

        if (_enemyActionState == BattleAction.Afflicted)
        {
            switch (enemyUnit.Monster.Affliction)
            {
                case e_Afflictions.FREEZE:
                    if (_enemyAfflictedTurn <= 0)
                        enemyUnit.Monster.SetAffliction(e_Afflictions.NONE);
                    else
                        _enemyAfflictedTurn--;
                    break;
                case e_Afflictions.SLEEP:
                    if (_enemyAfflictedTurn <= 0)
                        enemyUnit.Monster.SetAffliction(e_Afflictions.NONE);
                    else
                        _enemyAfflictedTurn--;
                    break;
                default:
                    break;
            }

            _enemyTurnEnded = true;

            CheckNextTurn();
        }

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

        unit.Monster.StatsBoost.Reset();
        unit.Monster.Regeneration(unit.Monster.Stats.MaxStamina);

        yield return new WaitForSecondsRealtime(1);

        unit.SetMonster(newMonster, true);

        HUD.Instance.BattleHUD.Player.Init(newMonster);

        CheckNextTurn();
    }

    private void CheckForEffects(obj_Techs tech, ActiveMonster unit)
    {
        e_Afflictions techAffliction = tech.Extra.Effect.affliction;
        e_Types techType = tech.Information.Type;

        Monsters target = unit.Monster;

        if (techAffliction == e_Afflictions.NONE) return;

        if (target.Affliction != e_Afflictions.NONE) return;

        switch (techAffliction)
        {
            case e_Afflictions.PARALYSIS:
                if (target.Base.Types[0] == e_Types.ELECTRO || target.Base.Types[1] == e_Types.ELECTRO) return;

                Debug.Log(target.Nickname + " " + "is" + " " + e_Afflictions.PARALYSIS.ToString());

                target.SetAffliction(e_Afflictions.PARALYSIS);
                break;
            case e_Afflictions.BURN:
                if (target.Base.Types[0] == e_Types.PYRO || target.Base.Types[1] == e_Types.PYRO) return;

                Debug.Log(target.Nickname + " " + "is" + " " + e_Afflictions.BURN.ToString());

                target.SetAffliction(e_Afflictions.BURN);
                break;
            case e_Afflictions.FREEZE:
                if (target.Base.Types[0] == e_Types.CRYO || target.Base.Types[1] == e_Types.CRYO) return;

                if(unit.ColdCount >= 2)
                {
                    Debug.Log(target.Nickname + " " + "is" + " " + e_Afflictions.FREEZE.ToString());

                    unit.ColdCount = 0;

                    target.SetAffliction(e_Afflictions.FREEZE);
                    break;
                }
                else if (unit.ColdCount < 2)
                {
                    Debug.Log(target.Nickname + " " + "is" + " " + "freezing !");

                    unit.ColdCount++;
                }
                break;
            case e_Afflictions.POISON:
                if (target.Base.Types[0] == e_Types.VENO || target.Base.Types[1] == e_Types.VENO) return;

                Debug.Log(target.Nickname + " " + "is" + " " + e_Afflictions.POISON.ToString());

                target.SetAffliction(e_Afflictions.POISON);
                break;
            case e_Afflictions.SLEEP:
                if (target.Base.Types[0] == e_Types.MENTAL || target.Base.Types[1] == e_Types.MENTAL) return;

                Debug.Log(target.Nickname + " " + "is" + " " + e_Afflictions.SLEEP.ToString());

                target.SetAffliction(e_Afflictions.SLEEP);
                break;
            default:
                break;
        }

    }

    private void CheckForStatsBoost(ActiveMonster source, ActiveMonster target, obj_Techs tech)
    {
        int atk = 0;
        int def = 0;
        int speAtk = 0;
        int speDef = 0;
        int speed = 0;

        switch (tech.Extra.Target)
        {
            case e_Targets.SELF:
                atk = tech.Extra.Effect.statsBoost.Atk;
                def = tech.Extra.Effect.statsBoost.Def;
                speAtk = tech.Extra.Effect.statsBoost.SpAtk;
                speDef = tech.Extra.Effect.statsBoost.SpDef;
                speed = tech.Extra.Effect.statsBoost.Speed;

                source.Monster.StatsBoost.UpdateBoost(atk, def, speAtk, speDef, speed);
                break;
            case e_Targets.OPPONENT:
                atk = tech.Extra.Effect.statsBoost.Atk;
                def = tech.Extra.Effect.statsBoost.Def;
                speAtk = tech.Extra.Effect.statsBoost.SpAtk;
                speDef = tech.Extra.Effect.statsBoost.SpDef;
                speed = tech.Extra.Effect.statsBoost.Speed;

                target.Monster.StatsBoost.UpdateBoost(atk, def, speAtk, speDef, speed);
                break;
            case e_Targets.ANY:
                break;
            default:
                break;
        }
    }

    private void CheckNextTurn()
    {
        if (_playerTurnEnded == true && _enemyTurnEnded == true)
        {
            _playerTurnEnded = false;
            _enemyTurnEnded = false;

            RunAfterTurn(playerUnit, enemyUnit);

            ActionSelect();

            return;
        }

        if (_playerTurnEnded == true)
        {
            EnemyTurn();

            return;
        }

        if(_enemyTurnEnded == true)
        {
            PlayerTurn();

            return;
        }
    }
    #endregion BUSY

    #region PARTY SCREEN

    #endregion PARTY SCREEN

    #region BATTLE OVER
    private void BattleOver(bool value)
    {
        state = eBattleState.BattleOver;

        //OnBattleOver(value);
    }

    private void GainXP(ActiveMonster unit)
    {
        playerUnit.Monster.AddExperience(unit.Monster.Base.GivenXp);
    }

    private void CheckForBattleOver(ActiveMonster faintedUnit)
    {
        if (faintedUnit.IsPlayer)
        {
            Monsters nextZori = playerTeam.GetHealthyZori();

            if (nextZori != null)
            {
                OpenPartyScreen();
            }
            else
            {
                GameManager.Instance.LoadWorldScene();

                BattleOver(false);
            }
        }

        /*if (faintedUnit.IsNpc)
        {
            GainXP(faintedUnit);

            Monsters nextZori = NpcTeam.GetHealthyZori();


            else
            {
                GameManager.Instance.LoadWorldScene();
                BattleOver(false);
            }
        }*/
        else
        {
            GainXP(faintedUnit);

            GameManager.Instance.LoadWorldScene();

            BattleOver(true);
        }
    }
    #endregion BATTLE OVER

    #region Calculate Damage Done
    private static int DealDamage(ActiveMonster activeSender, ActiveMonster activeReceiver)
    {
        if(activeSender.TechUsed.Extra.Style == e_Styles.TACTIC)
        {
            return 0;
        }

        float typeMult = TypeChart.GetEffectiveness(activeSender.TechUsed.Information.Type, activeReceiver.Monster.Base.Types[0]) *
                                TypeChart.GetEffectiveness(activeSender.TechUsed.Information.Type, activeReceiver.Monster.Base.Types[1]);

        float modifier = CheckStab(activeSender.TechUsed, activeSender.Monster.Base.Types) * typeMult;

        float checkStatsDiff = CheckStatsDiff(activeSender.TechUsed, activeSender.Monster, activeReceiver.Monster);

        int dmg = Mathf.FloorToInt(((((((2 * activeSender.Monster.Level) / 5) + 2) * activeSender.TechUsed.Information.Power * checkStatsDiff) / 50 + 2) * modifier));

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
                float senderAtk = sender.Stats.Atk;

                if(sender.Affliction == e_Afflictions.BURN)
                {
                    senderAtk = senderAtk - (senderAtk * 0.2f);
                }

                int sdAtk = sender.StatsBoost.Atk;
                int rcDef = receiver.StatsBoost.Def;

                return (float)(senderAtk * sender.StatsBoost.ReturnModificator(sdAtk)) / 
                    (float)(receiver.Stats.Def * receiver.StatsBoost.ReturnModificator(rcDef));

            case e_Styles.SPECIAL:
                float senderSpeAtk = sender.Stats.Atk;

                if (sender.Affliction == e_Afflictions.BURN)
                {
                    senderSpeAtk = senderSpeAtk - (senderSpeAtk * 0.2f);
                }

                int sdSpeAtk = sender.StatsBoost.SpAtk;
                int rcSpeDef = receiver.StatsBoost.SpDef;

                return (float)(senderSpeAtk * sender.StatsBoost.ReturnModificator(sdSpeAtk)) /
                    (float)(receiver.Stats.SpeDef * receiver.StatsBoost.ReturnModificator(rcSpeDef));
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