using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private Monsters _data = new Monsters();

    public Monsters Data
        => _data;

    public Monsters SetData(Monsters newData)
        => _data = newData;

    private void Start()
    {
        _data.Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit " + other.name);

        Player_Controller player = other.gameObject.GetComponent<Player_Controller>();

        if (player)
            Player.Instance.InitBattle(gameObject);
    }
}
