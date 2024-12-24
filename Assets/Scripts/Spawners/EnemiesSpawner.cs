using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private EnemyPooler.EnemyType spawnerType;
    [SerializeField] private Transform spawnAreaMin;   
    [SerializeField] private Transform spawnAreaMax;
    [SerializeField] private Vector2 playerDetectionArea;
    [SerializeField] private Transform playerDetectionOffset;
    [SerializeField] private float spawnInterval;
    [SerializeField] private LayerMask playerLayer;
    
    
    private float spawnTimer;

    private void Update()
    {
       
        spawnTimer -= Time.deltaTime;


        if (!(spawnTimer <= 0f)) return;
        if (!CheckIfPlayerInSpawner()) return;
        
        
        SpawnEnemy();
        spawnTimer = spawnInterval;
    }

    private void SpawnEnemy()
    {
        float spawnX = Random.Range(spawnAreaMin.position.x, spawnAreaMax.position.x);
        float spawnY = Random.Range(spawnAreaMin.position.y, spawnAreaMax.position.y);
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);
        EnemyPooler.Instance.SpawnFromPool(spawnerType, spawnPosition, Quaternion.identity);
    }

   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 size = new Vector2(spawnAreaMax.position.x - spawnAreaMin.position.x,spawnAreaMax.position.y - spawnAreaMin.position.y);
        Vector2 center = new Vector2(spawnAreaMin.position.x + spawnAreaMax.position.x,
            spawnAreaMax.position.y + spawnAreaMin.position.y);
        Gizmos.DrawWireCube((center) / 2.0f, size);


        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(playerDetectionOffset.position, playerDetectionArea);
    }
    
    

    private bool CheckIfPlayerInSpawner()
    {
        Collider2D hitColliders = Physics2D.OverlapBox(playerDetectionOffset.position, playerDetectionArea, 0f,playerLayer);
        return hitColliders is not null;
    }
}
