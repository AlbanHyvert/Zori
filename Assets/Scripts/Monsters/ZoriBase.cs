using UnityEngine;
using Monster.Enum;
using Monster.Stat;

[CreateAssetMenu(fileName = "Zori", menuName = "Zori/Create new Zori")]
public class ZoriBase : ScriptableObject
{
    [Header("Information")]
    [SerializeField] private string _id = "001";
    [SerializeField] private string _name = string.Empty;
    [SerializeField, TextArea] private string _description = string.Empty;
    [SerializeField] private string _weight = string.Empty;
    [SerializeField] private string _size = string.Empty;
    [Space, SerializeField] private e_XpStructure _xpStructure = e_XpStructure.FAST;

    [Header("GenderRatio")]
    [SerializeField] private int _maleValue = 50;
    [SerializeField] private int _femaleValue = 50;

    [Header("Prefab")]
    [SerializeField] private ZoriBase _evolvedForm = null;
    [SerializeField] private GameObject _model = null;
    [SerializeField] private GameObject _chromaModel = null;

    [Header("Types")]
    [SerializeField] private e_Types[] _types = new e_Types[2];

    [Header("Base Stats")]
    [SerializeField] private BStats _bstats = new BStats();

    [Space, Header("Passive")]
    [SerializeField] private PassiveBase[] _passivePossibilities = new PassiveBase[2];  

    [Header("LearnableTech")]
    [SerializeField] private LearnableTech[] _learnableTechs = null;

    [Space, Header("GivenStats")]
    [SerializeField] private BPGiven[] _bpGiven = null;
    [SerializeField] private int _xpGiven = 1;

    [Space, Header("Conditions")]
    [SerializeField] private EvolveCondition _evolveCondition = null;

#region PROPERTIES
    public string Id
    => _id;
    public string Name
        => _name;
    public string Description
        => _description;
    public string Weight
        => _weight;
    public string Size
        => _size;
    public e_XpStructure XpStructure
        => _xpStructure;
    public ZoriBase EvolvedForm
        => _evolvedForm;
    public GameObject Model
        => _model;
    public GameObject ChromaModel
        => _chromaModel;
    public e_Types[] Types
        => _types;
    public BStats BStats
        => _bstats;
    public PassiveBase[] PassivePossibilities
        => _passivePossibilities;
    public LearnableTech[] LearnableTechs
        => _learnableTechs;
    public BPGiven[] BPGiven
        => _bpGiven;
    public int XpGiven
        => _xpGiven;

#endregion PROPERTIES

//Check conditions to evolve the Zori to its next stage
    public void CheckEvolveCondition(ZoriBase form, int level, e_WorldTime worldTime, e_Gender gender)
    {
        //Check conditions + none value
    }

//Choose a gender for the zori at its creation
    public e_Gender GenerateGender(e_Gender curGender)
    {
        int value = Random.Range(0,100);

        if(value <= _maleValue)
            return e_Gender.MALE;
        
        else if(value >= _femaleValue)
            return e_Gender.FEMALE;

        return e_Gender.MALE;
    }
}

#region SERIALIZABLE
//Battle Points that will be given to the zori when defeated
[System.Serializable]
public class BPGiven
{
    [SerializeField] private int _quantity = 1;

    [SerializeField] private e_BpGiven _bpGiven = e_BpGiven.HP;
}

//List of the learnable techs that the zori has access to.
[System.Serializable]
public class LearnableTech
{
    [SerializeField] private int _level = 1;
    [SerializeField] private TechBase _techBase = null;

    public int Level
        => _level;
    
    public TechBase TechBase
        => _techBase;
}

//Conditions for the Zori to evolve
[System.Serializable]
public class EvolveCondition
{
    [SerializeField] private e_WorldTime _worldTime = e_WorldTime.DAY;
    [SerializeField] private e_Gender _gender = e_Gender.ASEXUAL;
    [SerializeField] private int _level = 1;

    public e_WorldTime WorldTime
        => _worldTime;

    public e_Gender Gender
        => _gender;
    
    public int Level
        => _level;
}
#endregion SERIALIZABLE