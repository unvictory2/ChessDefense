using UnityEngine;

public class Knight : ChessPiece
{
    [Header("Knight Settings")]
    [SerializeField] private float _detectionRange = 3f; // 적 감지 범위
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private int _attackDamage = 15;
    [SerializeField] private int _attackRangeLayer = 8; // 폰과 동일한 AttackRange 레이어

    [Header("Effects")]
    [SerializeField] private GameObject _attackParticlesPrefab;

    protected override void Start()
    {
        base.Start();
        // 공격 범위 콜라이더 생성 (폰과 100% 동일)
        GameObject rangeObj = new GameObject("AttackRange");
        rangeObj.transform.SetParent(transform);
        rangeObj.transform.localPosition = Vector3.zero;

        SphereCollider collider = rangeObj.AddComponent<SphereCollider>();
        collider.radius = _detectionRange;
        collider.isTrigger = true;

        // 레이어 설정 (폰과 동일)
        SetLayerRecursively(rangeObj, _attackRangeLayer);

        // 전략 설정 (폰과 동일한 인터페이스 사용)
        SetAttackStrategy(new KnightAttackStrategy(
            _attackCooldown,
            _attackDamage,
            _attackParticlesPrefab // 파티클 프리팹 전달
        ));
    }

    void Update()
    {
        // 폰과 동일한 공격 주기 체크
        if (_attackStrategy != null)
        {
            _attackStrategy.Attack(this);
        }
    }

    // 레이어 설정 재귀 함수 (폰과 동일)
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
        // 폰과 동일한 적 감지 로직
        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Enemy")) != 0)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                ((KnightAttackStrategy)_attackStrategy).OnEnemyDetected(enemy);
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
                ((KnightAttackStrategy)_attackStrategy).OnEnemyLeft(enemy);
            }
        }
    }
}
