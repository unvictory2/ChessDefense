using UnityEngine;
using System.Collections.Generic;

public class RookAttackStrategy : IAttackStrategy
{
    public float AttackRange { get; private set; }
    public float AttackThickness { get; private set; }
    public float AttackHeight { get; private set; }
    private float _attackInterval;
    private int _damage;
    private GameObject _projectilePrefab;
    private float _lastAttackTime;

    public RookAttackStrategy(
        float range,
        float thickness,
        float interval,
        int damage,
        float height,
        GameObject projectile
    )
    {
        AttackRange = range;
        AttackThickness = thickness;
        _attackInterval = interval;
        _damage = damage;
        AttackHeight = height;
        _projectilePrefab = projectile;
    }

    public void Attack(ChessPiece rook)
    {
        if (Time.time - _lastAttackTime < _attackInterval) return;

        Vector3[] directions = {
            new Vector3(0, 0, 1),  // 전방 (Z+)
            new Vector3(0, 0, -1), // 후방 (Z-)
            new Vector3(1, 0, 0),  // 우측 (X+)
            new Vector3(-1, 0, 0)  // 좌측 (X-)
        };

        bool hasEnemies = false;
        Dictionary<Vector3, Enemy> directionEnemies = new Dictionary<Vector3, Enemy>();

        // 1. 4방향 적 감지
        foreach (Vector3 dir in directions)
        {
            List<Enemy> enemiesInDir = DetectEnemiesInDirection(rook.transform.position, dir);
            Enemy nearest = GetNearestEnemy(rook.transform.position, enemiesInDir);
            directionEnemies[dir] = nearest;
            if (nearest != null) hasEnemies = true;
        }

        if (!hasEnemies) return;

        // 2. 모든 방향 공격 실행
        foreach (Vector3 dir in directions)
        {
            Vector3 targetPos = directionEnemies[dir] != null
                ? directionEnemies[dir].transform.position
                : rook.transform.position + dir * AttackRange;

            FireProjectile(rook.transform.position, targetPos);
        }

        _lastAttackTime = Time.time;
    }

    private List<Enemy> DetectEnemiesInDirection(Vector3 origin, Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        Vector3 halfExtents = new Vector3(
            AttackThickness * 0.5f,
            AttackHeight * 0.5f,
            AttackRange * 0.5f
        );
        Vector3 center = origin + direction * AttackRange * 0.5f;

        Collider[] hits = Physics.OverlapBox(
            center,
            halfExtents,
            rotation,
            LayerMask.GetMask("Enemy"),
            QueryTriggerInteraction.Collide
        );

        List<Enemy> enemies = new List<Enemy>();
        foreach (Collider hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null) enemies.Add(enemy);
        }
        return enemies;
    }

    private Enemy GetNearestEnemy(Vector3 origin, List<Enemy> enemies)
    {
        Enemy nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
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

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(_damage, LayerMask.GetMask("Enemy"), direction);
        }
    }
}
