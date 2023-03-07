using System.Collections;
using UnityEngine;
using Monster.Enum;

public class ActionTurnState : BattleState
{
    public override IEnumerator Enter() 
    {
        EnemyTechSelection();

        LoadTech();

        LoadSwitch();
        
        LoadUI();

        if(battleManager.PlayerUnit.Monster.Affliction != e_Afflictions.KO)
        {
            HUD.Instance.ActivateSwitch(false);
            HUD.Instance.ActivateBattleHUD(true);
        }  
        else
        {
            HUD.Instance.ActivateBattleHUD(false);
            HUD.Instance.ActivateSwitch(true);
        }

        yield break;
    }

    private void LoadUI()
    {
        HUD.Instance.BattleHUD.Player.Init(battleManager.PlayerUnit.Monster);
        HUD.Instance.BattleHUD.Enemy.Init(battleManager.EnemyUnit.Monster);
    }

    private void LoadSwitch()
    {
        //Clear every Monsters
        for(int i = 0; i < HUD.Instance.UISwitch.ActionButton.Length; i++)
        {
            HUD.Instance.UISwitch.ActionButton[i].Clear();
        }

        //Set the Monster
        for(int i = 0; i < battleManager.PlayerTeam.Team.Count; i++)
        {
            Monsters monster = battleManager.PlayerTeam.Team[i];

            if(monster == null) return;

            if(monster.Affliction == e_Afflictions.KO) return;

            HUD.Instance.UISwitch.ActionButton[i].SetMonster(monster);
        }
    }

    private void LoadTech()
    {
        //Clear every Tech
        for(int i = 0; i < HUD.Instance.Selector.ActionBtn.Length; i++)
        {
            HUD.Instance.Selector.ActionBtn[i].Clear();
        }

        Monsters unit = battleManager.PlayerUnit.Monster;
        obj_Techs tech = null;

        //Set the tech
        for(int i = 0; i < unit.Techs.Length; i++)
        {
            tech = unit.Techs[i];

            if(tech == null) return;

            if(tech.Information.Stamina > unit.Stamina) return;

            HUD.Instance.Selector.ActionBtn[i].SetTech(tech);
        }
    }

    private void EnemyTechSelection()
    {
        int maxTech = battleManager.EnemyUnit.Monster.Techs.Length;

        int rdmTech = Random.Range(0, maxTech);

        obj_Techs tech = battleManager.EnemyUnit.Monster.Techs[rdmTech];

        if(tech == null)
        {
            EnemyTechSelection();
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
