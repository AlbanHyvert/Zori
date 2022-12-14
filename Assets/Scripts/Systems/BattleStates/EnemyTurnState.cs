using UnityEngine;

public class EnemyTurnState : State
{
public EnemyTurnState(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    public override void Start()
    {
        Zori pZori = BattleSystem.PlayerZori;
        Zori eZori = BattleSystem.EnemyZori;
        
        TechBase techBase = BattleSystem.ETech;

        AttackTurn(techBase, eZori, pZori);
    }
//Last step before the next State
    public override void Stop()
    {
        if(BattleSystem.OldState != null)
        {
            BattleSystem.SetEnemyTech(null);
            BattleSystem.SetPlayerTech(null);
            BattleSystem.SetOldState(null);
            BattleSystem.SetState(new ActionTurnState(BattleSystem));
            return;
        }
        else
        {
            BattleSystem.SetOldState(this);
            BattleSystem.SetState(new PlayerTurnState(BattleSystem));
            BattleSystem.OnState.Start();
        }
    }

    private void AttackTurn(TechBase tech, Zori sender, Zori receiver)
    {
        //Update Information Text
        BattleSystem.EnemyHUD.UpdateDialBox(sender.Nickname.ToString() + " use " + tech.Name.ToString());
        
        //Start Animation

        //Launch FX / Sound
        
        //DealDamage
        receiver.onDamaged.Invoke(BattleSystem.CalculateDamage.DealDamage(tech, sender, receiver));

        //Update Information Text

        //Update Secondary Effects / Sound

        //Update Information Text

        //Update Afflictions / Sound

        //End Turn
        _turnEnded = true;

        Stop();
        return;
    }
}