using UnityEngine;
using Monster.Enum;

public class PlayerTurnState : BattleState, ICalculateDamage
{
    public PlayerTurnState(BattleSystem battleSystem) : base(battleSystem)
    {

    }

    private float _timeToStart = 0;

    public override void Init()
    {
        
    }

    private void DestroyOldZori(float time)
    {
        if(time < _timeToStart)
            return;

    }

    private void CreateNewZori(float time)
    {
        if(time < _timeToStart)
            return;

    }

    private void CallNewZori(float time)
    {
        if(time < _timeToStart)
            return;

    }

    private void DialTimeOut(float time)
    {
        if(time < _timeToStart)
            return;

    }

    private void VfxTimeOut(float time)
    {
        if(time < _timeToStart)
            return;

    }

    private void DealDamage(float time)
    {
        if(time < _timeToStart)
            return;

    }

    public override void End()
    {

    }

#region Calculate Damage Done
    public int DealDamage(ActiveMonster activeSender, ActiveMonster activeReceiver)
    {
        float typeMult = TypeChart.GetEffectiveness(activeSender.TechUsed.Information.Type, activeReceiver.CurMonster.Base.Types[0]) * 
                                TypeChart.GetEffectiveness(activeSender.TechUsed.Information.Type, activeReceiver.CurMonster.Base.Types[1]);

        float modifier = CheckStab(activeSender.TechUsed, activeSender.CurMonster.Base.Types) * typeMult;

        float checkStatsDiff = CheckStatsDiff(activeSender.TechUsed, activeSender.CurMonster, activeReceiver.CurMonster);

        int dmg = Mathf.FloorToInt(((((((2 * activeSender.CurMonster.Level) / 5) + 2) * activeSender.TechUsed.Information.Power * checkStatsDiff) / 50 + 2) * modifier));

        return dmg;
    }

    public int HealValue()
    {
        throw new System.NotImplementedException();
    }

//---------------Check extra Damage---------------
//Check the difference between stats and return the value
    private float CheckStatsDiff(obj_Techs tech, Monsters sender, Monsters receiver)
    {
        switch (tech.Extra.Style)
        {
            case e_Styles.PHYSIC:
            return (float)sender.Stats.Atk / (float)receiver.Stats.Def;
            case e_Styles.SPECIAL:
            return (float)sender.Stats.SpeAtk / (float)receiver.Stats.SpeDef;
        }

        return 1;
    }

//Check bonus between the tech and the pokemon type
    private float CheckStab(obj_Techs Tech, e_Types[] zoriTypes)
    {
        for (int i = 0; i < zoriTypes.Length; i++)
        {
            if (Tech.Information.Type == zoriTypes[i])
            {
                return  1.5f;
            }
        }

        return 1f;
    }
#endregion Calculate Damage Done
}