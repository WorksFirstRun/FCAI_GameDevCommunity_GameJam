using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObjectRefrence_SO> enemiesList; 
    [SerializeField] private Transform spawnAreaMin;   
    [SerializeField] private Transform spawnAreaMax;
    [SerializeField] private float spawnInterval;

    private float spawnTimer;

    private void Update()
    {
       
        spawnTimer -= Time.deltaTime;

        
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval; 
        }
    }

    private void SpawnEnemy()
    {
        float spawnX = Random.Range(spawnAreaMin.position.x, spawnAreaMax.position.x);
        float spawnY = Random.Range(spawnAreaMin.position.y, spawnAreaMax.position.y);
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);
        Transform whatToSpawn = enemiesList[Random.Range(0, enemiesList.Count)].entity;
        Instantiate(whatToSpawn, spawnPosition, Quaternion.identity);
    }

   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 size = new Vector2(spawnAreaMax.position.x - spawnAreaMin.position.x,spawnAreaMax.position.y - spawnAreaMin.position.y);
        Vector2 center = new Vector2(spawnAreaMin.position.x + spawnAreaMax.position.x,
            spawnAreaMax.position.y + spawnAreaMin.position.y);
        Gizmos.DrawWireCube((center) / 2.0f, size);
    }
}
