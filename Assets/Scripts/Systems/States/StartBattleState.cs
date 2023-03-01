using UnityEngine;
using System.Collections;
public class StartBattleState : BattleState
{
    public override IEnumerator Enter() 
    {
        Debug.Log("StartBattle.Enter");

        while(DialogueManager.Instance.IsTyping) yield return null;

        //Set player Monster
        battleManager.PlayerUnit.SetMonster(Player.Instance.TeamHolder.GetHealthyZori(), true);

        DialogueManager.Instance.StartDialogue("Player choose" + " " + battleManager.PlayerUnit.Monster.Nickname);
        
        while(DialogueManager.Instance.IsTyping) yield return null;
        
        //Set enemy Monster
        if(battleManager.EnemyTeam == null)
        {
            battleManager.EnemyUnit.SetMonster(Player.Instance.Encounter.WildMonster, true);
            DialogueManager.Instance.StartDialogue("A wild" + " " + battleManager.EnemyUnit.Monster.Nickname + " " + "has appeared !");
        }
        else
        {
            battleManager.EnemyUnit.SetMonster(battleManager.EnemyTeam.GetHealthyZori(), true);
            DialogueManager.Instance.StartDialogue("Enemy choose" + " " + battleManager.PlayerUnit.Monster.Nickname);
        }

        while(DialogueManager.Instance.IsTyping) yield return null;

        // Go to next state
        battleManager.SetState(new ActionTurnState());
    }

    public override void Exit()
    {
        battleManager.StopAllCoroutines();
    }
}
