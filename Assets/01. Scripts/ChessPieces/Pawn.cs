using UnityEngine;

public class Pawn : ChessPiece
{
    [Header("Pawn Settings")]
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private float _attackInterval = 1f;
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private int _attackRangeLayer = 8; // AttackRange 레이어 번호

    private SphereCollider _attackCollider;
    private float _lastAttackTime;

    protected override void Start()
    {
        // 폰 전용 공격 범위 콜라이더 생성
        GameObject rangeObj = new GameObject("AttackRange");
        rangeObj.transform.SetParent(transform);
        rangeObj.transform.localPosition = Vector3.zero;

        _attackCollider = rangeObj.AddComponent<SphereCollider>();
        _attackCollider.radius = _attackRange;
        _attackCollider.isTrigger = true;

        // 레이어 설정 (본인 + 모든 자식)
        SetLayerRecursively(rangeObj, _attackRangeLayer);

        // 폰 전용 전략 설정
        SetAttackStrategy(new PawnAttackStrategy(
            _attackInterval,
            _attackDamage,
            ProjectilePrefab
        ));
    }

    void Update()
    {
        // 쿨타임 확인 + 범위 내 적 존재 여부 확인
        if (Time.time - _lastAttackTime >= _attackInterval && HasEnemiesInRange())
        {
            PerformAttack();
            _lastAttackTime = Time.time;
        }
    }

    bool HasEnemiesInRange()
    {
        if (_attackStrategy is PawnAttackStrategy pawnStrategy)
        {
            return pawnStrategy.GetDetectedEnemies().Count > 0;
        }
        return false;
    }

    void PerformAttack()
    {
        if (_attackStrategy != null)
        {
            _attackStrategy.Attack(this);
        }
    }

    // 모든 자식 오브젝트 레이어 설정 (재귀 함수)
    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Enemy")) != 0)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                ((PawnAttackStrategy)_attackStrategy).OnEnemyDetected(enemy);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Enemy")) != 0)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                ((PawnAttackStrategy)_attackStrategy).OnEnemyLeft(enemy);
            }
        }
    }
}
