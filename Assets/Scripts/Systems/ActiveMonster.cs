using UnityEngine;

public class ActiveMonster : MonoBehaviour
{
    [SerializeField] private bool _isPlayer = false;
    
    private GameObject _model = null;
    private Monsters _monster = null;
    private obj_Techs _techUsed = null;

    private int _coldCount = 0;

    public Monsters CurMonster
        => _monster;

    public bool IsPlayer
        => _isPlayer;

    public obj_Techs TechUsed
        => _techUsed;

    public int ColdCount
    { get => _coldCount; set => _coldCount = value; }

    public Monsters SetMonster(Monsters monsters, bool isToCreate = false)
    {
        _monster = monsters;

        if(isToCreate == true)
            _model = Instantiate(_monster.Base.Model.Prefab, Position(), Quaternion.identity);

        return _monster;
    }

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