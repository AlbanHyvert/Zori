using Monster.Enum;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    BattleState state = BattleState.StartBattle;
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

        StartBattle(Player.Instance.Inventory.TeamHolder, Player.Instance.Encounter.WildMonster);

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
        HUD.Instance.BattleHUD.Player.Init(playerUnit.CurMonster);
        HUD.Instance.BattleHUD.Enemy.Init(enemyUnit.CurMonster);

        //Setup Techs
        SetUpTechs();

        dialogBox.AddDialogue($"A wild {enemyUnit.CurMonster.Nickname} appeared!");

        HUD.Instance.ActivateBattleHUD(true);

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

        SetUpTechs();

        switch (playerUnit.CurMonster.Affliction)
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
            obj_Techs tech = playerUnit.CurMonster.Techs[i];

            if (tech == null) return;

            if(tech.Information.Stamina > playerUnit.CurMonster.Stamina)
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
        switch (enemyUnit.CurMonster.Affliction)
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

        enemyUnit.SetTech(enemyUnit.CurMonster.Techs[0]);
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

        state = BattleState.ResolveTurn;

        playerUnit.SetTech(playerUnit.CurMonster.Techs[newTech]);

        HUD.Instance.BattleHUD.SetActiveSelector(false);
        HUD.Instance.BattleHUD.SetActiveTechSelector(false);

        ResolveTurn(_playerActionState);
    }

    public void OnSwitchSelected(Monsters newZori)
    {
        _playerActionState = BattleAction.Switch;

        state = BattleState.ResolveTurn;

        _switchPlayerMonster = newZori;

        StartCoroutine(ResolveTurn(_playerActionState));
    }
    #endregion MOVE SELECTION

    #region RESOLVE TURN
    
    private IEnumerator ResolveTurn(BattleAction playerAction)
    {
        if (state != BattleState.ResolveTurn) return null;

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
        
        if(playerUnit.CurMonster.Affliction == e_Afflictions.PARALYSIS)
        {
            playerSpeed = playerSpeed / 2;
        }

        if (enemyUnit.CurMonster.Affliction == e_Afflictions.PARALYSIS)
        {
            enemySpeed = enemySpeed / 2;
        }

        if (playerUnit.CurMonster.Stats.Speed >= enemyUnit.CurMonster.Stats.Speed)
        {
            PlayerTurn();
            return;
        }
        else if (playerUnit.CurMonster.Stats.Speed < enemyUnit.CurMonster.Stats.Speed)
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
        //Update Source Stamina
        source.CurMonster.Cost(tech.Information.Stamina);

        //yield return playerHUD.UpdateStam();

        dialogBox.AddDialogue($"{source.CurMonster.Nickname} used {tech.Information.Name}!");

        yield return new WaitForSeconds(dialogBox.ReturnDuration());

        //Temp variable for the damage
        int dmg = DealDamage(source, target);

        //Deal damage to the Target
        target.CurMonster.Damage(dmg);

        if(dmg > 0 && target.CurMonster.Affliction == e_Afflictions.SLEEP)
        {
            target.CurMonster.SetAffliction(e_Afflictions.NONE);

            Debug.Log(target.CurMonster.Nickname + " " + "is waking up");
        }

        CheckForEffects(tech, target);

        //Check KO Status
        if (target.CurMonster.Affliction.Equals(e_Afflictions.KO))
        {
            dialogBox.AddDialogue($"{target.CurMonster.Nickname} is KO!");

            yield return new WaitForSeconds(dialogBox.ReturnDuration());

            CheckForBattleOver(target);
        }

        CheckNextTurn();
    }
    
    private IEnumerator RunAfterTurn(ActiveMonster playerUnit, ActiveMonster enemyUnit)
    {
        if (playerUnit.CurMonster.Affliction != e_Afflictions.NONE)
        {
            switch (playerUnit.CurMonster.Affliction)
            {
                case e_Afflictions.BURN:
                    int burnDmg = Mathf.FloorToInt(playerUnit.CurMonster.Stats.MaxHp * 0.05f);

                    playerUnit.CurMonster.Damage(burnDmg);
                    break;
                case e_Afflictions.POISON:
                    float percentValue = 0.05f * (1 + _playerAfflictedTurn);

                    int poisonDmg = Mathf.FloorToInt(playerUnit.CurMonster.Stats.MaxHp * percentValue);

                    playerUnit.CurMonster.Damage(poisonDmg);

                    if (playerUnit.CurMonster.Affliction == e_Afflictions.SLEEP)
                    {
                        playerUnit.CurMonster.SetAffliction(e_Afflictions.NONE);

                        Debug.Log(playerUnit.CurMonster.Nickname + " " + "is waking up");
                    }

                    _playerAfflictedTurn++;
                    break;
                default:
                    break;
            }
        }

        if (enemyUnit.CurMonster.Affliction != e_Afflictions.NONE)
        {
            switch (enemyUnit.CurMonster.Affliction)
            {
                case e_Afflictions.BURN:
                    int burnDmg = Mathf.FloorToInt(enemyUnit.CurMonster.Stats.MaxHp * 0.05f);

                    enemyUnit.CurMonster.Damage(burnDmg);
                    break;
                case e_Afflictions.POISON:
                    float percentValue = 0.05f * (1 + _enemyAfflictedTurn);

                    int poisonDmg = Mathf.FloorToInt(enemyUnit.CurMonster.Stats.MaxHp * percentValue);

                    enemyUnit.CurMonster.Damage(poisonDmg);

                    if (enemyUnit.CurMonster.Affliction == e_Afflictions.SLEEP)
                    {
                        enemyUnit.CurMonster.SetAffliction(e_Afflictions.NONE);

                        Debug.Log(enemyUnit.CurMonster.Nickname + " " + "is waking up");
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
        state = BattleState.Busy;

        if(_playerActionState == BattleAction.Afflicted)
        {
            switch (playerUnit.CurMonster.Affliction)
            {
                case e_Afflictions.FREEZE:
                    if (_playerAfflictedTurn <= 0)
                        playerUnit.CurMonster.SetAffliction(e_Afflictions.NONE);
                    else
                        _playerAfflictedTurn--;
                    break;
                case e_Afflictions.SLEEP:
                    if (_playerAfflictedTurn <= 0)
                        playerUnit.CurMonster.SetAffliction(e_Afflictions.NONE);
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
        state = BattleState.Busy;

        EnemyTech();

        if (_enemyActionState == BattleAction.Afflicted)
        {
            switch (enemyUnit.CurMonster.Affliction)
            {
                case e_Afflictions.FREEZE:
                    if (_enemyAfflictedTurn <= 0)
                        enemyUnit.CurMonster.SetAffliction(e_Afflictions.NONE);
                    else
                        _enemyAfflictedTurn--;
                    break;
                case e_Afflictions.SLEEP:
                    if (_enemyAfflictedTurn <= 0)
                        enemyUnit.CurMonster.SetAffliction(e_Afflictions.NONE);
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

        yield return new WaitForSecondsRealtime(1);

        unit.SetMonster(newMonster);

        CheckNextTurn();
    }

    private void CheckForEffects(obj_Techs tech, ActiveMonster unit)
    {
        e_Afflictions techAffliction = tech.Extra.Effect.affliction;
        e_Types techType = tech.Information.Type;

        Monsters target = unit.CurMonster;

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
        state = BattleState.BattleOver;

        //OnBattleOver(value);
    }

    private void GainXP(ActiveMonster unit)
    {
        playerUnit.CurMonster.AddExperience(unit.CurMonster.Base.GivenXp);
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
                float senderAtk = sender.Stats.Atk;

                if(sender.Affliction == e_Afflictions.BURN)
                {
                    senderAtk = senderAtk - (senderAtk * 0.2f);
                }

                return (float)senderAtk / (float)receiver.Stats.Def;
            case e_Styles.SPECIAL:
                float senderSpeAtk = sender.Stats.Atk;

                if (sender.Affliction == e_Afflictions.BURN)
                {
                    senderSpeAtk = senderSpeAtk - (senderSpeAtk * 0.2f);
                }
                return (float)senderSpeAtk / (float)receiver.Stats.SpeDef;
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