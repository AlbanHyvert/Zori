using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private Monsters _data = new Monsters();

    public Monsters Data
        => _data;

    public Monsters SetData(Monsters newData)
        => _data = newData;
}
