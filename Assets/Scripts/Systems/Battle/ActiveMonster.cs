using UnityEngine;

public class ActiveMonster : MonoBehaviour
{
    [SerializeField] private bool _isPlayer = false;
    
    private GameObject _model = null;
    private Monsters _monster = null;
    private Monsters _newMonster = null;
    private obj_Techs _techUsed = null;
    private int _afflictedTurn = 0;

    private int _coldCount = 0;

    public Monsters Monster
        => _monster;
    
    public Monsters NewMonster
    => _newMonster;

    public bool IsPlayer
        => _isPlayer;

    public obj_Techs TechUsed
        => _techUsed;

    public int ColdCount
    { get => _coldCount; set => _coldCount = value; }
    public int AfflictedTurn
    { get => _afflictedTurn; set => _afflictedTurn = value; }

    public Monsters SetMonster(Monsters monsters, bool isToCreate = false)
    {
        _monster = monsters;

        if(isToCreate == true)
            _model = Instantiate(_monster.Base.Model.Prefab, Position(), Quaternion.identity);

        return _monster;
    }

    public Monsters SetSwitch(Monsters monster)
    => _newMonster = monster;

    public obj_Techs SetTech(obj_Techs techs)
        => _techUsed = techs;

    public void CreateMonsterToScene()
    {
        _model = Instantiate(_monster.Base.Model.Prefab, Position(), Quaternion.identity);
    }

    public void DestroyModel()
        => Destroy(_model, 0.1f);

    public Vector3 Position()
        => transform.position;

    public Vector3 Rotation()
        => transform.rotation.eulerAngles;
}