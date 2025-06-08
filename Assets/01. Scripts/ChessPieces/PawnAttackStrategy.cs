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
        // �ı��� �� ����
        _detectedEnemies.RemoveAll(enemy => enemy == null);

        Debug.Log($"���� �õ�: {Time.time}");
        if (Time.time - _lastAttackTime < _attackInterval) return;
        Debug.Log($"��Ÿ�� ���� ����");

        if (_detectedEnemies.Count == 0)
        {
            Debug.Log("������ �� ����");
            return;
        }

        Enemy nearest = GetNearestEnemy(pawn.transform.position);
        if (nearest == null) return;

        FireProjectile(pawn.transform.position, nearest.transform.position);
        _lastAttackTime = Time.time;
    }

    // �� ��� ��ȯ �޼���
    public List<Enemy> GetDetectedEnemies() => _detectedEnemies;

    // �� ���� �浹 ���� �޼���
    public void OnEnemyDetected(Enemy enemy) => _detectedEnemies.Add(enemy);
    public void OnEnemyLeft(Enemy enemy) => _detectedEnemies.Remove(enemy);

    private Enemy GetNearestEnemy(Vector3 origin)
    {
        Enemy nearest = null;
        float minDistance = Mathf.Infinity;

        // �ı��� �� ����
        _detectedEnemies.RemoveAll(enemy => enemy == null || enemy.Equals(null));

        foreach (Enemy enemy in _detectedEnemies)
        {
            if (enemy == null) continue; // �߰� ���� ��ġ

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
