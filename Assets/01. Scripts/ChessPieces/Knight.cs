using UnityEngine;
using System.Collections.Generic;

public class Knight : ChessPiece
{
    [Header("Knight Settings")]
    [SerializeField] private float _detectionRange = 3f;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private float _attackRadius = 1.5f;
    [SerializeField] private int _attackDamage = 15;

    public List<Enemy> DetectedEnemies = new List<Enemy>();
    public bool IsAttacking { get; set; }

    protected override void Start()
    {
        base.Start();

        // 공격 범위 콜라이더 생성 (적 감지용)
        GameObject rangeObj = new GameObject("AttackRange");
        rangeObj.transform.SetParent(transform);
        rangeObj.transform.localPosition = Vector3.zero;
        SphereCollider collider = rangeObj.AddComponent<SphereCollider>();
        collider.radius = _detectionRange;
        collider.isTrigger = true;

        SetAttackStrategy(new KnightAttackStrategy(_attackCooldown));
    }

    void Update()
    {
        if (_attackStrategy != null && !IsAttacking)
        {
            _attackStrategy.Attack(this);
        }
    }

    public Enemy GetNearestEnemy()
    {
        DetectedEnemies.RemoveAll(enemy => enemy == null);
        Enemy nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Enemy enemy in DetectedEnemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }

    public void ApplyAreaDamage(Vector3 center)
    {
        Collider[] hits = Physics.OverlapSphere(center, _attackRadius, LayerMask.GetMask("Enemy"));
        foreach (Collider hit in hits)
        {
            hit.GetComponent<IDamageable>()?.TakeDamage(_attackDamage);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Enemy")) != 0)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !DetectedEnemies.Contains(enemy))
                DetectedEnemies.Add(enemy);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & LayerMask.GetMask("Enemy")) != 0)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            DetectedEnemies.Remove(enemy);
        }
    }
}
