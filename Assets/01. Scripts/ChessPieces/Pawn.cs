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
        // 폰 전용 공격 범위 콜라이더 생성
        GameObject rangeObj = new GameObject("AttackRange");
        rangeObj.transform.SetParent(transform);
        rangeObj.transform.localPosition = Vector3.zero;

        _attackCollider = rangeObj.AddComponent<SphereCollider>();
        _attackCollider.radius = _attackRange;
        _attackCollider.isTrigger = true;

        // 폰 전용 전략 설정
        SetAttackStrategy(new PawnAttackStrategy(
            _attackInterval,
            _attackDamage,
            ProjectilePrefab
        ));
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"충돌 감지: {other.name}"); // 1. 충돌 발생 여부 확인
        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Enemy")) != 0)
        {
            Debug.Log($"적 감지: {other.name}"); // 2. 적 레이어 필터링 확인
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                ((PawnAttackStrategy)_attackStrategy).OnEnemyDetected(enemy);
                Debug.Log($"공격 대상 등록: {enemy.name}"); // 3. 적 등록 확인
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
