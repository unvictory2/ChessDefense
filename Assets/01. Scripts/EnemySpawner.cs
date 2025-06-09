using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    [Header("Spawn Timing")]
    public float baseSpawnInterval = 3f; // �ʱ� ���� ����
    public float minSpawnInterval = 1f; // �ּ� ���� ����
    public float spawnIntervalDecrease = 0.5f; // 20�ʸ��� ���ҷ�
    public int baseMaxEnemies = 10; // �⺻ �ִ� �� ��
    public float maxEnemiesIncreaseInterval = 20f; // 20�ʸ��� 3������ ����

    private int currentEnemyCount = 0;
    private float elapsedTime = 0f;
    private float spawnTimer = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        float currentSpawnInterval = GetCurrentSpawnInterval();

        if (spawnTimer >= currentSpawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount >= GetCurrentMaxEnemies()) return;
        if (spawnPoints.Count == 0 || enemyPrefabs.Count == 0) return;

        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        int enemyIndex = GetWeightedEnemyIndex(elapsedTime);
        GameObject enemyPrefab = enemyPrefabs[enemyIndex];

        GameObject enemy = Instantiate(enemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
        currentEnemyCount++;
    }

    // 20�ʸ��� �ִ� ���� �� 3������ ����
    private int GetCurrentMaxEnemies()
    {
        int increments = Mathf.FloorToInt(elapsedTime / maxEnemiesIncreaseInterval);
        return baseMaxEnemies + increments * 3;
    }

    // 20�ʸ��� spawnInterval ���� (�ּҰ� ����)
    private float GetCurrentSpawnInterval()
    {
        int decrements = Mathf.FloorToInt(elapsedTime / maxEnemiesIncreaseInterval);
        float interval = baseSpawnInterval - decrements * spawnIntervalDecrease;
        return Mathf.Max(minSpawnInterval, interval);
    }

    // �ð��� ���� ���� ���� Ȯ��
    private int GetWeightedEnemyIndex(float time)
    {
        float[] weights;
        if (time < 20f)
            weights = new float[] { 0.8f, 0.2f, 0.0f };
        else if (time < 40f)
            weights = new float[] { 0.5f, 0.3f, 0.2f };
        else if (time < 60f)
            weights = new float[] { 0.2f, 0.4f, 0.4f };
        else
            weights = new float[] { 0.05f, 0.25f, 0.7f };

        float rand = Random.value;
        float cumulative = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (rand < cumulative)
                return i;
        }
        return weights.Length - 1; // fallback
    }

    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
    }
}
