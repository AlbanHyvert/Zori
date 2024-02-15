using System;
using UnityEngine;

public class Player : Singleton<Player>
{
    [SerializeField] private Inventory _inventory = new Inventory();
    [SerializeField] private TeamHolder _teamHolder = new TeamHolder();
    [Space]
    [SerializeField] private Encounter _encounter = new Encounter();
    [Space]
    [SerializeField] private PlayerController _controller = null;

    public Inventory Inventory
        => _inventory;
    public TeamHolder TeamHolder
        => _teamHolder;

    public Encounter Encounter
        => _encounter;

    private event Action<bool> _isInBattle = null;
    public event Action<bool> IsInBattle
    {
        add
        {
            _isInBattle -= value;
            _isInBattle += value;
        }
        remove
        {
            _isInBattle -= value;
        }
    }

    private event Action<bool> _isLoaded = null;
    public event Action<bool> IsLoaded
    {
        add
        {
            _isLoaded -= value;
            _isLoaded += value;
        }
        remove
        {
            _isLoaded -= value;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _teamHolder.Init();

        if(_encounter.WildMonster != null )
            _encounter.WildMonster.Init();

        GameManager.Instance.GetPlayer(this);
    }

    private void Start()
    {
        if (_isInBattle != null)
            _isInBattle(false);
    }

    public void InitBattle(GameObject encounter = null)
    {
        //Check npc, if null

        if (_isInBattle != null)
            _isInBattle(true);

        MonsterController monsterController = encounter.GetComponent<MonsterController>();

        if (monsterController == null) return;

        Monsters monsters = monsterController.Data;

        if (monsters == null) return;

        _encounter.SetWild(monsters);

        LoadingSceneManager.Instance.LoadLevelAsync("BattleScene");
    }

    public void Loaded(bool value)
    {
        Debug.Log("Loaded: " + value);

        if(_isLoaded != null)
        {
            _isLoaded(value);
        }
    }

    public void ExitBattle()
    {
        if (_isInBattle != null)
            _isInBattle(false);
    }
}