using UnityEngine;
using Monster.Enum;

public class CalculateDamage
{
//Calculate and Return the damage deal to the monster
    public int DealDamage(TechBase tech, Zori sender, Zori receiver)
    {
        float typeMult = TypeChart.GetEffectiveness(tech.Type, receiver.Base.Types[0]) * TypeChart.GetEffectiveness(tech.Type, receiver.Base.Types[1]);

        float modifier = CheckStab(tech, sender.Base.Types) * typeMult;

        float checkStatsDiff = CheckStatsDiff(tech, sender, receiver);

        if(tech.Power <= 0)
            return 0;

        int dmg = Mathf.FloorToInt(((((((2 * sender.Level) / 5) + 2) * tech.Power * checkStatsDiff) / 50 + 2) * modifier));

        return dmg;
    }

    public e_Afflictions DealAfflictions()
    {
        return e_Afflictions.NONE;
    }

//Check the difference between stats and return the value
    private float CheckStatsDiff(TechBase tech, Zori sender, Zori receiver)
    {
        switch (tech.Style)
        {
            case e_Styles.PHYSIC:
            return (float)sender.Stats.Atk / (float)receiver.Stats.Def;
            case e_Styles.SPECIAL:
            return (float)sender.Stats.SpAtk / (float)receiver.Stats.SpDef;
        }

        return 1;
    }

//Check bonus between the tech and the pokemon type
    private float CheckStab(TechBase Tech, e_Types[] zoriTypes)
    {
        for (int i = 0; i < zoriTypes.Length; i++)
        {
            if (Tech.Type == zoriTypes[i])
            {
                return  1.5f;
            }
        }

        return 1f;
    }
}