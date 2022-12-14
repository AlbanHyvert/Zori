using UnityEngine;
using Monster.Enum;
using Monster.Stat;
using Monster.Utilities;
using UnityEngine.Events;
public class Zori : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] private ZoriBase _base = null;

    [Header("Information")]
    [SerializeField] private string _nickname = string.Empty;
    [SerializeField, Range(1, 100)] private int _level = 1;
    [Space ,SerializeField] private BattlePoint _battlePoint = new BattlePoint();
    [SerializeField] private bool _isChroma = false;
    
    [Header("Animator")]
    [SerializeField] private Animator _animator = null;

#region VARIABLES
    private AdditionalEffects _addEffects = new AdditionalEffects();
    private Stat _stats = new Stat();
    private e_Gender _gender = e_Gender.NONE;
    private PassiveBase _passive = null;
    private e_Traits _trait = e_Traits.NONE;
    private TechBase[] _techniques = new TechBase[4];
    private int _curStamina = 1;
    private int _curHp = 1;
    private int _curExperience = 0;
    private int _xpLevelUp = 1;
    private GameObject _item = null;
    private bool _isInit = false;
#endregion VARIABLES

//Information needed OUTSIDE the scripts
#region PROPERTIES
public ZoriBase Base
    => _base;
public string Nickname
    => _nickname;
public int Level
    => _level;
public Stat Stats
    => _stats;
public e_Gender Gender
    => _gender;
public PassiveBase Passive
    => _passive;
public e_Traits Trait
    => _trait;
public TechBase[] Techniques
    => _techniques;
public int CurSta
    => _curStamina;
public int CurHp
    => _curHp;
public int CurXp
    => _curExperience;
public AdditionalEffects AddEffects
    =>_addEffects;
public BattlePoint BPoint
        => _battlePoint;
public Animator Animator
    => _animator;
#endregion PROPERTIES

#region EVENTS
public UnityAction<int> onDamaged;
public UnityAction<int> onUpdateHealth;
public UnityAction<int> onUpdateStamina;
public UnityAction<int> onStaUsed;
#endregion EVENTS
    
    //HAS to happen BEFORE everything else for the creation of the zori to happen
    public void Init()
    {
        onDamaged += Damaged;
        onStaUsed += RemoveStaCost;
        if(!_isInit)
            _isInit = true;

        int rdmPassive = UnityEngine.Random.Range(0, 1);
        int rdmTrait = UnityEngine.Random.Range(0, 19);

        if(_base.PassivePossibilities[rdmPassive] != null)
            _passive = _base.PassivePossibilities[rdmPassive];
        else
            _passive = _base.PassivePossibilities[0];

        _trait = (e_Traits)rdmTrait;

        CheckTechs(_base.LearnableTechs);
        _stats.CalculateStats(_base.BStats, _battlePoint, _level, _trait);

        _curHp = _stats.Hp;
        _curStamina = _stats.Sta;

        _gender = _base.GenerateGender(_gender);

        if(_nickname == string.Empty)
            _nickname = _base.Name;
    }
    
    // Checks the different techniques POSSIBLY usable at the CURRENT time by the Zori
    private void CheckTechs(LearnableTech[] learnableTech)
    {
        int j = 0;
        for (int i = 0; i < learnableTech.Length; i++)
        {
            if(learnableTech[i] != null)
            {
                if(j < _techniques.Length)
                    if(learnableTech[i].Level <= _level)
                    {
                        AddTechs(learnableTech[i], j);
                        j++;
                    }
            }
                
        }
    }
    
    //Add the currently usable techs to the zori to use
    private void AddTechs(LearnableTech learnTech,int i)
    {
        _techniques[i] = learnTech.TechBase;
    }
    
    //Add the chosen name to the Zori
    public void SetNickname(string name)
        => _nickname = name;
    
    //Impact the current Health of the zori
    private void Damaged(int value)
    {
        _curHp -= value;

        if(_curHp < 0)
            _curHp = 0;

        onUpdateHealth(_curHp);
    }

    private void RemoveStaCost(int value)
    {
        _curStamina -= value;

        if(_curStamina < 0)
            _curStamina = 0;

        onUpdateStamina(_curStamina);
    }
}