using UnityEngine;

public class Player : Singleton<Player>
{
    [Header("Inventory")]
    [SerializeField] private Inventory _inventory = new Inventory();
    [Space]
    [SerializeField] private Encounter _encounter = new Encounter();

    public Inventory Inventory
        => _inventory;

    public Encounter Encounter
        => _encounter;

    protected override void Awake()
    {
        base.Awake();

        _inventory.Init();
        _encounter.WildMonster.Init();
    }
}
