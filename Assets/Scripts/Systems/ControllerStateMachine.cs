using UnityEngine;

public class ControllerStateMachine : MonoBehaviour
{
    protected ControllerState State = null;

    public MonsterController mController = null;
    public PlayerController pController = null;

    public void SetState(ControllerState state) 
    {
        if (State != null)
        {
            State.Exit();
        }

        State = state;

        if(State == null) return;

        State.SetManager(this);

        State.Enter();
    }

}
