using UnityEngine;
using Monster.Enum;
public class EnemyTurnState : BattleState
{
    public EnemyTurnState(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    private Monsters _self = null;
    private Monsters _enemy = null;

    private obj_Techs _selfTech = null;

    private float _timeToStart = 0;

    public override void Init()
    {
        HUD.Instance.BattleHUD.SetActiveTechSelector(false);
        HUD.Instance.BattleHUD.SetActiveSelector(false);

        _self = BattleSystem.ActiveEnemy.CurMonster;
        _enemy = BattleSystem.ActivePlayer.CurMonster;

        _selfTech = BattleSystem.EnemyTech;

        HUD.Instance.DialBox.AddDialogue(_self.Nickname + " " + "use" + " " + _selfTech.Information.Name + " !");

        _timeToStart = GameManager.Instance.Time + HUD.Instance.DialBox.ReturnDuration();

        GameManager.Instance.OnUpdateBattle += DialTimeOut;
    }

    private void DialTimeOut(float time)
    {
        if(time < _timeToStart)
            return;

        _timeToStart = 0;

//-------------------Add Vfx and Sound time-------------------
       //_timeToStart = time + 2f;
        
        GameManager.Instance.OnUpdateBattle += VfxTimeOut;
        GameManager.Instance.OnUpdateBattle -= DialTimeOut;
    }

    private void VfxTimeOut(float time)
    {
        if(time < _timeToStart)
            return;

        _timeToStart = 0;

//-------------------Add Damage Fx and Sound time-------------------
        
        GameManager.Instance.OnUpdateBattle += DealDamage;
        GameManager.Instance.OnUpdateBattle -= VfxTimeOut;
    }

    private void DealDamage(float time)
    {
        if(time < _timeToStart)
            return;

        _timeToStart = 0;

//-------------------Close Battle-------------------
        _self.Cost(_selfTech.Information.Stamina);

        GameManager.Instance.OnUpdateBattle -= DealDamage;
        End();
    }

    public override void End()
    {
        BattleSystem.EnemyTurnEnded = true;

        //Check if Enemy Turn as ended
        if(BattleSystem.AllyTurnEnded == true)
        {
            BattleSystem.SetState(new EndTurnState(BattleSystem));
            return;
        }
        else
        {
            BattleSystem.SetState(new PlayerTurnState(BattleSystem));
        }
    }
}
