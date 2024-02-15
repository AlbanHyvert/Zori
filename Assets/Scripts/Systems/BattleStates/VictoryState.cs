using System.Collections;

public class VictoryState : BattleState
{
    public override IEnumerator Enter()
    {
        battleManager.HUD.ActivateBattleHUD(false);

        DialogueManager.Instance.StartDialogue("You have won !");

        while(DialogueManager.Instance.IsTyping) yield return null;

        DialogueManager.Instance.StartDialogue(battleManager.PlayerUnit.Monster.Nickname + " " + "has gained" + " " + battleManager.EnemyUnit.Monster.Base.GivenXp.ToString() + " !");

        while(DialogueManager.Instance.IsTyping) yield return null;

        battleManager.GainXP(battleManager.EnemyUnit);

        battleManager.PlayerUnit.Monster.Regeneration(battleManager.PlayerUnit.Monster.MaxStamina);

        LoadingSceneManager.Instance.LoadLevelAsync("EncounterScene");
        Player.Instance.ExitBattle();
    }
}