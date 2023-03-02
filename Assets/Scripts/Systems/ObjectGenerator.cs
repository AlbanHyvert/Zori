using UnityEngine;
using System.Collections.Generic;

public class ObjectGenerator : MonoBehaviour
{
    [Header("TERRAIN")]
    [SerializeField] private float _offsetFromGround = 0.2f;
    [SerializeField] private LayerMask _layerMask = 0;
    [Space(10)]
    [SerializeField] private List<Generators> _generatorList = new List<Generators>();

    private List<MonsterController> _monsterList = new List<MonsterController>();
    private float terrainHeight = 0f;
    private Vector3 terrainNormal = Vector3.up;

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

                CheckForBounds(generator, monster, bounds, rdmMonsterLevel, ref j);
            }
        }
    }

    private void CheckForBounds(Generators generators, MonsterController monsterController, Bounds bounds, int level, ref int curIndex)
    {
            Vector3 position = GetRandomPosition(generators);

            if (!IsPositionObstructed(position, generators))
            {
                MonsterController monster = Instantiate(monsterController, position, Quaternion.identity);

                Renderer renderer = monster.GetComponentInChildren<Renderer>();

                if(renderer != null) ObjectVisibilityManager.Instance.AddRenderer(renderer);

                monster.OnStart(level);
                _monsterList.Add(monster);
            }
            else
                curIndex --;
    }

    private Vector3 GetRandomPosition(Generators generators)
    {
        Vector3 spawnPos = Vector3.zero;
    
        // Get a random point within the collider bounds
        spawnPos = new Vector3(
                Random.Range(generators.coll.bounds.min.x, generators.coll.bounds.max.x),
                generators.coll.bounds.center.magnitude,
                Random.Range(generators.coll.bounds.min.z, generators.coll.bounds.max.z));
    
        // Cast a ray downwards from the random point to find the ground level
        RaycastHit hit;
        if (Physics.Raycast(spawnPos, Vector3.down, out hit, Mathf.Infinity, _layerMask))
        {
            // Adjust the spawn position by subtracting the offset from the hit point
            spawnPos = hit.point - Vector3.up * _offsetFromGround;

            return spawnPos;
        }
        else if (Physics.Raycast(spawnPos, Vector3.up, out hit, Mathf.Infinity, _layerMask))
        {
            // Adjust the spawn position by subtracting the offset from the hit point
            spawnPos = hit.point - Vector3.down / _offsetFromGround;
        }
    
        return spawnPos;
    }

    private bool IsPositionObstructed(Vector3 position, Generators generator)
    {
        //RaycastHit hit;

        bool hitSomething = Physics.SphereCast(position, generator.radius, position, out RaycastHit hitInfo, generator.maxDistance, _layerMask);
        
        return hitSomething;
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