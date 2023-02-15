using Monster.Enum;
using UnityEngine;

[System.Serializable]
public class BattlePoints
{
    [SerializeField] private int _maxHp = 0;
    [SerializeField] private int _atk = 0;
    [SerializeField] private int _def = 0;
    [SerializeField] private int _speAtk = 0;
    [SerializeField] private int _speDef = 0;
    [SerializeField] private int _speed = 0;

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

    public void UpdateBp(e_BpGiven bpGiven, int value)
    {
        switch(bpGiven)
        {
            case e_BpGiven.HP :
                _maxHp += value;
                if(_maxHp > 200)
                    _maxHp = 200;
            break;
            case e_BpGiven.ATK :
                _atk += value;
                if(_atk > 200)
                    _atk = 200;
            break;
            case e_BpGiven.DEF :
                _def += value;
                if(_def > 200)
                    _def = 200;
            break;
            case e_BpGiven.SPATK :
                _speAtk += value;
                if(_speAtk > 200)
                    _speAtk = 200;
            break;
            case e_BpGiven.SPDEF :
                _speDef += value;
                if(_speDef > 200)
                        _speDef = 200;
            break;
            case e_BpGiven.SPEED :
                _speed += value;
                if(_speed > 200)
                    _speed = 200;
            break;
        }
    }
}
