using UnityEngine;

[System.Serializable]
public class Encounter
{
    [SerializeField] private GameObject _npc = null;
    [SerializeField] private Monsters _wildMonster = null;

    public GameObject NPC
        => _npc;

    public Monsters WildMonster
        => _wildMonster;
}