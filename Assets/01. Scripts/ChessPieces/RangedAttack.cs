using UnityEngine;
using System.Collections.Generic;

public class RangedAttack : IAttackStrategy
{
    private float _attackRange;
    private int _damage;
    private GameObject _projectilePrefab;
    private LayerMask _enemyLayer;

    public RangedAttack(float range, int damage, GameObject projectile, LayerMask enemyLayer)
    {
        _attackRange = range;
        _damage = damage;
        _projectilePrefab = projectile;
        _enemyLayer = enemyLayer;
    }

    public void Attack(ChessPiece piece)
    {
        if (_projectilePrefab == null) return;

        BoardTile currentTile = piece.CurrentTile;
        List<Enemy> targets = new List<Enemy>();

        // 8방향 탐색
        for (int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                if (xOffset == 0 && yOffset == 0) continue;

                int x = currentTile.x + xOffset;
                int y = currentTile.y + yOffset;

                if (x < 0 || x >= BoardManager.Instance.boardSize ||
                    y < 0 || y >= BoardManager.Instance.boardSize) continue;

                BoardTile tile = BoardManager.Instance.tiles[x, y];
                Enemy enemy = tile.GetComponentInChildren<Enemy>();
                if (enemy != null) targets.Add(enemy);
            }
        }

        if (targets.Count > 0)
        {
            Enemy nearest = GetNearestEnemy(piece.transform.position, targets);
            Vector3 direction = (nearest.transform.position - piece.transform.position).normalized;

            GameObject projectile = Object.Instantiate(
                _projectilePrefab,
                piece.transform.position,
                Quaternion.LookRotation(direction)
            );
            projectile.GetComponent<Projectile>().Initialize(_damage, _enemyLayer, direction);
        }
    }

    private Enemy GetNearestEnemy(Vector3 origin, List<Enemy> enemies)
    {
        Enemy nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float dist = Vector3.Distance(origin, enemy.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }

    public void ShowAttackRange(ChessPiece piece) { /* 범위 표시 로직 */ }
}
