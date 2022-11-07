using System.Collections;

public abstract class State
{
    protected BattleSystem BattleSystem = null;

    public State(BattleSystem battleSystem)
        => BattleSystem = battleSystem;

    public virtual void Start()
    {
        
    }

    public virtual void Tick()
    {

    }

    public virtual void Stop()
    {

    }
}
