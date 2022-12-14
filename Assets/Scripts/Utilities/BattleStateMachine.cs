using UnityEngine;

public abstract class BattleStateMachine : MonoBehaviour
{
// Current State
    protected State State = null;

    public State OnState
        => State;

//Set the new State of the Battle System
    public State SetState(State newState)
    {
        State = newState;

        if(State == null)
            return null;

        return State;
    }
}
