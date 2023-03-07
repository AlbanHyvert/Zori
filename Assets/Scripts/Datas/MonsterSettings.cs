using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerSetting")]
public class MonsterSettings : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _runSpeed = 1f;
    [SerializeField] protected float _acceleration = 2f;
    [SerializeField] private float _maxWalkVelocity = 10;
    [SerializeField] private float _maxRunVelocity = 10;
    [SerializeField] private float _gravityForce = 10;
    [Space]
    [Header("Jump")]
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private float _airTime = 5;
    [Space]
    [Header("Target")]
    [SerializeField, Range(0, 360)] private float _fieldOfViewAngle = 10;
    [SerializeField] private float _maxViewDistance = 10;
    [SerializeField] private float _chaseDuration = 1f;

    public float WalkSpeed
        => _walkSpeed;
    public float RunSpeed
        => _runSpeed;

    public float Acceleration
        => _acceleration;

    public float MaxWalkVelocity
        => _maxWalkVelocity;

    public float MaxRunVelocity
        => _maxRunVelocity;

    public float JumpForce
        => _jumpForce;

    public float AirTime
        => _airTime;

    public float GravityForce
        => _gravityForce;

    public float FieldOfView
        => _fieldOfViewAngle;
    
    public float MaxViewDistance
        => _maxViewDistance;

    public float ChaseDuration
        => _chaseDuration;
}
