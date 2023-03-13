using System.Collections;

public class LostState : BattleState
{
    public override IEnumerator Enter()
    {
        battleManager.HUD.ActivateBattleHUD(false);

        DialogueManager.Instance.StartDialogue("You have lost !");

        while(DialogueManager.Instance.IsTyping) yield return null;

        DialogueManager.Instance.StartDialogue("You do not have any healthy zori anymore.");

        while(DialogueManager.Instance.IsTyping) yield return null;

        LoadingSceneManager.Instance.LoadLevelAsync("EncounterScene");

        Player.Instance.ExitBattle();
    }
}
