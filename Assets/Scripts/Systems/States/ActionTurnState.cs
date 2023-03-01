using System.Collections;
using UnityEngine;

public class ActionTurnState : BattleState
{
    public override IEnumerator Enter() 
    {
        EnemyTechSelection();

        LoadTech();

        LoadUI();

        HUD.Instance.ActivateBattleHUD(true);

        yield break;
    }

    private void LoadUI()
    {
        HUD.Instance.BattleHUD.Player.Init(battleManager.PlayerUnit.Monster);
        HUD.Instance.BattleHUD.Enemy.Init(battleManager.EnemyUnit.Monster);
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
