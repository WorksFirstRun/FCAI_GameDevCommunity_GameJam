using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooler : MonoBehaviour
{
    public enum EnemyType
    {
        DarkSamurai,
        WaterWitch
    }

    [System.Serializable]
    public class EnemyPool
    {
        public EnemyType enemyType;
        public GameObjectRefrence_SO prefab;
        public int amount;
    }

    private Dictionary<EnemyType, Queue<GameObject>> poolDictionary;
    [SerializeField] private List<EnemyPool> poolList;

    public static EnemyPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<EnemyType, Queue<GameObject>>();

        foreach (EnemyPool enemyPool in poolList)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < enemyPool.amount; i++)
            {
                Transform obj = Instantiate(enemyPool.prefab.entity);
                obj.GetComponent<Enemy>().InitializeTheEnemy();
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj.gameObject);
            }
            
            poolDictionary.Add(enemyPool.enemyType,objectPool);
        }
    }


    public GameObject SpawnFromPool(EnemyType enemyType, Vector2 location, Quaternion quaternion)
    {
        if (!poolDictionary.ContainsKey(enemyType))
        {
            Debug.LogWarning("enemy type not found");
            return null;
        }
        
        GameObject obj = poolDictionary[enemyType].Dequeue();

        if (obj.activeInHierarchy)
        {
            obj.GetComponent<BaseHealthScript>().ResetHealthToMax();
        }

        else
        {
            obj.GetComponent<IEnemyEnable>().EnableBackTheEnemy();
        }
        
        obj.transform.position = location;
        obj.transform.rotation = quaternion;
        
        poolDictionary[enemyType].Enqueue(obj);

        return obj;
    }
}
