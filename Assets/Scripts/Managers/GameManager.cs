using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float _worldSpeed = 1.5f;
    [Space, Tooltip("UpdatePlayerLastPos in second")]
    [SerializeField] private float _updatePlayerLastPos = 120;

    private float _time = 0;
    private float _timeBeforeSave = 0;
    private Player _player = null;

    private Transform _firstPlayerSpawnPos = null;
    private Vector3 _playerLastPosition = Vector3.zero;

    public Transform FirstPlayerSpawnPos
    { get => transform; set => _firstPlayerSpawnPos = value; }

#region Properties
    public float WorldSpeed
        => _worldSpeed;
    public float Time
        => _time;
    public Player GetPlayer(Player player)
    {
        if (_player != null)
            Destroy(player.gameObject);
        else
            return _player = player;

        return _player;
    }
#endregion Properties

#region Events
    private event Action _onUpdateInput;
    public event Action OnUpdateInput
    {
        add
        {
            _onUpdateInput -= value;
            _onUpdateInput += value;
        }
        remove
        {
            _onUpdateInput -= value;
        }
    }

    private event Action<float> _onUpdatePlayer;
    public event Action<float> OnUpdatePlayer
    {
        add
        {
            _onUpdatePlayer -= value;
            _onUpdatePlayer += value;
        }
        remove
        {
            _onUpdatePlayer -= value;
        }
    }

    private event Action<float> _onUpdateBattle;
    public event Action<float> OnUpdateBattle
    {
        add
        {
            _onUpdateBattle -= value;
            _onUpdateBattle += value;
        }
        remove
        {
            _onUpdateBattle -= value;
        }
    }

    private event Action<float> _onUpdateMonster;
    public event Action<float> OnUpdateMonster
    {
        add
        {
            _onUpdateMonster -= value;
            _onUpdateMonster += value;
        }
        remove
        {
            _onUpdateMonster -= value;
        }
    }

    private event Action<float> _onUpdateHUD;
    public event Action<float> OnUpdateHUD
    {
        add
        {
            _onUpdateHUD -= value;
            _onUpdateHUD += value;
        }
        remove
        {
            _onUpdateHUD -= value;
        }
    }
#endregion Events

    private void Start()
    {
        LoadingSceneManager.Instance.LoadLevelAsync("EncounterScene");
    }

    private void FixedUpdate()
    {
        _time = _worldSpeed * UnityEngine.Time.fixedDeltaTime;
        _timeBeforeSave += _time;

        if (_onUpdateInput != null)
            _onUpdateInput();
        
        if(_onUpdatePlayer != null)
            _onUpdatePlayer(_time);
        
        if(_onUpdateMonster != null)
            _onUpdateMonster(_time);
        
        if(_onUpdateBattle != null)
            _onUpdateBattle(_time);
        
        if(_onUpdateHUD != null)
            _onUpdateHUD(_time);

        if(_timeBeforeSave > _updatePlayerLastPos)
        {
            if(Player.Instance)
            {
                UpdatePlayerLastPos(Player.Instance.transform.position);
                _timeBeforeSave = 0;
            }
        }
    }

    public void UpdatePlayerLastPos(Vector3 pos)
        => _playerLastPosition = pos;

    public void ClearTime()
        => _time = 0;

    protected override void OnDestroy()
    {
        _onUpdateInput = null;
        _onUpdatePlayer = null;
        _onUpdateMonster = null;
        _onUpdateBattle = null;
        _onUpdateHUD = null;
    }
}