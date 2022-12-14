using UnityEngine;
using Monster.Enum;

[System.Serializable]
public class Stats
{
    [SerializeField] private int _maxHp = 0;
    [SerializeField] private int _atk = 0;
    [SerializeField] private int _def = 0;
    [SerializeField] private int _speAtk = 0;
    [SerializeField] private int _speDef = 0;
    [SerializeField] private int _speed = 0;
    [SerializeField] private int _maxStamina = 0;

    public int MaxHp
        => _maxHp;
    public int Atk
        => _atk;
    public int Def
        => _def;
    public int SpeAtk
        => _speAtk;
    public int SpeDef
        => _speDef;
    public int Speed
        => _speed;
    public int MaxStamina
        => _maxStamina;

    public void Init( int level, obj_MonsterBase mBase, BattlePoints bp, e_Traits traits)
        {
            UpdateStats(level, mBase, bp);

           switch (traits)
            {
                case e_Traits.BOASTFUL:
                    _speed = (int)(_speed + (_speed * 0.1f));
                    _def = (int)(_def - (_def * 0.1f));
                    break;

                case e_Traits.COMBATIVE:
                    _atk = (int)(_atk + (_atk * 0.1f));
                    _speAtk = (int)(_speAtk - (_speAtk * 0.1f));
                    break;

                case e_Traits.DREAMY:
                    _speDef = (int)(_speDef + (_speDef * 0.1f));
                    _speed = (int)(_speed - (_speed * 0.1f));              
                    break;
                
                case e_Traits.EASY_GOING:
                    _def = (int)(_def + (_def * 0.1f));
                    _speed = (int)(_speed - (_speed * 0.1f));
                    break;

                case e_Traits.GENTLE:
                    _speAtk = (int)(_speAtk + (_speAtk * 0.1f));
                    _def = (int)(_def - (_def * 0.1f));
                    break;

                case e_Traits.GULLIBLE:
                    _speed = (int)(_speed + (_speed * 0.1f));
                    _speDef = (int)(_speDef - (_speDef * 0.1f));
                    break;

                case e_Traits.GUTSY:
                    _def = (int)(_def + (_def * 0.1f));
                    _atk = (int)(_atk - (_atk * 0.1f));
                    break;

                case e_Traits.IRRITABLE:
                    _atk = (int)(_atk + (_atk * 0.1f));
                    _speDef = (int)(_speDef - (_speDef * 0.1f));
                    break;

                case e_Traits.JOYFUL:
                    _speed = (int)(_speed + (_speed * 0.1f));
                    _speAtk = (int)(_speAtk - (_speAtk * 0.1f));
                    break;

                case e_Traits.KIND:
                    _speDef = (int)(_speDef + (_speDef * 0.1f));
                    _def = (int)(_def - (_def * 0.1f));
                    break;

                case e_Traits.LIVELY:
                    _speed = (int)(_speed + (_speed * 0.1f));
                    _atk = (int)(_atk - (_atk * 0.1f));
                    break;

                case e_Traits.METICULOUS:
                    _speAtk = (int)(_speAtk + (_speAtk * 0.1f));
                    _speed = (int)(_speed - (_speed * 0.1f));
                    break;

                case e_Traits.OBSTINATE:
                    _atk = (int)(_atk + (_atk * 0.1f));
                    _speed = (int)(_speed - (_speed * 0.1f));
                    break;

                case e_Traits.PEACEFUL:
                    _speDef = (int)(_speDef + (_speDef * 0.1f));
                    _atk = (int)(_atk - (_atk * 0.1f));
                    break;
                
                case e_Traits.REBEL:
                    _atk = (int)(_atk + (_atk * 0.1f));
                    _def = (int)(_def - (_def * 0.1f));
                    break;

                case e_Traits.SENSITIVE:
                    _speAtk = (int)(_speAtk + (_speAtk * 0.1f));
                    _speDef = (int)(_speDef - (_speDef * 0.1f));
                    break;
                
                case e_Traits.SMART:
                    _speAtk = (int)(_speAtk + (_speAtk * 0.1f));
                    _atk = (int)(_atk - (_atk * 0.1f));
                    break;
                
                case e_Traits.STURDY:
                    _def = (int)(_def + (_def * 0.1f));
                    _speAtk = (int)(_speAtk - (_speAtk * 0.1f));
                    break;
                
                case e_Traits.WITHDRAWN:
                    _speDef = (int)(_speDef + (_speDef * 0.1f));
                    _speAtk = (int)(_speAtk - (_speAtk * 0.1f));
                    break;
                
                case e_Traits.WORRIED:
                    _def = (int)(_def + (_def * 0.1f));
                    _speDef = (int)(_speDef - (_speDef * 0.1f));
                    break;
            default :          
                break;
           }
        }
    
    public void UpdateStats(int level, obj_MonsterBase mBase, BattlePoints bp)
    {
        _maxHp = ((2 * mBase.Stats.MaxHp + (bp.MaxHp / 5)) * level) / 100 + level + 10;
        _atk = (((2 * mBase.Stats.Atk + (bp.Atk / 4)) * level) / 100) + 5;
        _def = (((2 * mBase.Stats.Def + (bp.Def / 4)) * level) / 100) + 5;
        _speAtk = (((2 * mBase.Stats.SpeAtk + (bp.SpeAtk / 4)) * level) / 100) + 5;
        _speDef = (((2 * mBase.Stats.SpeDef + (bp.SpeDef / 4)) * level) / 100) + 5;
        _speed = (((2 * mBase.Stats.Speed + (bp.Speed / 4)) * level) / 100) + 5;
        _maxStamina = mBase.Stats.MaxStamina + (level / 2);
    }
}
