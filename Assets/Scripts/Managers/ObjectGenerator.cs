using UnityEngine;
using System.Collections.Generic;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask = 0;
    [Space(10)]
    [SerializeField] private List<Generators> _generatorList = new List<Generators>();

    private List<MonsterController> _monsterList = new List<MonsterController>();

    private void Start()
    {
        GenerateObjects();
    }

    public void GenerateObjects()
    {

        for(int i = 0; i < _generatorList.Count; i++)
        {
            Bounds bounds = _generatorList[i].coll.bounds;

            Generators generator = _generatorList[i];

            for(int j = 0; j < generator.quantityToCreate; j++)
            {
                int rdmMonsterIndex = Random.Range(0, generator.monstersArray.Length);

                MonsterController monster = generator.monstersArray[rdmMonsterIndex];

                if(monster == null)
                {
                    j--;
                    return;
                }

                int rdmMonsterLevel = Random.Range(generator.minLevel, generator.maxLevel);

                CheckForBounds(generator, monster, bounds, rdmMonsterIndex, ref j);
            }
        }
    }

    private void CheckForBounds(Generators generators, MonsterController monsterController, Bounds bounds, int level, ref int curIndex)
    {
        Vector3 position = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
            if (!IsPositionObstructed(position, generators))
            {
                MonsterController monster = Instantiate(monsterController, position, Quaternion.identity);

                monster.OnStart(level);

                _monsterList.Add(monster);
            }
            else
                curIndex --;
    }

    private bool IsPositionObstructed(Vector3 position, Generators generator)
    {
        //RaycastHit hit;

        bool hitSomething = Physics.SphereCast(position, generator.radius, position, out RaycastHit hitInfo, generator.maxDistance, _layerMask);
        
        return hitSomething;
        
        /*if (Physics.Raycast(position, Vector3.down, out hit))
        {
            if (hit.collider.CompareTag("Terrain") || hit.collider.CompareTag("Plane"))
            {
                return true;
            }
        }
        return false;*/
    }

    [System.Serializable]
    private struct Generators
    {
        [Header("REGION")]
        [TextArea]
        [SerializeField] private string name;
        [Header("Collision Check")]
        public Collider coll;
        public float radius;
        public float maxDistance;
        [Header("Range Monster Level")]
        public int minLevel;
        public int maxLevel;
        [Header("Monster List To Generate")]
        public MonsterController[] monstersArray;
        public int quantityToCreate;
    }
}
