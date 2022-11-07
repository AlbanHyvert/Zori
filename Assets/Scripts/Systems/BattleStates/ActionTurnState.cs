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
        CalculateDamage calculateDamage = BattleSystem.CalculateDamage;
        Zori pZori = BattleSystem.PlayerZori;
        Zori eZori = BattleSystem.EnemyZori;

        BattleSystem.SetEnemyTech(BattleSystem.EnemyTech());

        CalculatePriority(BattleSystem.PlayerZori, BattleSystem.EnemyZori);
    }

//Only use during fight
    public override void Tick()
    {

    }

//Last step before the next State
    public override void Stop()
    {
        BattleSystem.StopAllCoroutines();
        BattleSystem.SetState(new ActionTurnState(BattleSystem));
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
            PlayerAtkTurn(BattleSystem.PTech, PZori, EZori);
            return;
        }
        else if(eTechPriority > pTechPriority)
        {
            EnemyAtkTurn(BattleSystem.ETech, EZori, PZori);
            return;
        }

        if(pZoriSpeed > eZoriSpeed)
        {
            PlayerAtkTurn(BattleSystem.PTech, PZori, EZori);
            return;
        }
        else if(eZoriSpeed > pZoriSpeed)
        {
            EnemyAtkTurn(BattleSystem.ETech, EZori, PZori);
            return;
        }

        if(rdm > 0)
        {
            PlayerAtkTurn(BattleSystem.PTech, PZori, EZori);
            return;
        }
        else
        {
            EnemyAtkTurn(BattleSystem.ETech, EZori, PZori);
            return;
        }
    }

    private void PlayerAtkTurn(TechBase tech, Zori PZori, Zori EZori)
    {
        _pTurnEnded = true;

        Debug.Log("Player Zori use: " + tech.Name);

        EZori.onDamaged.Invoke(BattleSystem.CalculateDamage.DealDamage(tech, PZori, EZori));

        if(_eTurnEnded == true)
        {
            Stop();
            return;
        }

        EnemyAtkTurn(BattleSystem.ETech, EZori, PZori);
        return;
    }
    private void EnemyAtkTurn(TechBase tech, Zori EZori, Zori PZori)
    {
        _eTurnEnded = true;

        Debug.Log("Enemy Zori use: " + tech.Name);

        PZori.onDamaged.Invoke(BattleSystem.CalculateDamage.DealDamage(tech, EZori, PZori));

        if(_pTurnEnded == true)
        {
            Stop();
            return;
        }

        PlayerAtkTurn(BattleSystem.PTech, PZori, EZori);
        return;
    }
}
