using System.Collections;

public abstract class State
{
    protected BattleSystem BattleSystem = null;

    protected bool _turnEnded = false;

    public bool TurnEnded
        => _turnEnded;
        

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
