using UnityEngine;
using Monster.Enum;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Item")]
public class obj_Item : ScriptableObject
{
    [Header("Information")]
    [SerializeField] private string _id = string.Empty;
    [TextArea, SerializeField] private string _description = string.Empty;
    [SerializeField] private Sprite _icon = null;
    [SerializeField] private float _value = 1;
    [SerializeField, Range(1,500)] private int _price = 1;
    [Space(10)]
    [Header("Category")]
    [SerializeField] private e_Type _type = e_Type.Held;
    [SerializeField] private e_Names _name = e_Names.NormalCharm;
    [SerializeField] private e_Category _category = e_Category.Battle_Item;
    
    public string ReturnName()
        => GameManager.EnumToString(_name);

    public string ReturnType()
        => GameManager.EnumToString(_type);

    public string ReturnCategory()
        => GameManager.EnumToString(Category);

    public e_Category Category
        => _category;

    public void Use(Monsters monster, List<obj_Item> list)
    {
        switch (_name)
        {
            case e_Names.CrystAura:
                break;
            case e_Names.RareCrystAura:
                break;
            case e_Names.MysticCrystAura:
                break;
            case e_Names.LegendaryCrystAura:
                break;
            case e_Names.VerdantCrystAura:
                break;
            case e_Names.StormingCrystAura:
                break;
            case e_Names.MoltenCrystAura:
                break;
            case e_Names.CosmicCrystAura:
                break;
            case e_Names.ClimaticCrystAura:
                break;
            case e_Names.TemporalCrystAura:
                break;
            case e_Names.ChromaticCrystAura:
                break;
            case e_Names.LinkCrystAura:
                break;
            case e_Names.CoolBalm:
                if(monster.Affliction.Equals(e_Afflictions.BURN))
                {
                    Cure(monster);
                    list.Remove(this);
                }
                break;
            case e_Names.Antidote:
                if(monster.Affliction.Equals(e_Afflictions.POISON))
                {
                    Cure(monster);
                    list.Remove(this);
                }
                break;
            case e_Names.VigorPill:
                if(monster.Affliction.Equals(e_Afflictions.PARALYSIS))
                {
                    Cure(monster);
                    list.Remove(this);
                }
                break;
            case e_Names.EnergyDrink:
                if(monster.Affliction.Equals(e_Afflictions.SLEEP))
                {
                    Cure(monster);
                    list.Remove(this);
                }
                break;
            case e_Names.HeatingPad:
                if(monster.Affliction.Equals(e_Afflictions.FREEZE))
                {
                    Cure(monster);
                    list.Remove(this);
                }
                break;
            case e_Names.VitalityJam:
                AddBattlePoints(e_BpGiven.HP, monster);
                list.Remove(this);
                break;
            case e_Names.StrengthJam:
                AddBattlePoints(e_BpGiven.ATK, monster);
                list.Remove(this);
                break;
            case e_Names.ArmorJam:
                AddBattlePoints(e_BpGiven.DEF, monster);
                list.Remove(this);
                break;
            case e_Names.PowerJam:
                AddBattlePoints(e_BpGiven.SPATK, monster);
                list.Remove(this);
                break;
            case e_Names.ResistJam:
                AddBattlePoints(e_BpGiven.SPDEF, monster);
                list.Remove(this);
                break;
            case e_Names.AgilityJam:
                AddBattlePoints(e_BpGiven.SPEED, monster);
                list.Remove(this);
                break;
            case e_Names.NullJam:
                AddBattlePoints(e_BpGiven.HP, monster);
                AddBattlePoints(e_BpGiven.ATK, monster);
                AddBattlePoints(e_BpGiven.DEF, monster);
                AddBattlePoints(e_BpGiven.SPATK, monster);
                AddBattlePoints(e_BpGiven.SPDEF, monster);
                AddBattlePoints(e_BpGiven.SPEED, monster);
                list.Remove(this);
                break;
            case e_Names.VitalityReduction:
                RemoveBattlePoints(e_BpGiven.HP, monster);
                list.Remove(this);
                break;
            case e_Names.StrengthReduction:
                RemoveBattlePoints(e_BpGiven.ATK, monster);
                list.Remove(this);
                break;
            case e_Names.ArmorReduction:
                RemoveBattlePoints(e_BpGiven.DEF, monster);
                list.Remove(this);
                break;
            case e_Names.PowerReduction:
                RemoveBattlePoints(e_BpGiven.SPATK, monster);
                list.Remove(this);
                break;
            case e_Names.ResistReduction:
                RemoveBattlePoints(e_BpGiven.SPDEF, monster);
                list.Remove(this);
                break;
            case e_Names.AgilityReduction:
                RemoveBattlePoints(e_BpGiven.SPEED, monster);
                list.Remove(this);
                break;
            case e_Names.TraitToken:
                break;
            case e_Names.AbilityToken:
                break;
            case e_Names.FierceScroll:
                break;
            case e_Names.EagerScroll:
                break;
            case e_Names.CarefulScroll:
                break;
            case e_Names.ResilientScroll:
                break;
            case e_Names.SealedScroll:
                break;
            case e_Names.Elixir:
                Heal(monster);
                list.Remove(this);
                break;
            case e_Names.TimeLocket:
                break;
            case e_Names.GreaterElixir:
                Heal(monster);
                list.Remove(this);
                break;
            case e_Names.SuperiorElixir:
                Heal(monster);
                list.Remove(this);
                break;
            case e_Names.Tonic:
                Regenerate(monster);
                list.Remove(this);
                break;
            case e_Names.GreaterTonic:
                Regenerate(monster);
                list.Remove(this);
                break;
            case e_Names.SuperiorTonic:
                Regenerate(monster);
                list.Remove(this);
                break;
            default:
                break;
        }
    }

    public void Use(obj_Techs tech, List<obj_Item> list)
    {
        switch (_name)
        {
            case e_Names.NormalCharm:
                if(tech.Information.Type.Equals(e_Types.NEUTRAL))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.FireCharm:
                if(tech.Information.Type.Equals(e_Types.PYRO))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.WaterCharm:
                if(tech.Information.Type.Equals(e_Types.HYDRO))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.NatureCharm:
                if(tech.Information.Type.Equals(e_Types.PHYTO))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.ElectricCharm:
                if(tech.Information.Type.Equals(e_Types.ELECTRO))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.IceCharm:
                if(tech.Information.Type.Equals(e_Types.CRYO))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.ToxicCharm:
                if(tech.Information.Type.Equals(e_Types.VENO))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.EarthCharm:
                if(tech.Information.Type.Equals(e_Types.GEO))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.WindCharm:
                if(tech.Information.Type.Equals(e_Types.AERO))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.HiveCharm:
                if(tech.Information.Type.Equals(e_Types.INSECTO))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.IronCharm:
                if(tech.Information.Type.Equals(e_Types.MENTAL))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.BattleCharm:
                if(tech.Information.Type.Equals(e_Types.MARTIAL))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.PsyCharm:
                if(tech.Information.Type.Equals(e_Types.MENTAL))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.GhostCharm:
                if(tech.Information.Type.Equals(e_Types.SPECTRAL))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.DarkCharm:
                if(tech.Information.Type.Equals(e_Types.UMBRA))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            case e_Names.LightCharm:
                if(tech.Information.Type.Equals(e_Types.LUMA))
                {
                    BoostTechPower(tech);
                    list.Remove(this);
                }
                break;
            default:
                break;
        }
    }

    private void Heal(Monsters monster)
    {
        monster.Heal((int)_value);
    }

    private void Regenerate(Monsters monster)
    {
        monster.Regeneration((int)_value);
    }

    private void Cure(Monsters monster)
    {
        monster.SetAffliction(e_Afflictions.NONE);
    }

    private void BoostTechPower(obj_Techs tech)
    {
        tech.Information.SetBonus(_value);
    }

    private void AddBattlePoints(e_BpGiven stat, Monsters monster)
    {
        monster.BattlePoints.UpdateBp(stat, (int)_value);
    }

    private void RemoveBattlePoints(e_BpGiven stat, Monsters monster)
    {
        monster.BattlePoints.RemoveBp(stat, (int)_value);
    }
}

public enum e_Names
{
    NormalCharm,
    FireCharm,
    WaterCharm,
    NatureCharm,
    ElectricCharm,
    IceCharm,
    ToxicCharm,
    EarthCharm,
    WindCharm,
    HiveCharm,
    IronCharm,
    BattleCharm,
    PsyCharm,
    GhostCharm,
    DarkCharm,
    LightCharm,
    ToughChains,
    TwistedChains,
    PolishedChains,
    CrystAura,
    RareCrystAura,
    MysticCrystAura,
    LegendaryCrystAura,
    VerdantCrystAura,
    StormingCrystAura,
    MoltenCrystAura,
    CosmicCrystAura,
    ClimaticCrystAura,
    TemporalCrystAura,
    ChromaticCrystAura,
    LinkCrystAura,
    CoolBalm,
    Antidote,
    VigorPill,
    EnergyDrink,
    HeatingPad,
    NormalPlate,
    FirePlate,
    WaterPlate,
    NaturePlate,
    ElectricPlate,
    IcePlate,
    ToxicPlate,
    EarthPlate,
    WindPlate,
    HivePlate,
    IronPlate,
    BattlePlate,
    PsyPlate,
    GhostPlate,
    DarkPlate,
    LightPlate,
    VitalityJam,
    StrengthJam,
    ArmorJam,
    PowerJam,
    ResistJam,
    AgilityJam,
    NullJam,
    BarbedArmor,
    MirrorArmor,
    VitalityReduction,
    StrengthReduction,
    ArmorReduction,
    PowerReduction,
    ResistReduction,
    AgilityReduction,
    TraitToken,
    AbilityToken,
    FierceScroll,
    EagerScroll,
    CarefulScroll,
    ResilientScroll,
    SealedScroll,
    TrainingBandana,
    Elixir,
    TimeLocket,
    GreaterElixir,
    SuperiorElixir,
    Tonic,
    GreaterTonic,
    SuperiorTonic,
    BravePad,
    FishingRod,
    SynergyConduits,
    BraveryEmblemsBox,
    SynergyBracelet,
    SynergyStone,
    RollerbladeShoes,
    EternalLink,
    SynergyCrystal
}
public enum e_Type
{
    Escape = 0,
    Repels,
    Evolution,
    Valuable,
    Exchangeable,
    Held,
    Cryst_Aura,
    HP_restoring,
    Afflictions_Curing,
    Reviving,
    Stamina_Restoring,
    Stats_Increasing,
    Ability_Changing,
    Technique_Learning,
    Story,
    Synergy,
    Capture_Helper
}