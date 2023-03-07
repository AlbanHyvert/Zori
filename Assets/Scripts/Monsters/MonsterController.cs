using UnityEngine;

public class MonsterController : ControllerStateMachine
{
    [SerializeField] private Monsters _data = new Monsters();
    [Space(10)]
    [SerializeField] private MonsterSettings _settings = null;
    [Space(10)]
    [SerializeField] private MeshRenderer _meshRenderer = null;

    private Rigidbody _rb = null;
    private Bounds _area = new Bounds();

    private Transform _target = null;

    public Monsters Data
        => _data;
    
    public MonsterSettings Settings
        => _settings;

    public Rigidbody Rigidbody
        => _rb;

    public Monsters SetData(Monsters newData)
        => _data = newData;

    public Bounds Area
        => _area;

    public Transform Target
        => _target;
    public Transform SetTarget(Transform target)
        => _target = target;

    public MeshRenderer MeshRenderer
        => _meshRenderer;

    public void OnStart(int level, Bounds bounds)
    {
        _rb = GetComponent<Rigidbody>();

        if(_rb == null)
        {
            _rb = gameObject.AddComponent<Rigidbody>();

            _rb.freezeRotation = true;
        }

        _data.Init(level);

        _area = bounds;

        mController = this;

        SetState(new Monster_IdleState());
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player)
        {
            Player.Instance.InitBattle(gameObject);
            Destroy(gameObject);
        }
    }
}