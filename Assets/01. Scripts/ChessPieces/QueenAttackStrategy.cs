using UnityEngine;
using System.Collections.Generic;

public class QueenAttackStrategy : IAttackStrategy
{
    private float _range;
    private int _damage;
    private GameObject _projectilePrefab;
    private LayerMask _enemyLayer;

    public QueenAttackStrategy(float range, int damage, GameObject projectile, LayerMask enemyLayer)
    {
        _range = range;
        _damage = damage;
        _projectilePrefab = projectile;
        _enemyLayer = enemyLayer;
    }

    public void Attack(ChessPiece piece)
    {
        if (_projectilePrefab == null) return;

        // 8�������� �߻� (�����¿� + �밢��)
        Vector3[] directions = {
            new Vector3(0, 0, 1),              // ��
            new Vector3(0, 0, -1),             // ��
            new Vector3(-1, 0, 0),             // ��
            new Vector3(1, 0, 0),              // ��
            new Vector3(1, 0, 1).normalized,   // ���
            new Vector3(-1, 0, 1).normalized,  // �»�
            new Vector3(1, 0, -1).normalized,  // ����
            new Vector3(-1, 0, -1).normalized  // ����
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
        // 8���� ���� ǥ�� ����
    }
}
