public abstract class BattleState
{
    protected BattleSystem BattleSystem = null;

    //Set Battle System
    public BattleState(BattleSystem battleSystem)
        => BattleSystem = battleSystem;

    //Init Variables and system
    public virtual void Init(){}

    //Set the turn and time for each action
    public virtual void Tick(float time){}

    //End of the state Set the new State
    public virtual void End(){}
}
