using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<Transform> spawnPoints = new List<Transform>(); // 스폰 포인트들
    public List<GameObject> enemyPrefabs = new List<GameObject>(); // 3종류 몬스터 프리팹

    [Header("Spawn Timing")]
    public float spawnInterval = 3f;
    public int maxEnemies = 10;

    private int currentEnemyCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemies) return;
        if (spawnPoints.Count == 0 || enemyPrefabs.Count == 0) return;

        // 랜덤 스폰포인트 선택
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // 랜덤 몬스터 타입 선택
        GameObject randomEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // 몬스터 생성
        GameObject enemy = Instantiate(randomEnemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
        currentEnemyCount++;

        //Debug.Log($"몬스터 생성: {enemy.name} at {randomSpawnPoint.name}");
    }

    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
    }
}
