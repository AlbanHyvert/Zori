using UnityEngine;
using Monster.Enum;

namespace Monster.Stat
{
    //Base Stats are only used at the creation of a Zori.
    [System.Serializable]
    public class BStats
    {
        [SerializeField] private int _hp = 0;
        [SerializeField] private int _atk = 0;
        [SerializeField] private int _def = 0;
        [SerializeField] private int _spAtk = 0;
        [SerializeField] private int _spDef = 0;
        [SerializeField] private int _speed = 0;
        [SerializeField] private int _sta = 50;

        public int Hp
            => _hp;

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
        
        public int Sta
            => _sta;
    }
    //Stats are the actual value the Zori will use to fight and roam in the world.
    [System.Serializable]
    public struct Stat
    {
        [SerializeField] private int hp;
        [SerializeField] private int atk;
        [SerializeField] private int def;
        [SerializeField] private int spAtk;
        [SerializeField] private int spDef;
        [SerializeField] private int speed;
        [SerializeField] private int sta;

        public int Hp
            => hp;

        public int Atk
            => atk;

        public int Def
            => def;

        public int SpAtk
            => spAtk;

        public int SpDef
            => spDef;

        public int Speed
            => speed;
        
        public int Sta
            => sta;
    

        public void CalculateStats(BStats bStats, BattlePoint bp, int level, e_Traits traits)
        {
            hp = ((2 * bStats.Hp + (bp.HP / 5)) * level) / 100 + level + 10;
            atk = (((2 * bStats.Atk + (bp.atk / 4)) * level) / 100) + 5;
            def = (((2 * bStats.Def + (bp.def / 4)) * level) / 100) + 5;
            spAtk = (((2 * bStats.SpAtk + (bp.spAtk / 4)) * level) / 100) + 5;
            spDef = (((2 * bStats.SpDef + (bp.spDef / 4)) * level) / 100) + 5;
            speed = (((2 * bStats.Speed + (bp.speed / 4)) * level) / 100) + 5;
            sta = bStats.Sta + (level / 2);

           switch (traits)
            {
                case e_Traits.BOASTFUL:
                    speed = (int)(speed + (speed * 0.1f));
                    def = (int)(def - (def * 0.1f));
                    break;

                case e_Traits.COMBATIVE:
                    atk = (int)(atk + (atk * 0.1f));
                    spAtk = (int)(spAtk - (spAtk * 0.1f));
                    break;

                case e_Traits.DREAMY:
                    spDef = (int)(spDef + (spDef * 0.1f));
                    speed = (int)(speed - (speed * 0.1f));              
                    break;
                
                case e_Traits.EASY_GOING:
                    def = (int)(def + (def * 0.1f));
                    speed = (int)(speed - (speed * 0.1f));
                    break;

                case e_Traits.GENTLE:
                    spAtk = (int)(spAtk + (spAtk * 0.1f));
                    def = (int)(def - (def * 0.1f));
                    break;

                case e_Traits.GULLIBLE:
                    speed = (int)(speed + (speed * 0.1f));
                    spDef = (int)(spDef - (spDef * 0.1f));
                    break;

                case e_Traits.GUTSY:
                    def = (int)(def + (def * 0.1f));
                    atk = (int)(atk - (atk * 0.1f));
                    break;

                case e_Traits.IRRITABLE:
                    atk = (int)(atk + (atk * 0.1f));
                    spDef = (int)(spDef - (spDef * 0.1f));
                    break;

                case e_Traits.JOYFUL:
                    speed = (int)(speed + (speed * 0.1f));
                    spAtk = (int)(spAtk - (spAtk * 0.1f));
                    break;

                case e_Traits.KIND:
                    spDef = (int)(spDef + (spDef * 0.1f));
                    def = (int)(def - (def * 0.1f));
                    break;

                case e_Traits.LIVELY:
                    speed = (int)(speed + (speed * 0.1f));
                    atk = (int)(atk - (atk * 0.1f));
                    break;

                case e_Traits.METICULOUS:
                    spAtk = (int)(spAtk + (spAtk * 0.1f));
                    speed = (int)(speed - (speed * 0.1f));
                    break;

                case e_Traits.OBSTINATE:
                    atk = (int)(atk + (atk * 0.1f));
                    speed = (int)(speed - (speed * 0.1f));
                    break;

                case e_Traits.PEACEFUL:
                    spDef = (int)(spDef + (spDef * 0.1f));
                    atk = (int)(atk - (atk * 0.1f));
                    break;
                
                case e_Traits.REBEL:
                    atk = (int)(atk + (atk * 0.1f));
                    def = (int)(def - (def * 0.1f));
                    break;

                case e_Traits.SENSITIVE:
                    spAtk = (int)(spAtk + (spAtk * 0.1f));
                    spDef = (int)(spDef - (spDef * 0.1f));
                    break;
                
                case e_Traits.SMART:
                    spAtk = (int)(spAtk + (spAtk * 0.1f));
                    atk = (int)(atk - (atk * 0.1f));
                    break;
                
                case e_Traits.STURDY:
                    def = (int)(def + (def * 0.1f));
                    spAtk = (int)(spAtk - (spAtk * 0.1f));
                    break;
                
                case e_Traits.WITHDRAWN:
                    spDef = (int)(spDef + (spDef * 0.1f));
                    spAtk = (int)(spAtk - (spAtk * 0.1f));
                    break;
                
                case e_Traits.WORRIED:
                    def = (int)(def + (def * 0.1f));
                    spDef = (int)(spDef - (spDef * 0.1f));
                    break;
            default :          
                break;
           }
        }
    }
    // Battle Points are a bonus stats type system to give the zori an slight advantage in a fight
    [System.Serializable]
    public struct BattlePoint
    {
        [Range(0,200)] public int HP;
        [Range(0,200)] public int atk;
        [Range(0,200)] public int def;
        [Range(0,200)] public int spAtk;
        [Range(0,200)] public int spDef;
        [Range(0,200)] public int speed;

        public void UpdateBp(e_BpGiven bpGiven, int value)
        {
           switch(bpGiven)
           {
                case e_BpGiven.HP :
                    HP += value;
                    if(HP > 200)
                        HP = 200;
                break;
                case e_BpGiven.ATK :
                    atk += value;
                    if(atk > 200)
                        atk = 200;
                break;
                case e_BpGiven.DEF :
                    def += value;
                    if(def > 200)
                        def = 200;
                break;
                case e_BpGiven.SPATK :
                    spAtk += value;
                    if(spAtk > 200)
                        spAtk = 200;
                break;
                case e_BpGiven.SPDEF :
                    spDef += value;
                    if(spDef > 200)
                        spDef = 200;
                break;
                case e_BpGiven.SPEED :
                    speed += value;
                    if(speed > 200)
                        speed = 200;
                break;
                default:
                break;
           }
        }
    }
}