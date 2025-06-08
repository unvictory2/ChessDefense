using UnityEngine;
using System.Collections.Generic;

public class BishopAttackStrategy : IAttackStrategy
{
    private float _range;
    private int _damage;
    private GameObject _projectilePrefab;
    private LayerMask _enemyLayer;

    public BishopAttackStrategy(float range, int damage, GameObject projectile, LayerMask enemyLayer)
    {
        _range = range;
        _damage = damage;
        _projectilePrefab = projectile;
        _enemyLayer = enemyLayer;
    }

    public void Attack(ChessPiece piece)
    {
        if (_projectilePrefab == null) return;

        // 대각선 4방향으로 발사
        Vector3[] directions = {
            new Vector3(1, 0, 1).normalized,   // 우상
            new Vector3(-1, 0, 1).normalized,  // 좌상
            new Vector3(1, 0, -1).normalized,  // 우하
            new Vector3(-1, 0, -1).normalized  // 좌하
        };

        foreach (Vector3 direction in directions)
        {
            FireProjectile(piece.transform.position, direction);
        }
    }

    private void FireProjectile(Vector3 from, Vector3 direction)
    {
        GameObject projectile = Object.Instantiate(
            _projectilePrefab,
            from,
            Quaternion.LookRotation(direction)
        );
        projectile.GetComponent<Projectile>().Initialize(_damage, _enemyLayer, direction);
    }

    public void ShowAttackRange(ChessPiece piece)
    {
        // 대각선 범위 표시 로직
    }
}
