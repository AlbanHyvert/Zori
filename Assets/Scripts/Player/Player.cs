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

        if(_encounter.WildMonster != null )
            _encounter.WildMonster.Init();

        GameManager.Instance.GetPlayer(this);
    }

    public void InitBattle(GameObject encounter = null)
    {
        //Check npc, if null

        MonsterController monsterController = encounter.GetComponent<MonsterController>();

        if (monsterController == null) return;

        Monsters monsters = monsterController.Data;

        if (monsters == null) return;

        _encounter.SetWild(monsters);

        GameManager.Instance.LoadBattleScene();
    }
}