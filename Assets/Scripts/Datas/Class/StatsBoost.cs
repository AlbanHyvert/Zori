using UnityEngine;

//Modify slightly a stat value to give it a bonus or malus bonus
[System.Serializable]
public class StatsBoost
{
    [SerializeField, Range(-6, 6)] private int _atk;
    [SerializeField, Range(-6, 6)] private int _def;
    [SerializeField, Range(-6, 6)] private int _spAtk;
    [SerializeField, Range(-6, 6)] private int _spDef;
    [SerializeField, Range(-6, 6)] private int _speed;

    public int Atk
        => _atk;
    public int Def
        => _def;
    public int SpAtk
        => _spAtk;
    public int SpDef
        => _spDef;
    public int Speed
        => _speed;

    public void Reset()
    {
        _atk = 0;
        _def = 0;
        _spAtk = 0;
        _spDef = 0;
        _speed = 0;
    }

    public void UpdateBoost(int atk, int def, int spAtk, int spDef, int speed)
    {
        _atk += atk;
        _def += def;
        _spAtk += spAtk;
        _spDef += spDef;
        _speed += speed;
    }

    public float ReturnModificator(int value)
    {
        switch (value)
        {
            case -6:
                return 0.25f;
            case -5:
                return 0.29f;
            case -4:
                return 0.33f;
            case -3:
                return 0.40f;
            case -2:
                return 0.5f;
            case -1:
                return 0.67f;
            case 0:
                return 1f;
            case 1:
                return 1.5f;
            case 2:
                return 2f;
            case 3:
                return 2.5f;
            case 4:
                return 3f;
            case 5:
                return 3.5f;
            case 6:
                return 4f;
        }
        return 1f;
    }
}
