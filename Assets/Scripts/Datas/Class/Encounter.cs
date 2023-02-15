using UnityEngine;

[System.Serializable]
public class Encounter
{
    private GameObject _npc = null;
    private Monsters _wildMonster = null;

    public GameObject NPC
        => _npc;

    public Monsters WildMonster
        => _wildMonster;

    public GameObject SetNpc(GameObject newNpc)
        => _npc = newNpc;

    public Monsters SetWild(Monsters newMonster)
        => _wildMonster = newMonster;
}