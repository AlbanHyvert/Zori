using UnityEngine;

public class ChooseTurnState : BattleState
{
    public ChooseTurnState(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    private Monsters _ally = null;
    private Monsters _enemy = null;

    public override void Init()
    {
        HUD.Instance.DialBox.AddDialogue("What will you do ?!");
        HUD.Instance.BattleHUD.SetActiveSelector(true);

        _ally = BattleSystem.ActivePlayer.CurMonster;
        _enemy = BattleSystem.ActiveEnemy.CurMonster;

        BattleSystem.OnActionSelected += OnPlayerReady;

        UpdateAllyTech();
        UpdateAllySwitch();

        CheckMonsterWeakness();
    }

    private void OnPlayerReady()
    {
        End();
    }

    public override void End()
    {
        BattleSystem.OnActionSelected -= OnPlayerReady;
        
        int allyPriority = BattleSystem.AllyActionPrio;
        int enemyPriority = BattleSystem.EnemyActionPrio;

//---------------------Check Priority---------------------
        if(allyPriority > enemyPriority)
        {
            BattleSystem.SetState(new PlayerTurnState(BattleSystem));
            return;
        }
        if(enemyPriority > allyPriority)
        {
            BattleSystem.SetState(new EnemyTurnState(BattleSystem));
            return;
        }
//---------------------Check Speed---------------------
        if(_ally.Stats.Speed >= _enemy.Stats.Speed)
        {
            BattleSystem.SetState(new PlayerTurnState(BattleSystem));
            return;
        }
        else
        {
            BattleSystem.SetState(new EnemyTurnState(BattleSystem));
            return;
        }
    }

    private void UpdateAllyTech()
    {
        for (int i = 0; i < HUD.Instance.Selector.ActionBtn.Length; i++)
        {
            HUD.Instance.Selector.ActionBtn[i].Clear();
        }

        int buttonId = 0;

        for (int i = 0; i < _ally.Techs.Length; i++)
        {
            obj_Techs tech = _ally.Techs[i];
            obj_Techs btnTech = null;

            if(tech == null)
            return;

            if(CheckStamina(tech) == true)
            {
                btnTech = HUD.Instance.Selector.ActionBtn[buttonId].SetTech(_ally.Techs[i]);

                buttonId++;
            }
        }
    }

    public void UpdateAllySwitch()
    {
        for (int i = 0; i < HUD.Instance.UISwitch.ActionButton.Length; i++)
        {
            HUD.Instance.UISwitch.ActionButton[i].Clear();
        }

        int buttonId = 0;

        for (int i = 0; i < Player.Instance.Inventory.TeamHolder.Team.Count; i++)
        {
            Monsters monster = Player.Instance.Inventory.TeamHolder.Team[i];

            if(monster == null)
            {
                return;
            }

            HUD.Instance.UISwitch.ActionButton[buttonId].SetMonster(monster);

            buttonId++;
        }
    }

    private bool CheckStamina(obj_Techs tech)
    {
        if(tech.Information.Stamina > _ally.Stamina)
        {
            return false;
        }

        return true;
    }

    private void SetEnemyTech(int techId)
    {
        obj_Techs enemyTech = _enemy.Techs[techId];

        BattleSystem.SetEnemyTech(enemyTech);
    }

    private void CheckMonsterWeakness()
    {
        int maxLength = 0;

        for (int j = 0; j < _enemy.Techs.Length; j++)
        {
            if(_enemy.Techs[j] == null)
                return;
            else
                maxLength++;
        }

        int i = Random.Range(0, maxLength - 1);

        SetEnemyTech(i);
    }
}