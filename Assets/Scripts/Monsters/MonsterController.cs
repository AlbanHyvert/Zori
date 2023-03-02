using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private Monsters _data = new Monsters();

    public Monsters Data
        => _data;

    public Monsters SetData(Monsters newData)
        => _data = newData;

    public void OnStart(int value)
    {
        _data.SetLevel(value);
        
        _data.Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit " + other.name);

        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player)
        {
            Player.Instance.InitBattle(gameObject);
            Destroy(gameObject);
        }
    }
}