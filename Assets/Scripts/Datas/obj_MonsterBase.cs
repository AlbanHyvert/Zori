using UnityEngine;
using Monster.Enum;

[CreateAssetMenu(menuName = "Data/MonsterBase")]
public class obj_MonsterBase : ScriptableObject
{
    [Header("BASE")]
    [SerializeField] private Stats _stats = new Stats();
    [Space]
    [SerializeField] private Informations _informations = new Informations();
    [Header("Model")]
    [SerializeField] private MonsterModel _model = null;
    [Space]
    [SerializeField] private GenderRatio _genderRatio = new GenderRatio();
    [Space, Header("Types")]
    [SerializeField] private e_Types[] _types = new e_Types[2];
    [Space, Header("Passive")]
    [SerializeField] private Passive[] _passivePossibilities = new Passive[2];
    [Space]
    [SerializeField] private BattlePoints _givenBps = new BattlePoints();
    [SerializeField] private int _givenXp = 300;
    [Space]
    [SerializeField] private LearnableTech[] _learnableTechs = null;

#region Properties
    public Stats Stats
        => _stats;
    public Informations Information
        => _informations;
    public MonsterModel Model
        => _model;
    public GenderRatio GenderR
        => _genderRatio;
    public e_Types[] Types
        => _types;
    public Passive[] PassivePos
        => _passivePossibilities;
    public BattlePoints GivenBps
        => _givenBps;
    public int GivenXp
        => _givenXp;
    public LearnableTech[] LearnableTechs
        => _learnableTechs;
#endregion Properties

#region Structs
    [System.Serializable]
    public struct Informations
    {
        public string Id;
        public string Name;
        [TextArea] public string Description;
        public string Weight;
        public string Size;
        [Space]
        public e_XpStructure XpStructure;
    }
    [System.Serializable]
    public struct GenderRatio
    {
        public int Male;
        public int Female;
    }
    
    [System.Serializable]
    public struct LearnableTech
    {
        [Range(1,100)] public int Level;
        public obj_Techs Techs;
    }
#endregion Structs

    public e_Gender GenerateGender()
    {
        int rdmValue = Random.Range(0, 100);

        if(rdmValue < _genderRatio.Male)
            return e_Gender.MALE;
        
        return e_Gender.FEMALE;
    }

}
