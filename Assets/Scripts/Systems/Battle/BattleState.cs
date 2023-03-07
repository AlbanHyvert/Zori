using System.Collections;

public abstract class BattleState
{
    protected BattleManager battleManager = null;

    public void SetManager(BattleManager manager)
    => battleManager = manager;

    public virtual IEnumerator Enter() {yield break;}
    public virtual void Exit() {}
}