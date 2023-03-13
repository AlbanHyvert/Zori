using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSetting _settings = null;
    [SerializeField] private Rigidbody _rb = null;
    [Space]
    [SerializeField] private CameraController _camController = null;

    private bool _isChased = false;
    private bool _isLoaded = false;
    private bool _isInBattle = false;

    public bool IsChased
        => _isChased;

    public void SetIsChased(bool value)
        => _isChased = value;

    private Vector3 _velocity = Vector3.zero;

    public Vector3 Velocity
    { get { return _velocity; } set { _velocity = value; } }

    private void Start()
    {
        _camController.InBattle(false);

        Player.Instance.IsInBattle += InBattle;
        Player.Instance.IsLoaded += IsLoaded;

        GameManager.Instance.OnUpdatePlayer += Tick;

        ObjectVisibilityManager.Instance.SetCamera(_camController.Camera);
    }

    private void Tick(float time)
    {
        Vector3 dir = Vector3.zero;

        if(Input.GetKey(KeyCode.Z))
        {
            dir += transform.forward;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            dir -= transform.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir -= transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir += transform.right;
        }

        Vector3 desiredMovement = dir * _settings.Speed;

        _velocity = Vector3.MoveTowards(_velocity, desiredMovement, _settings.Acceleration * time);

        _velocity = Vector3.ClampMagnitude(_velocity, _settings.MaxVelocity);

        _rb.velocity = _velocity;
    }

    private void InBattle(bool value)
    {
        _camController.InBattle(value);

        _isInBattle = value;

        if (value == true)
        {
            GameManager.Instance.UpdatePlayerLastPos(this.transform.position);

            GameManager.Instance.OnUpdatePlayer -= Tick;
            _rb.isKinematic = false;
            _rb.useGravity = true;
        }
        else
        {
            this.transform.position = GameManager.Instance.LastPosition;

            GameManager.Instance.OnUpdatePlayer += Tick;
            _rb.useGravity = false;
            _rb.isKinematic = true;
        }
    }

    private void IsLoaded (bool value)
    {
        _isLoaded = value;

        if(value == true)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
        }
        else
        {
            _rb.useGravity = false;
            _rb.isKinematic = true;
        }
    }
}