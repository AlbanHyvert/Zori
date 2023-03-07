using System.Collections;
using Monster.Enum;
using UnityEngine;

public class AfterTurnState : BattleState
{
    public override IEnumerator Enter()
    {
        battleManager.StartCoroutine(RunAfterTurn(battleManager.PlayerUnit, battleManager.EnemyUnit));

        yield break;
    }

    private IEnumerator RunAfterTurn(ActiveMonster playerUnit, ActiveMonster enemyUnit)
    {
        if (playerUnit.Monster.Affliction != e_Afflictions.NONE)
        {
            switch (playerUnit.Monster.Affliction)
            {
                case e_Afflictions.BURN:
                    int burnDmg = Mathf.FloorToInt(playerUnit.Monster.Stats.MaxHp * 0.05f);

                    playerUnit.Monster.Damage(burnDmg);
                    break;
                case e_Afflictions.POISON:
                    float percentValue = 0.05f * (1 + playerUnit.AfflictedTurn);

                    int poisonDmg = Mathf.FloorToInt(playerUnit.Monster.Stats.MaxHp * percentValue);

                    playerUnit.Monster.Damage(poisonDmg);

                    if (playerUnit.Monster.Affliction == e_Afflictions.SLEEP)
                    {
                        playerUnit.Monster.SetAffliction(e_Afflictions.NONE);

                        Debug.Log(playerUnit.Monster.Nickname + " " + "is waking up");
                    }

                    playerUnit.AfflictedTurn++;
                    break;
                default:
                    break;
            }
        }

        if (enemyUnit.Monster.Affliction != e_Afflictions.NONE)
        {
            switch (enemyUnit.Monster.Affliction)
            {
                case e_Afflictions.BURN:
                    int burnDmg = Mathf.FloorToInt(enemyUnit.Monster.Stats.MaxHp * 0.05f);

                    enemyUnit.Monster.Damage(burnDmg);
                    break;
                case e_Afflictions.POISON:
                    float percentValue = 0.05f * (1 + enemyUnit.AfflictedTurn);

                    int poisonDmg = Mathf.FloorToInt(enemyUnit.Monster.Stats.MaxHp * percentValue);

                    enemyUnit.Monster.Damage(poisonDmg);

                    if (enemyUnit.Monster.Affliction == e_Afflictions.SLEEP)
                    {
                        enemyUnit.Monster.SetAffliction(e_Afflictions.NONE);

                        Debug.Log(enemyUnit.Monster.Nickname + " " + "is waking up");
                    }

                    enemyUnit.AfflictedTurn++;
                    break;
                default:
                    break;
            }
        }

        battleManager.SetState(new ActionTurnState());

        yield break;
    }

    public override void Exit()
    {
        battleManager.StopAllCoroutines();
    }
}