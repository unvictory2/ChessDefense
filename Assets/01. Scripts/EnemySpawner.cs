using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    [Header("Spawn Timing")]
    public float baseSpawnInterval = 3f; // 초기 스폰 간격
    public float minSpawnInterval = 1f; // 최소 스폰 간격
    public float spawnIntervalDecrease = 0.5f; // 20초마다 감소량
    public int baseMaxEnemies = 10; // 기본 최대 몹 수
    public float maxEnemiesIncreaseInterval = 20f; // 20초마다 3마리씩 증가

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

    // 20초마다 최대 몬스터 수 3마리씩 증가
    private int GetCurrentMaxEnemies()
    {
        int increments = Mathf.FloorToInt(elapsedTime / maxEnemiesIncreaseInterval);
        return baseMaxEnemies + increments * 3;
    }

    // 20초마다 spawnInterval 감소 (최소값 제한)
    private float GetCurrentSpawnInterval()
    {
        int decrements = Mathf.FloorToInt(elapsedTime / maxEnemiesIncreaseInterval);
        float interval = baseSpawnInterval - decrements * spawnIntervalDecrease;
        return Mathf.Max(minSpawnInterval, interval);
    }

    // 시간에 따른 몬스터 등장 확률
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
