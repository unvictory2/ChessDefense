// Pawn.cs
using UnityEngine;

public class Pawn : ChessPiece
{
    [Header("Pawn Settings")]
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private float _attackInterval = 1f;
    [SerializeField] private int _attackDamage = 10;

    private SphereCollider _attackCollider;

    protected override void Start()
    {
        // �� ���� ���� ���� �ݶ��̴� ����
        GameObject rangeObj = new GameObject("AttackRange");
        rangeObj.transform.SetParent(transform);
        rangeObj.transform.localPosition = Vector3.zero;

        _attackCollider = rangeObj.AddComponent<SphereCollider>();
        _attackCollider.radius = _attackRange;
        _attackCollider.isTrigger = true;

        // �� ���� ���� ����
        SetAttackStrategy(new PawnAttackStrategy(
            _attackInterval,
            _attackDamage,
            ProjectilePrefab
        ));
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"�浹 ����: {other.name}"); // 1. �浹 �߻� ���� Ȯ��
        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Enemy")) != 0)
        {
            Debug.Log($"�� ����: {other.name}"); // 2. �� ���̾� ���͸� Ȯ��
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                ((PawnAttackStrategy)_attackStrategy).OnEnemyDetected(enemy);
                Debug.Log($"���� ��� ���: {enemy.name}"); // 3. �� ��� Ȯ��
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Enemy")) != 0)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
                ((PawnAttackStrategy)_attackStrategy).OnEnemyLeft(enemy);
        }
    }
}
