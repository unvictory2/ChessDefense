using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    public int enemiesPerWave = 5;
    private float _timer;
    private int _spawned;

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= spawnInterval && _spawned < enemiesPerWave)
        {
            SpawnEnemy();
            _timer = 0f;
            _spawned++;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = new Vector3(Random.Range(0, 8), 0.5f, 0);
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, DynamicObjects.Enemies);
    }
}
