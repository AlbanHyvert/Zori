using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float _worldSpeed = 1.5f;

    private float _time = 0;

#region Properties
    public float WorldSpeed
        => _worldSpeed;
    public float Time
        => _time;
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

    private event Action _onUpdatePlayer;
    public event Action OnUpdatePlayer
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

    private event Action _onUpdateMonster;
    public event Action OnUpdateMonster
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

    private void FixedUpdate()
    {
        _time += _worldSpeed * UnityEngine.Time.fixedDeltaTime;

        if(_onUpdateInput != null)
            _onUpdateInput();
        
        if(_onUpdatePlayer != null)
            _onUpdatePlayer();
        
        if(_onUpdateMonster != null)
            _onUpdateMonster();
        
        if(_onUpdateBattle != null)
            _onUpdateBattle(Time);
        
        if(_onUpdateHUD != null)
            _onUpdateHUD(Time);
    }

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