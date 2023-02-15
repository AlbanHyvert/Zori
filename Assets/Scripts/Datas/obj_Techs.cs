using UnityEngine;
using Monster.Enum;

[CreateAssetMenu(menuName = "Data/Techs")]
public class obj_Techs : ScriptableObject
{
    [SerializeField] private Informations _information = new Informations();
    [Space]
    [SerializeField] private Extras _extras = new Extras();
    [Space, Header("Priority")]
    [SerializeField, Range(-1, 5)] private int _priority = 0;

    public Informations Information
        => _information;
    public Extras Extra
        => _extras;
    public int Priority
        => _priority;

#region Structs
    [System.Serializable]
    public struct Informations
    {
        public string Name;
        [TextArea] public string Description;
        public e_Types Type;
        [Header("Cost")]
        public int Power;
        public int Stamina;
    }
    [System.Serializable]
    public struct Extras
    {
        public e_Tags Tag;
        public e_Styles Style;
        public e_Targets Target;
        public Effects Effect;
    }
    [System.Serializable]
    public struct Effects
    {
        public e_Afflictions affliction;
        public StatsBoost statsMod;
    }
#endregion Structs
}