using UnityEngine;

public abstract class BattleStateMachine : MonoBehaviour
{
    //Current State
    protected BattleState State = null;

    //Set new Battle State
    public BattleState SetState(BattleState newState)
    {
        State = newState;

        if(State == null)
            return State = newState;
        
        State.Init();

        return State;
    }
}
