using UnityEngine.Events;
using System;
public class GameManager : Singleton<GameManager>
{
    #region Events
    private event Action onUpdateBSystem;
    public event Action OnUpdateBSystem
    {
        add
        {
            onUpdateBSystem -= value;
            onUpdateBSystem += value;
        }
        remove
        {
            onUpdateBSystem -=value;
        }
    }
    
    private event Action onUpdatePlayer;
    public event Action OnUdpatePlayer
    {
        add
        {
            onUpdatePlayer -= value;
            onUpdatePlayer += value;
        }
        remove
        {
            onUpdatePlayer -=value;
        }
    }
    
    private event Action onUpdateCamera;
    public event Action OnUdpateCamera
    {
        add
        {
            onUpdateCamera -= value;
            onUpdateCamera += value;
        }
        remove
        {
            onUpdateCamera -=value;
        }
    }
   
    private event Action onUpdateInputs;
    public event Action OnUdpateInputs
    {
        add
        {
            onUpdateInputs -= value;
            onUpdateInputs += value;
        }
        remove
        {
            onUpdateInputs -=value;
        }
    }
    
    private event Action onUpdateWorld;
    public event Action OnUdpateWorld
    {
        add
        {
            onUpdateWorld -= value;
            onUpdateWorld += value;
        }
        remove
        {
            onUpdateWorld -=value;
        }
    }
    
    private event Action onUpdateMonsters;
    public event Action OnUdpateMonsters
    {
        add
        {
            onUpdateMonsters -= value;
            onUpdateMonsters += value;
        }
        remove
        {
            onUpdateMonsters -=value;
        }
    }

    #endregion Events

    private void FixedUpdate()
    {
        if(onUpdateInputs != null)
            onUpdateInputs();

        if(onUpdateCamera != null)
            onUpdateCamera();

        if(onUpdatePlayer != null)
            onUpdatePlayer();
        
        if(onUpdateWorld != null)
            onUpdateWorld();
        
        if(onUpdateMonsters != null)
            onUpdateMonsters();
        
        if(onUpdateBSystem != null)
            onUpdateBSystem();
    }

    private void OnDestroy()
    {
        onUpdateInputs = null;

        onUpdateCamera = null;

        onUpdatePlayer = null;
        
        onUpdateWorld = null;
        
        onUpdateMonsters = null;
        
        onUpdateBSystem = null;
        
    }
}