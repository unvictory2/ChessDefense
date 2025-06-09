using UnityEngine;

public class Pawn : ChessPiece
{
    [Header("Pawn Settings")]
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private float _attackInterval = 1f;
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private int _attackRangeLayer = 8; // AttackRange ���̾� ��ȣ

    private SphereCollider _attackCollider;
    private float _lastAttackTime;

    protected override void Start()
    {
        // �� ���� ���� ���� �ݶ��̴� ����
        GameObject rangeObj = new GameObject("AttackRange");
        rangeObj.transform.SetParent(transform);
        rangeObj.transform.localPosition = Vector3.zero;

        _attackCollider = rangeObj.AddComponent<SphereCollider>();
        _attackCollider.radius = _attackRange;
        _attackCollider.isTrigger = true;

        // ���̾� ���� (���� + ��� �ڽ�)
        SetLayerRecursively(rangeObj, _attackRangeLayer);

        // �� ���� ���� ����
        SetAttackStrategy(new PawnAttackStrategy(
            _attackInterval,
            _attackDamage,
            ProjectilePrefab
        ));
    }

    void Update()
    {
        // ��Ÿ�� Ȯ�� + ���� �� �� ���� ���� Ȯ��
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

    // ��� �ڽ� ������Ʈ ���̾� ���� (��� �Լ�)
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
