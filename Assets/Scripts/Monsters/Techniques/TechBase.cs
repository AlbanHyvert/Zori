using UnityEngine;
using Monster.Enum;
using Monster.Utilities;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Move", menuName = "Zori/Create new Tech")]
public class TechBase : ScriptableObject
{
    [Header("Information")]
    [SerializeField] private string _name = string.Empty;
    [SerializeField, TextArea] private string _description = string.Empty;
    [Space]
    [SerializeField, Header("Type")] private e_Types _type = e_Types.NONE;
    [Space, Header("Cost")]
    [SerializeField] private int _power = 1;
    [SerializeField] private int _staminaCost = 1;
    [Space, Header("Extras")]
    [SerializeField] private e_Tags _tag = e_Tags.NONE;
    [SerializeField] private e_Styles _style = e_Styles.PHYSIC;
    [SerializeField] private e_Targets _target = e_Targets.OPPONENT;
    [SerializeField] private Effects _effect = new Effects();
    [Space, Header("Priority")]
    [SerializeField, Range(-1, 5)] private int _priority = 0;

#region PROPERTIES
    public string Name
        => _name;
    public string Description
        => _description;
    public e_Types Type
        => _type;
    public int Power
        => _power;
    public int StaCost
        => _staminaCost;
    public e_Tags Tag
        => _tag;
    public e_Styles Style
        => _style;
    public e_Targets Target
        => _target;
    public Effects Effect
        => _effect;
    public int Priority
        => _priority;
#endregion PROPERTIES
    
    [System.Serializable]
    public struct Effects
    {
        [SerializeField] private e_Afflictions affliction;
        [SerializeField] private AdditionalEffects statsMod;
    }
}