using UnityEngine;

public class ActionTurnState : State
{
//Reference to the BattleSystem
    public ActionTurnState(BattleSystem battleSystem) : base(battleSystem)
    {
    }


    private bool _pTurnEnded = false;
    private bool _eTurnEnded = false;

//First function to start at the beginning of the State
    public override void Start()
    {
        Zori pZori = BattleSystem.PlayerZori;
        Zori eZori = BattleSystem.EnemyZori;

        BattleSystem.SetEnemyTech(BattleSystem.EnemyTech());

        CalculatePriority(pZori, eZori);
    }

//Last step before the next State
    public override void Stop()
    {
        
    }

//Calculate First Zori to Attack
    private void CalculatePriority(Zori PZori, Zori EZori)
    {
        int pZoriSpeed = PZori.Stats.Speed;
        int eZoriSpeed = EZori.Stats.Speed;

        int pTechPriority = BattleSystem.PTech.Priority;
        int eTechPriority = BattleSystem.ETech.Priority;

        int rdm = Random.Range(0,1);

        if(pTechPriority > eTechPriority)
        {
            BattleSystem.SetState(new PlayerTurnState(BattleSystem));
            BattleSystem.OnState.Start();
            return;
        }
        else if(eTechPriority > pTechPriority)
        {
            BattleSystem.SetState(new EnemyTurnState(BattleSystem));
            BattleSystem.OnState.Start();
            return;
        }

        if(pZoriSpeed > eZoriSpeed)
        {
            BattleSystem.SetState(new PlayerTurnState(BattleSystem));
            BattleSystem.OnState.Start();
            return;
        }
        else if(eZoriSpeed > pZoriSpeed)
        {
            BattleSystem.SetState(new EnemyTurnState(BattleSystem));
            BattleSystem.OnState.Start();
            return;
        }

        if(rdm > 0)
        {
            BattleSystem.SetState(new PlayerTurnState(BattleSystem));
            BattleSystem.OnState.Start();
            return;
        }
        else
        {
            BattleSystem.SetState(new EnemyTurnState(BattleSystem));
            BattleSystem.OnState.Start();
            return;
        }
    }
}
