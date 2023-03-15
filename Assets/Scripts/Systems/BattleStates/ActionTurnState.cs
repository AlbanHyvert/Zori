using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Monster.Enum;

public class ActionTurnState : BattleState
{
    private HUD HUD = null;

    public override IEnumerator Enter() 
    {
        HUD = battleManager.HUD;

        EnemyTechSelection();

        LoadTech();

        LoadItem();

        LoadSwitch();

        if(battleManager.PlayerUnit.Monster.Affliction != e_Afflictions.KO)
        {
            HUD.ActivateSwitch(false);
            HUD.ActivateBattleHUD(true);
        }  
        else
        {
            HUD.ActivateBattleHUD(false);
            HUD.ActivateSwitch(true);
        }

        yield break;
    }

    private void LoadSwitch()
    {
        //Clear every Monsters
        for(int i = 0; i < HUD.UISwitch.ActionButton.Length; i++)
        {
            HUD.UISwitch.ActionButton[i].Clear();
        }

        //Set the Monster
        for(int i = 0; i < battleManager.PlayerTeam.Team.Count; i++)
        {
            Monsters monster = battleManager.PlayerTeam.Team[i];

            if(monster == null) return;

            if(monster.Affliction == e_Afflictions.KO)
            {
                HUD.UISwitch.ActionButton[i].Clear();
            }
            else if(monster.Affliction != e_Afflictions.KO)
                HUD.UISwitch.ActionButton[i].SetMonster(monster);
        }
    }

    private void LoadTech()
    {
        Monsters unit = battleManager.PlayerUnit.Monster;
        obj_Techs tech = null;
        obj_Item item = battleManager.PlayerUnit.Monster.HoldItem;

        //Clear every Tech
        for(int i = 0; i < HUD.TechSelector.ActionBtn.Length; i++)
        {
            HUD.TechSelector.ActionBtn[i].Clear();
        }

        if(item != null)
        {
            if(item.Name.Equals(e_Names.ToughChains) || item.Name.Equals(e_Names.TwistedChains) || item.Name.Equals(e_Names.PolishedChains))
            {
                tech = unit.Techs[0];

                HUD.TechSelector.ActionBtn[0].SetTech(tech);

                return;
            }
        }

        //Set the tech
        for(int i = 0; i < unit.Techs.Length; i++)
        {
            tech = unit.Techs[i];

            if(tech == null) return;

            if(tech.Information.Stamina > unit.Stamina) return;

            HUD.TechSelector.ActionBtn[i].SetTech(tech);
        }
    }

    private void LoadItem()
    {
        //Clear every Item
        for(int i = 0; i < HUD.ItemSelector.ActionBtn.Length; i++)
        {
            HUD.ItemSelector.ActionBtn[i].Clear();
        }

        List<obj_Item> items = Player.Instance.Inventory.ItemList;
        obj_Item item = null;

        //Set the tech
        for(int i = 0; i < items.Count; i++)
        {
            item = items[i];

            if(item == null) return;

            HUD.ItemSelector.ActionBtn[i].SetItem(item);
        }
    }

    private void EnemyTechSelection()
    {
        int maxTech = battleManager.EnemyUnit.Monster.Techs.Length;
        int rdmTech = Random.Range(0, maxTech);

        obj_Techs tech = battleManager.EnemyUnit.Monster.Techs[rdmTech];
        obj_Item item = battleManager.EnemyUnit.Monster.HoldItem;

        if(tech == null)
        {
            EnemyTechSelection();
            return;
        }

        if(item != null)
        {
            if(item.Name.Equals(e_Names.ToughChains) || item.Name.Equals(e_Names.TwistedChains) || item.Name.Equals(e_Names.PolishedChains))
            {
                tech = battleManager.EnemyUnit.Monster.Techs[0];

                HUD.TechSelector.ActionBtn[0].SetTech(tech);
            }

            return;
        }

        battleManager.SetEActionType(BattleManager.ActionType.MOVE);

        battleManager.EnemyUnit.SetTech(tech);
    }

    public override void Exit()
    {
        battleManager.StopAllCoroutines();
    }
}
