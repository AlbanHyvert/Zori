public class EndTurnState : State
{
//Reference to the BattleSystem
    public EndTurnState(BattleSystem battleSystem) : base(battleSystem)
    {
    }

//First function to start at the beginning of the State
    public override void Start()
    {
        
    }

//Only use during fight
    public override void Tick()
    {

    }

//Last step before the next State
    public override void Stop()
    {

    }
}
