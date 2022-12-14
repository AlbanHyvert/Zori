using System.Diagnostics;

public class EndTurnState : BattleState
{
    public EndTurnState(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    private Monsters _ally = null;
    private Monsters _enemy = null;
    private Player _player = null;
    private float _timeToStart = 0;

    public override void Init()
    {
        _ally = BattleSystem.ActivePlayer.CurMonster;
        _enemy = BattleSystem.ActiveEnemy.CurMonster;
        _player = Player.Instance;

        HUD.Instance.DialBox.AddDialogue("Turn as ended !");

        GameManager.Instance.OnUpdateBattle += Tick;
    }

    public override void Tick(float time)
    {
        if(time < _timeToStart)
            return;

        CheckMonsterHp(BattleSystem.ActivePlayer);
        CheckMonsterHp(BattleSystem.ActiveEnemy);

        End();

        GameManager.Instance.OnUpdateBattle -= Tick;
    }

    private void CheckMonsterHp(ActiveMonster activeMonster)
    {
       switch (activeMonster.IsPlayer)
       {
        case true:
            if(_ally.Hp < 0)
            {
                Monsters newMonster = _player.Inventory.TeamHolder.GetHealthyZori();

                if(newMonster == null)
                {
                    //Lost

                    GameManager.Instance.OnUpdateBattle -= Tick;
                    return;
                }

                //Continue Battle
                HUD.Instance.ActivateSwitch(true);
            }
            break;

        case false:
            if(_enemy.Hp < 0)
            {
                /*Monsters newMonster = _npc.TeamHolder.GetHealthyZori();

                if(newMonster == null)
                {
                    GameManager.Instance.OnUpdateBattle -= Tick;
                    return;
                }

                BattleSystem.SetNewMonsters(activeMonster, newMonster);*/
            }
            break;
       }
    }

    public override void End()
    {
        BattleSystem.AllyTurnEnded = false;
        BattleSystem.EnemyTurnEnded = false;

        BattleSystem.SetState(new ChooseTurnState(BattleSystem));
    }
}