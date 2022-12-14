using System;
using UnityEngine;

public class StartBattleState : BattleState
{
    public StartBattleState(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    private Monsters _ally = null;
    private Monsters _enemy = null;

    private float _timeToStart = 0;

    public override void Init()
    {
        HUD.Instance.ActivateDialbox(true);
        HUD.Instance.ActivateBattleHUD(false);

        HUD.Instance.BattleHUD.Player.Init(_ally);
        HUD.Instance.BattleHUD.Enemy.Init(_enemy);

        HUD.Instance.DialBox.AddDialogue("A wild " + _enemy.Nickname + " as appeared !!");

        _timeToStart = GameManager.Instance.Time + HUD.Instance.DialBox.ReturnDuration();

        GameManager.Instance.OnUpdateBattle += InitEnemy;
    }

    private void InitEnemy(float time)
    {
        if(time < _timeToStart)
            return;
        
        HUD.Instance.BattleHUD.SetActiveOtherUi(true);

        //_enemy.gameObject.SetActive(true);

        HUD.Instance.DialBox.AddDialogue("GO défoncé sa gueule " + _ally.Nickname + " à ce bâtard de " + _enemy.Nickname + " !!");

        _timeToStart = 0;
        _timeToStart = time + HUD.Instance.DialBox.ReturnDuration();

        GameManager.Instance.OnUpdateBattle -= InitEnemy;
        GameManager.Instance.OnUpdateBattle += InitAlly;
    }

    private void InitAlly(float time)
    {
        if(time < _timeToStart)
            return;

        HUD.Instance.BattleHUD.SetActivePlayerUi(true);

        _timeToStart = 0;

        _timeToStart = time + 2;

        //_ally.gameObject.SetActive(true);

        GameManager.Instance.OnUpdateBattle -= InitAlly;
        GameManager.Instance.OnUpdateBattle += ApplyHUD;
    }

    private void ApplyHUD(float time)
    {
        if(time < _timeToStart)
            return;

        _timeToStart = 0;
        
        End();

        GameManager.Instance.OnUpdateBattle -= ApplyHUD;
    }

    public override void End()
    {
        BattleSystem.SetState(new ChooseTurnState(BattleSystem));
    }
}
