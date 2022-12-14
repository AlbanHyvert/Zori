using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TeamHolder
{
    [SerializeField] private List<Monsters> _team = new List<Monsters>();

    public List<Monsters> Team
        => _team;

    public void Init()
    {
        foreach (Monsters monster in _team)
        {
            if(monster != null)
            {
                monster.Init();
            }
        }
    }

    public Monsters GetHealthyZori()
    {
        foreach (Monsters Monsters in _team)
        {
            if(Monsters.Hp > 0)
            {
                return Monsters;
            }
        }

        return null;
    }

    public Monsters AddMonster(Monsters monster)
    {
        for (int i = 0; i < _team.Count -1; i++)
        {
            if(_team[i] == null)
                return _team[i] = monster;
        }

        return null;
    }
}