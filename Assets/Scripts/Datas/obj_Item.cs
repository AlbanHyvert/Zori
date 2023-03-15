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
    public e_Names Name
        => _name;
    public float Value
        => _value;

    public void Use(Monsters monster, List<obj_Item> list)
    {
        switch (_name)
        {
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