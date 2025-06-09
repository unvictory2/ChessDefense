using UnityEngine;

public class Knight : ChessPiece
{
    [Header("Knight Settings")]
    [SerializeField] private float _detectionRange = 3f; // �� ���� ����
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private int _attackDamage = 15;
    [SerializeField] private int _attackRangeLayer = 8; // ���� ������ AttackRange ���̾�

    [Header("Effects")]
    [SerializeField] private GameObject _attackParticlesPrefab;

    protected override void Start()
    {
        base.Start();
        // ���� ���� �ݶ��̴� ���� (���� 100% ����)
        GameObject rangeObj = new GameObject("AttackRange");
        rangeObj.transform.SetParent(transform);
        rangeObj.transform.localPosition = Vector3.zero;

        SphereCollider collider = rangeObj.AddComponent<SphereCollider>();
        collider.radius = _detectionRange;
        collider.isTrigger = true;

        // ���̾� ���� (���� ����)
        SetLayerRecursively(rangeObj, _attackRangeLayer);

        // ���� ���� (���� ������ �������̽� ���)
        SetAttackStrategy(new KnightAttackStrategy(
            _attackCooldown,
            _attackDamage,
            _attackParticlesPrefab // ��ƼŬ ������ ����
        ));
    }

    void Update()
    {
        // ���� ������ ���� �ֱ� üũ
        if (_attackStrategy != null)
        {
            _attackStrategy.Attack(this);
        }
    }

    // ���̾� ���� ��� �Լ� (���� ����)
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
        // ���� ������ �� ���� ����
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
