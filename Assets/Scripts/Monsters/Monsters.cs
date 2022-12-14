using UnityEngine;
using System;
using Monster.Enum;

[System.Serializable]
public class Monsters : IHealth, IStamina
{
    private bool isInit = false;

    [SerializeField] private obj_MonsterBase _base = null;
    [Space(10), Header("Informations")]
    [Range(1, 100)]
    [SerializeField] private int _level = 1; 
    [Space]
    [SerializeField] private Animator _animator = null;
    [Header("Dev Tools")]
    [Space ,SerializeField] private BattlePoints _battlePoints = new BattlePoints();

#region Variables
    private string _nickname = string.Empty;
    private e_Afflictions _affliction = e_Afflictions.NONE;
    private e_Gender _gender = e_Gender.NONE;
    private e_Traits _trait = e_Traits.NONE;
    private Passive _passive = null;
    private bool _isChroma = false;
    private int _xpLevelUp = 100;
    private GameObject _item = null;
    private int _hp = 0;
    private int _stamina = 0;
    private int _experience = 0;
    private Stats _stats = new Stats();
    private obj_Techs[] _techs = new obj_Techs[4];
    private StatsBoost _statsBoostHolder = new StatsBoost();
    
#endregion Variables

#region Properties
    public string Nickname
        => _nickname;
    public int Level
        => _level; 
    public Stats Stats
        => _stats;
    public e_Afflictions Affliction
        => _affliction;
    public obj_Techs[] Techs
        => _techs;
    public StatsBoost StatsBoost
        => _statsBoostHolder;
    public obj_MonsterBase Base
        => _base;
    public int Hp
        => _hp;
    public int Stamina
        => _stamina;
#endregion Properties

#region  Events
    private event Action<int> _onUpdateHealth;
    public event Action<int> OnUpdateHealth
    {
        add
        {
            _onUpdateHealth -= value;
            _onUpdateHealth += value;
        }
        remove
        {
            _onUpdateHealth -= value;
        }
    }

    private event Action<int> _onUpdateStamina;
    public event Action<int> OnUpdateStamina
    {
        add
        {
            _onUpdateStamina -= value;
            _onUpdateStamina += value;
        }
        remove
        {
            _onUpdateStamina -= value;
        }
    }
#endregion Events

    public void Init()
    {
        if(isInit == false)
            isInit = true;
        else
            return;
//-----------------------Set Passive & Traits-----------------------
        int rdmPassive = UnityEngine.Random.Range(0, 1);
        int rdmTrait = UnityEngine.Random.Range(0, 19);

        if(_base.PassivePos[rdmPassive] != null)
            _passive = _base.PassivePos[rdmPassive];
        else
            _passive = _base.PassivePos[0];

        _trait = (e_Traits)rdmTrait;
//-----------------------Set Techs & Init Stats-----------
        CheckTechs(_base.LearnableTechs);
        _stats.Init(_level, Base, _battlePoints, _trait);
//-----------------------Set Hp & Stamina-----------------
        _hp = _stats.MaxHp;
        _stamina = _stats.MaxStamina;
//-----------------------Set Gender-----------------------
        _gender = _base.GenerateGender();
//-----------------------Set Nickname---------------------
        if(_nickname == string.Empty)
            _nickname = _base.name;
    }

    private void CheckTechs(obj_MonsterBase.LearnableTech[] learnableTech)
    {
        int j = 0;

        for (int i = 0; i < learnableTech.Length; i++)
        {
            obj_Techs tech = learnableTech[i].Techs;
            int techLevel = learnableTech[i].Level;

            if(tech == null)
                return;

            if(techLevel < _level)
            {
                _techs[j] = tech;

                j++;
            }
        }
    }

    private void CalculateNewMaxXp()
        {
            int level = _level;
            int maxXp = _xpLevelUp;

           switch (_base.Information.XpStructure)
           {
            case e_XpStructure.PARABOLIC:
                maxXp = Mathf.FloorToInt((1.2f * (level^3)) - (15 * (level^2))
                             + (100 * level - 140));
                break;
            case e_XpStructure.SLOW:
                maxXp = Mathf.FloorToInt((1.25f * (level^3)));
                break;
            case e_XpStructure.MEDIUM:
                maxXp = Mathf.FloorToInt(level^3);
                break;
            case e_XpStructure.FAST:
                maxXp = Mathf.FloorToInt(0.8f * (level^3));
                break;
           }
        }

    public void SetLevel(int level)
        => _level = level;
    public string SetNickname(string newName)
        => _nickname = newName;

#region Interfaces
//--------------------------Health--------------------------
    //Remove value to the Hp
    public void Damage(int value)
    {
        int newHp = _hp - value;

        if(newHp <= 0)
        {
            _hp = 0;
            _affliction = e_Afflictions.KO;
        }
        else
            _hp = newHp;

        _onUpdateHealth(_hp);
    }

    //Add value to the Hp
    public void Heal(int value)
    {
        if(_hp == 0)
        {
            _affliction = e_Afflictions.NONE;
        }

        int newHp = _hp + value;

        if(newHp > _stats.MaxHp)
            _hp = _stats.MaxHp;
        else
            _hp = newHp;

        _onUpdateHealth(_hp);
    }
//--------------------------Stamina--------------------------
    //Add value to the stamina
    public void Regeneration(int value)
    {
        int newSta = _stamina + value;

        if(newSta > _stats.MaxStamina)
            _stamina = _stats.MaxStamina;
        else
            _stamina = newSta;

        _onUpdateStamina(_stamina);
    }

    //Remove value to the Stamina
    public void Cost(int value)
    {
        int newSta = _stamina - value;

        if(newSta < 0)
            _stamina = 0;
        else
            _stamina = newSta;

        _onUpdateStamina(_stamina);
    }
#endregion Interfaces
}