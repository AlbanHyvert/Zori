using UnityEngine;

//Modify slightly a stat value to give it a bonus or malus bonus
    [System.Serializable]
    public class StatsBoost
    {
        [SerializeField] private StatsModificator _statMod = new StatsModificator();

        public StatsModificator StatsMod
            => _statMod;

        [System.Serializable]
        public struct StatsModificator
        {
            [SerializeField, Range(-6, 6)] private int atk;
            [SerializeField, Range(-6, 6)] private int def;
            [SerializeField, Range(-6, 6)] private int spAtk;
            [SerializeField, Range(-6, 6)] private int spDef;
            [SerializeField, Range(-6, 6)] private int speed;

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

            public void Reset()
            {
                atk = 0;
                def = 0;
                spAtk = 0;
                spDef = 0;
                speed = 0;
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
    }