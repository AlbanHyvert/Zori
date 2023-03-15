using System.Collections;
using UnityEngine;
using Monster.Enum;

public class ResolveTurnState : BattleState
{
    private bool _playerTurnEnded = false;
    private bool _enemyTurnEnded = false;
    private HUD HUD = null;

    public override IEnumerator Enter()
    {
        HUD = battleManager.HUD;

        _playerTurnEnded = false;
        _enemyTurnEnded = false;

        HUD.BattleHUD.SetActiveSelector(false);
        HUD.BattleHUD.SetActiveTechSelector(false);
        HUD.BattleHUD.SetActivePlayerUi(true);
        HUD.BattleHUD.SetActiveEnemyUi(true);

        ChoosePriority();

        yield break;
    }

#region TURNS
    private void ChoosePriority()
    {
        Debug.Log("Priorities");

        if (battleManager.PlayerPriority > battleManager.EnemyPriority)
        {
            PlayerTurn();

            return;
        }
        else if(battleManager.PlayerPriority < battleManager.EnemyPriority)
        {
            EnemyTurn();

            return;
        }

        int playerSpeedBoost = battleManager.PlayerUnit.Monster.StatsBoost.Speed;
        int enemySpeedBoost = battleManager.EnemyUnit.Monster.StatsBoost.Speed;

        int playerSpeed = Mathf.FloorToInt(battleManager.PlayerUnit.Monster.Stats.Speed * battleManager.PlayerUnit.Monster.StatsBoost.ReturnModificator(playerSpeedBoost));
        int enemySpeed = Mathf.FloorToInt(battleManager.EnemyUnit.Monster.Stats.Speed * battleManager.EnemyUnit.Monster.StatsBoost.ReturnModificator(enemySpeedBoost));

        obj_Item pItem = battleManager.PlayerUnit.Monster.HoldItem;
        obj_Item eItem = battleManager.EnemyUnit.Monster.HoldItem;

        Debug.Log("Check For Items");

        if(pItem != null)
        {
            if(pItem.Name.Equals(e_Names.PolishedChains))
            {
                playerSpeed = Mathf.FloorToInt(playerSpeed * pItem.Value);
            }
        }
        
        if(eItem != null)
        {
            if(eItem.Name.Equals(e_Names.PolishedChains))
            {
                enemySpeed = Mathf.FloorToInt(enemySpeed * eItem.Value);
            }
        }

        if(battleManager.PlayerUnit.Monster.Affliction == e_Afflictions.PARALYSIS)
        {
            playerSpeed = playerSpeed / 2;
        }

        if (battleManager.EnemyUnit.Monster.Affliction == e_Afflictions.PARALYSIS)
        {
            enemySpeed = enemySpeed / 2;
        }

        Debug.Log("Check For turns");

        if (battleManager.PlayerUnit.Monster.Stats.Speed >= battleManager.EnemyUnit.Monster.Stats.Speed)
        {
            PlayerTurn();
            return;
        }
        else if (battleManager.PlayerUnit.Monster.Stats.Speed < battleManager.EnemyUnit.Monster.Stats.Speed)
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

    private void CheckNextTurn()
    {
        if (_playerTurnEnded == true && _enemyTurnEnded == true)
        {
            _playerTurnEnded = false;
            _enemyTurnEnded = false;

            battleManager.SetState(new AfterTurnState());

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

    private void PlayerTurn()
    {
        Debug.Log("Player Turns");

        _playerTurnEnded = true;

        if(battleManager.PlayerAction == BattleManager.ActionType.AFFLICTED)
        {
            CheckForAffliction(battleManager.PlayerUnit);

            CheckNextTurn();
            
            return;
        }

        switch (battleManager.PlayerAction)
        {
            case BattleManager.ActionType.NONE:
                break;
            case BattleManager.ActionType.MOVE:
            battleManager.StartCoroutine(RunTech(battleManager.PlayerUnit, battleManager.EnemyUnit, battleManager.PlayerUnit.TechUsed));
                break;
            case BattleManager.ActionType.ITEM:
            battleManager.StartCoroutine(UseItem(battleManager.PlayerUnit, battleManager.PlayerUnit.Item));
                break;
            case BattleManager.ActionType.SWITCH:
            battleManager.StartCoroutine(SwitchZori(battleManager.PlayerUnit, battleManager.PlayerUnit.NewMonster));
                break;
            case BattleManager.ActionType.RUN:
                break;
            default:
            break;
        }
    }

    private void EnemyTurn()
    {
        Debug.Log("Player Turns");

        _enemyTurnEnded = true;

        if (battleManager.EnemyAction == BattleManager.ActionType.AFFLICTED)
        {
            CheckForAffliction(battleManager.EnemyUnit);

            CheckNextTurn();

            return;
        }

        switch (battleManager.EnemyAction)
        {
            case BattleManager.ActionType.NONE:
                break;
            case BattleManager.ActionType.MOVE:
            battleManager.StartCoroutine(RunTech(battleManager.EnemyUnit, battleManager.PlayerUnit, battleManager.EnemyUnit.TechUsed));
                break;
            case BattleManager.ActionType.ITEM:
                break;
            case BattleManager.ActionType.SWITCH:
            battleManager.StartCoroutine(SwitchZori(battleManager.EnemyUnit, battleManager.EnemyUnit.NewMonster));
                break;
            case BattleManager.ActionType.RUN:
                break;
            default:
            break;
        }
    }

    private void CheckForBattleOver(ActiveMonster faintedUnit)
    {
        Debug.Log("Monster Fainted: " + faintedUnit.Monster.Nickname);

        if (faintedUnit.IsPlayer)
        {
            Monsters nextZori = battleManager.PlayerTeam.GetHealthyZori();

            if (nextZori != null)
            {
                battleManager.SetState(new ActionTurnState());
            }
            else
            {
                battleManager.SetState(new LostState());
            }
        }

        /*if (faintedUnit.IsNpc)
        {
            DialogueManager.Instance.StartDialogue(_playerUnit.Monster.Nickname + " " + "has gained" + " " + _enemyUnit.Monster.Base.GivenXp.ToString() + " !");

            while(DialogueManager.Instance.IsTyping) yield return null;

            battleManager.GainXP(faintedUnit);

            Monsters nextZori = NpcTeam.GetHealthyZori();

            if(nextZori != null)
            {
                SwitchZori(battleManager.EnemyUnit, nextZori);

                _enemyTurnEnded = true;

                return false;
            }
            else
            {
                battleManager.Victory();
                return true;
            }
        }*/
        if (!faintedUnit.IsPlayer)
        {
            battleManager.SetState(new VictoryState());
        }
    }
#endregion TURNS

#region  RUN MOVE
    //Move
    IEnumerator RunTech(ActiveMonster source, ActiveMonster target, obj_Techs tech)
    {
        CheckForStatsBoost(source, target, tech);

        //Update Source Stamina
        source.Monster.Cost(tech.Information.Stamina);

        //yield return playerHUD.UpdateStam();

        DialogueManager.Instance.StartDialogue($"{source.Monster.Nickname} used {tech.Information.Name}!");

        Debug.Log("Monster: " + source.Monster.Nickname + "is Player: " + source.IsPlayer + " Tech: " + tech.Information.Name);

        //Temp variable for the damage
        int dmg = DealDamage(source, target);

         while(DialogueManager.Instance.IsTyping) yield return null;

        //Deal damage to the Target
        target.Monster.Damage(dmg);

        while(target.UI.HasUpdated == false) yield return null;

        if(dmg > 0 && target.Monster.Affliction == e_Afflictions.SLEEP)
        {
            target.Monster.SetAffliction(e_Afflictions.NONE);

            Debug.Log(target.Monster.Nickname + " " + "is waking up");
        }

        CheckForEffects(tech, target);

        //Check KO Status
        if (target.Monster.Affliction.Equals(e_Afflictions.KO))
        {
            DialogueManager.Instance.StartDialogue($"{target.Monster.Nickname} is KO!");
            
            CheckForBattleOver(target);

             while(DialogueManager.Instance.IsTyping) yield return null;

            yield return null;
        }

        CheckNextTurn();

        yield return null;
    }

    //Switch
    private IEnumerator SwitchZori(ActiveMonster unit, Monsters newMonster)
    {
        DialogueManager.Instance.StartDialogue("Come back" + " " + unit.Monster.Nickname + " !");

        while(DialogueManager.Instance.IsTyping) yield return null;

        unit.DestroyModel();

        unit.Monster.StatsBoost.Reset();
        unit.Monster.Regeneration(unit.Monster.Stats.MaxStamina);

        DialogueManager.Instance.StartDialogue("Its your turn NOW" + " " + newMonster.Nickname + " !");

        while(DialogueManager.Instance.IsTyping) yield return null;

        unit.SetMonster(newMonster, true);

        CheckNextTurn();
    }

    //Item
    private IEnumerator UseItem(ActiveMonster unit, obj_Item item)
    {
        Debug.Log("Use Item");

        switch(item.Category)
        {
            case e_Category.Medicine:
                DialogueManager.Instance.StartDialogue("Player is using " + item.ReturnName() + " on " + unit.Monster.Nickname);

                while(DialogueManager.Instance.IsTyping) yield return null;

                item.Use(unit.Monster, Player.Instance.Inventory.ItemList);

                while(unit.UI.HasUpdated == false) yield return null;
                
                break;

            case e_Category.Battle_Item:
                DialogueManager.Instance.StartDialogue("Player is giving the " + item.ReturnName() + " to " + unit.Monster.Nickname);

                while(DialogueManager.Instance.IsTyping) yield return null;

                unit.Monster.GiveItem(item);

                while(unit.UI.HasUpdated == false) yield return null;
                
                break;
        }

        CheckNextTurn();
    }

#endregion RUN MOVE

#region  AFFLICTIONS
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

    private void CheckForAffliction(ActiveMonster unit)
    {
        switch (unit.Monster.Affliction)
            {
                case e_Afflictions.FREEZE:
                    if (unit.AfflictedTurn <= 0)
                        unit.Monster.SetAffliction(e_Afflictions.NONE);
                    else
                        unit.AfflictedTurn--;
                    break;
                case e_Afflictions.SLEEP:
                    if (unit.AfflictedTurn <= 0)
                        unit.Monster.SetAffliction(e_Afflictions.NONE);
                    else
                        unit.AfflictedTurn--;
                    break;
                default:
                    break;
            }
    }

    //Stats Boost
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

#endregion AFFLICTIONS

#region  DEAL DAMAGE
    private int DealDamage(ActiveMonster activeSender, ActiveMonster activeReceiver)
    {
        if(activeSender.TechUsed.Extra.Style == e_Styles.TACTIC)
        {
            return 0;
        }

        float typeMult = TypeChart.GetEffectiveness(activeSender.TechUsed.Information.Type, activeReceiver.Monster.Base.Types[0]) *
                                TypeChart.GetEffectiveness(activeSender.TechUsed.Information.Type, activeReceiver.Monster.Base.Types[1]);

        float modifier = CheckStab(activeSender.TechUsed, activeSender.Monster.Base.Types) * typeMult;

        float checkStatsDiff = CheckStatsDiff(activeSender.TechUsed, activeSender.Monster, activeReceiver.Monster);

        float actualPower = CalculatePower(activeSender);

        int dmg = Mathf.FloorToInt(((((((2 * activeSender.Monster.Level) / 5) + 2) * actualPower * checkStatsDiff) / 50 + 2) * modifier));

        Debug.Log("Damage: " + dmg);

        //CheckForItem
        if(activeReceiver.Monster.HoldItem != null)
        {
            obj_Item item = activeReceiver.Monster.HoldItem;
            obj_Techs tech = activeSender.TechUsed;

            switch(item.Name)
            {
                case e_Names.NormalPlate:
                if(tech.Information.Type.Equals(e_Types.NEUTRAL))
                {
                    float curDmg = dmg / item.Value;

                    Debug.Log("CurDamage: " + curDmg);

                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.FirePlate:
                if(tech.Information.Type.Equals(e_Types.PYRO))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.WaterPlate:
                if(tech.Information.Type.Equals(e_Types.HYDRO))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.NaturePlate:
                if(tech.Information.Type.Equals(e_Types.PHYTO))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.ElectricPlate:
                if(tech.Information.Type.Equals(e_Types.ELECTRO))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.IcePlate:
                if(tech.Information.Type.Equals(e_Types.CRYO))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.ToxicPlate:
                if(tech.Information.Type.Equals(e_Types.VENO))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.EarthPlate:
                if(tech.Information.Type.Equals(e_Types.GEO))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.WindPlate:
                if(tech.Information.Type.Equals(e_Types.AERO))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.HivePlate:
                if(tech.Information.Type.Equals(e_Types.INSECTO))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.IronPlate:
                if(tech.Information.Type.Equals(e_Types.METAL))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.BattlePlate:
                if(tech.Information.Type.Equals(e_Types.MARTIAL))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.PsyPlate:
                if(tech.Information.Type.Equals(e_Types.MENTAL))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.GhostPlate:
                if(tech.Information.Type.Equals(e_Types.SPECTRAL))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.DarkPlate:
                if(tech.Information.Type.Equals(e_Types.UMBRA))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            case e_Names.LightPlate:
                if(tech.Information.Type.Equals(e_Types.LUMA))
                {
                    dmg = Mathf.FloorToInt(dmg / item.Value);
                }
                break;
            }

            Debug.Log("Damage with item: " + item.ReturnName()+ " " + dmg);
        }

        return dmg;
    }

    private float CalculatePower(ActiveMonster sender)
    {
        obj_Item item = sender.Monster.HoldItem;

        if(item == null) return sender.TechUsed.Information.ReturnPower();

        obj_Techs tech = sender.TechUsed;

        switch(sender.Monster.HoldItem.Name)
        {
            case e_Names.NormalCharm:
                if(tech.Information.Type.Equals(e_Types.NEUTRAL))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.FireCharm:
                if(tech.Information.Type.Equals(e_Types.PYRO))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.WaterCharm:
                if(tech.Information.Type.Equals(e_Types.HYDRO))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.NatureCharm:
                if(tech.Information.Type.Equals(e_Types.PHYTO))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.ElectricCharm:
                if(tech.Information.Type.Equals(e_Types.ELECTRO))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.IceCharm:
                if(tech.Information.Type.Equals(e_Types.CRYO))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.ToxicCharm:
                if(tech.Information.Type.Equals(e_Types.VENO))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.EarthCharm:
                if(tech.Information.Type.Equals(e_Types.GEO))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.WindCharm:
                if(tech.Information.Type.Equals(e_Types.AERO))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.HiveCharm:
                if(tech.Information.Type.Equals(e_Types.INSECTO))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.IronCharm:
                if(tech.Information.Type.Equals(e_Types.METAL))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.BattleCharm:
                if(tech.Information.Type.Equals(e_Types.MARTIAL))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.PsyCharm:
                if(tech.Information.Type.Equals(e_Types.MENTAL))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.GhostCharm:
                if(tech.Information.Type.Equals(e_Types.SPECTRAL))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.DarkCharm:
                if(tech.Information.Type.Equals(e_Types.UMBRA))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
            case e_Names.LightCharm:
                if(tech.Information.Type.Equals(e_Types.LUMA))
                {
                    return tech.Information.ReturnPower() * item.Value;
                }
                break;
        }
    
        return tech.Information.ReturnPower();
    }

    //---------------Check extra Damage---------------
    //Check the difference between stats and return the value
    private float CheckStatsDiff(obj_Techs tech, Monsters sender, Monsters receiver)
    {
        switch (tech.Extra.Style)
        {
            case e_Styles.PHYSIC:
                float senderAtk = sender.Stats.Atk;
                float receiverDef = receiver.Stats.Def;

                if(sender.HoldItem != null)
                {
                    switch(sender.HoldItem.Name)
                    {
                        case e_Names.ToughChains:
                            senderAtk = senderAtk * sender.HoldItem.Value;
                            break;
                    }
                }

                if(sender.Affliction == e_Afflictions.BURN)
                {
                    senderAtk = senderAtk - (senderAtk * 0.2f);
                }

                int sdAtk = sender.StatsBoost.Atk;
                int rcDef = receiver.StatsBoost.Def;

                return (float)(senderAtk * sender.StatsBoost.ReturnModificator(sdAtk)) / 
                    (float)(receiverDef * receiver.StatsBoost.ReturnModificator(rcDef));

            case e_Styles.SPECIAL:
                float senderSpeAtk = sender.Stats.Atk;
                float receiverSpeDef = receiver.Stats.SpeDef;

                if(sender.HoldItem != null)
                {
                    switch(sender.HoldItem.Name)
                    {
                        case e_Names.TwistedChains:
                            senderSpeAtk = senderSpeAtk * sender.HoldItem.Value;
                            break;
                    }
                }

                if (sender.Affliction == e_Afflictions.BURN)
                {
                    senderSpeAtk = senderSpeAtk - (senderSpeAtk * 0.2f);
                }

                int sdSpeAtk = sender.StatsBoost.SpAtk;
                int rcSpeDef = receiver.StatsBoost.SpDef;

                return (float)(senderSpeAtk * sender.StatsBoost.ReturnModificator(sdSpeAtk)) /
                    (float)(receiverSpeDef * receiver.StatsBoost.ReturnModificator(rcSpeDef));
        }

        return 1;
    }

    //Check bonus between the tech and the pokemon type
    private float CheckStab(obj_Techs Tech, e_Types[] zoriTypes)
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
#endregion DEAL DAMAGE

    public override void Exit()
    {
       battleManager.StopAllCoroutines();
    }
}