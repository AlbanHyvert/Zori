public abstract class ControllerState
{
    protected ControllerStateMachine Controller = null;

    public void SetManager(ControllerStateMachine manager)
    => Controller = manager;

    public virtual void Enter() {}

    public virtual void Tick(float time) {}

    public virtual void Exit() {}
}