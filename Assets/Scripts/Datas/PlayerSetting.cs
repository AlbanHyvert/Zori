using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerSetting")]
public class PlayerSetting : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float _speed = 1f;
    [SerializeField] protected float _acceleration = 2f;
    [SerializeField] private float _maxVelocity = 10;
    [SerializeField] private float _gravityForce = 10;
    [Space]
    [Header("Jump")]
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private float _airTime = 5;
    [Space]
    [Header("Rotation")]
    [SerializeField] private float _rotationSpeedX = 10;
    [SerializeField] private float _rotationSpeedY = 10;
    [SerializeField] private float _smoothSpeed = 1;

    public float Speed
        => _speed;

    public float Acceleration
        => _acceleration;

    public float MaxVelocity
        => _maxVelocity;

    public float JumpForce
        => _jumpForce;

    public float AirTime
        => _airTime;

    public float GravityForce
        => _gravityForce;

    public float RotationSpeedX
    { get => _rotationSpeedX; set => _rotationSpeedX = value;}

    public float RotationSpeedY
    { get => _rotationSpeedY; set => _rotationSpeedY = value;}

    public float SmoothSpeed
        => _smoothSpeed;
}