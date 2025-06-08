using UnityEngine;
using System.Collections.Generic;

public class PawnAttackStrategy : IAttackStrategy
{
    private float _attackInterval;
    private int _damage;
    private GameObject _projectilePrefab;
    private List<Enemy> _detectedEnemies = new List<Enemy>();
    private float _lastAttackTime;

    public PawnAttackStrategy(float interval, int damage, GameObject projectile)
    {
        _attackInterval = interval;
        _damage = damage;
        _projectilePrefab = projectile;
    }

    public void Attack(ChessPiece pawn)
    {
        // 파괴된 적 제거
        _detectedEnemies.RemoveAll(enemy => enemy == null);

        Debug.Log($"공격 시도: {Time.time}");
        if (Time.time - _lastAttackTime < _attackInterval) return;
        Debug.Log($"쿨타임 조건 충족");

        if (_detectedEnemies.Count == 0)
        {
            Debug.Log("감지된 적 없음");
            return;
        }

        Enemy nearest = GetNearestEnemy(pawn.transform.position);
        if (nearest == null) return;

        FireProjectile(pawn.transform.position, nearest.transform.position);
        _lastAttackTime = Time.time;
    }

    // 적 목록 반환 메서드
    public List<Enemy> GetDetectedEnemies() => _detectedEnemies;

    // 폰 전용 충돌 감지 메서드
    public void OnEnemyDetected(Enemy enemy) => _detectedEnemies.Add(enemy);
    public void OnEnemyLeft(Enemy enemy) => _detectedEnemies.Remove(enemy);

    private Enemy GetNearestEnemy(Vector3 origin)
    {
        Enemy nearest = null;
        float minDistance = Mathf.Infinity;

        // 파괴된 적 제거
        _detectedEnemies.RemoveAll(enemy => enemy == null || enemy.Equals(null));

        foreach (Enemy enemy in _detectedEnemies)
        {
            if (enemy == null) continue; // 추가 안전 장치

            float distance = Vector3.Distance(origin, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }
        return nearest;
    }

    private void FireProjectile(Vector3 from, Vector3 to)
    {
        Vector3 direction = (to - from).normalized;
        GameObject projectile = Object.Instantiate(
            _projectilePrefab,
            from,
            Quaternion.LookRotation(direction)
        );
        projectile.GetComponent<Projectile>().Initialize(_damage, LayerMask.GetMask("Enemy"), direction);
    }
}
