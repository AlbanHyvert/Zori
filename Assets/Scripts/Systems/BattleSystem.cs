using UnityEngine;
using UnityEngine.Events;

public class BattleSystem : BattleStateMachine
{
    [Header("HUD")]
    [SerializeField] private BattleHUD _playerHUD = null;
    [SerializeField] private BattleHUD _enemyHUD = null;
    [Space, Header("Zori")]
    [SerializeField] private Zori _playerZori = null;
    [SerializeField] private Zori _enemyZori = null;
    [Header("Tech Selector")]
    [SerializeField] private TechSelector _techSelector = null;

    private State _oldState = null;
    private TechBase _pTech = null;
    private TechBase _eTech = null;
    private CalculateDamage _calculateDamage = new CalculateDamage();

#region PROPERTIES
    public Zori PlayerZori
        => _playerZori;
    public Zori EnemyZori
        => _enemyZori;
    public CalculateDamage CalculateDamage
        => _calculateDamage;
    public BattleHUD PlayerHUD
        => _playerHUD;
    public BattleHUD EnemyHUD
        => _enemyHUD;
    public TechSelector TechSelector
        => _techSelector;
    public TechBase PTech
        => _pTech;
    public TechBase ETech
        => _eTech;
    public State OldState
        => _oldState;
#endregion PROPERTIES

//SetUp battle information
    private void Awake()
    {
        _oldState = null;
        
        _playerZori.Init();
        _enemyZori.Init();

        _playerHUD.Informations.SetInformations(_playerZori);
        _enemyHUD.Informations.SetInformations(_enemyZori);

        SetState(new ActionTurnState(this));
    }

//Switch Zori function during the battle
    public void SwitchPlayerZori(Zori newZori)
        => _playerZori = newZori;
    public void SwitchEnemyZori(Zori newZori)
        => _enemyZori = newZori;

//Set Zori Techs
    public void SetPlayerTech(TechBase tech)
    {
        _pTech = tech;
        onTechValid.Invoke();
    }
    public void SetEnemyTech(TechBase tech)
    {
        _eTech = tech;
    }

//Generate Enemy Attack
public TechBase EnemyTech()
{
    int ChosenTech = 0;
    int techQuantity = 0;

    for (int i = 0; i < _enemyZori.Techniques.Length; i++)
    {
        if(_enemyZori.Techniques[i] != null)
            techQuantity++;
    }

    ChosenTech = Random.Range(0, techQuantity);

    return _enemyZori.Techniques[ChosenTech];
}

private UnityAction onTechValid;
public UnityAction onUpdate;

//Set the first battle State
    private void Start()
    {
        GameManager.Instance.OnUpdateBSystem += OnUpdate;

        onTechValid += FightReady;

        _playerZori.onUpdateHealth += _playerHUD.UpdateHp;
        _enemyZori.onUpdateHealth += _enemyHUD.UpdateHp;

        _playerZori.onUpdateStamina += _playerHUD.UpdateSta;
        _enemyZori.onUpdateStamina += _enemyHUD.UpdateSta;


        for (int i = 0; i < _playerZori.Techniques.Length; i++)
        {
            if(_playerZori.Techniques[i] == null)
                return;

            _techSelector.ButtonDatas[i].SetTech(_playerZori.Techniques[i]);
            _techSelector.ButtonDatas[i].Text.text = _playerZori.Techniques[i].Name;
;       }

        
    }

    private void OnUpdate()
    {
        if(onUpdate != null)
        {
            onUpdate();
        }
    }

    private void FightReady()
    {
        if(_pTech != null)
        {
            State.Start();
        }
    }

    public State SetOldState(State curState)
        => _oldState = curState;
}