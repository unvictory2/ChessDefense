using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<Transform> spawnPoints = new List<Transform>(); // ���� ����Ʈ��
    public List<GameObject> enemyPrefabs = new List<GameObject>(); // 3���� ���� ������

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

        // ���� ��������Ʈ ����
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // ���� ���� Ÿ�� ����
        GameObject randomEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // ���� ����
        GameObject enemy = Instantiate(randomEnemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
        currentEnemyCount++;

        //Debug.Log($"���� ����: {enemy.name} at {randomSpawnPoint.name}");
    }

    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
    }
}
