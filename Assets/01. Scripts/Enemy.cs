using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    public int Health; // �ν����Ϳ��� ������ ü�� (���� ü�� + �ִ� ü��)
    public float MoveSpeed;
    public float AttackRange;
    public int AttackDamage;
    public float AttackCooldown;
    public int rewardCoin;

    [Header("Health Settings")]
    [SerializeField] private GameObject enemyHealthBarPrefab; // �� ü�¹� ������
    private HealthBar _healthBar;
    private int _maxHealth; // �ִ� ü�� ����� ���� �߰�

    [Header("Targeting")]
    [SerializeField] private LayerMask towerLayer;
    private Transform _currentTarget;
    private List<ChessPiece> _nearbyTowers = new List<ChessPiece>();

    [Header("Animation")]
    private Animator _animator;
    private bool _isMoving = false;
    private bool _isDead = false;
    private float _attackTimer;

    void Start()
    {
        _maxHealth = Health; // �ִ� ü�� ����
        _healthBar = Instantiate(enemyHealthBarPrefab, transform).GetComponent<HealthBar>();
        _healthBar.SetHealth(Health, _maxHealth); // �ִ� ü�� �ݿ�

        _animator = GetComponentInChildren<Animator>();
        FindNewTarget();
        UpdateAnimation();
    }

    void Update()
    {
        if (_isDead) return;

        _attackTimer += Time.deltaTime;

        // 0.5�ʸ��� Ÿ�� ��Ž�� (60FPS ���� 30������)
        if (Time.frameCount % 30 == 0) FindNewTarget();

        if (_currentTarget == null)
        {
            FindNewTarget();
            return;
        }

        float distance = Vector3.Distance(transform.position, _currentTarget.position);

        if (distance > AttackRange)
        {
            MoveTowardsTarget();
        }
        else
        {
            StopMoving();
            if (_attackTimer >= AttackCooldown)
            {
                AttackTarget();
                _attackTimer = 0f;
            }
        }
    }

    void FindNewTarget()
    {
        // �ֺ� Ÿ�� Ž�� (���� ������ 120% ����)
        Collider[] towers = Physics.OverlapSphere(
            transform.position,
            AttackRange * 1.2f,
            towerLayer,
            QueryTriggerInteraction.Collide // Ʈ���� �ݶ��̴��� ����
        );

        _nearbyTowers.Clear();
        foreach (Collider col in towers)
        {
            ChessPiece tower = col.GetComponent<ChessPiece>();
            if (tower != null) _nearbyTowers.Add(tower);
        }

        // ���� ����� Ÿ�� ����
        ChessPiece closestTower = GetClosestTarget(_nearbyTowers);

        // Ÿ�� ���� �켱����: Ÿ�� > ŷ
        _currentTarget = closestTower != null ?
            closestTower.transform :
            BoardManager.Instance.mainKing.transform;
    }

    ChessPiece GetClosestTarget(List<ChessPiece> targets)
    {
        ChessPiece closest = null;
        float minDistance = Mathf.Infinity;

        foreach (ChessPiece target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = target;
            }
        }
        return closest;
    }

    void MoveTowardsTarget()
    {
        if (!_isMoving)
        {
            _isMoving = true;
            UpdateAnimation();
        }

        Vector3 direction = (_currentTarget.position - transform.position).normalized;
        direction.y = 0;

        transform.position += direction * MoveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 5f
            );
        }
    }
    void StopMoving()
    {
        if (_isMoving)
        {
            _isMoving = false;
            UpdateAnimation();
        }
    }
    void UpdateAnimation()
    {
        if (_animator != null)
        {
            _animator.SetBool("IsMoving", _isMoving);
            _animator.SetFloat("Speed", _isMoving ? MoveSpeed : 0f);
        }
    }
    void AttackTarget()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }

        if (_currentTarget.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(AttackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        _healthBar.SetHealth(Health, _maxHealth); // �ִ� ü������ ����
        if (Health <= 0) Die();
    }

    void Die()
    {
        _isDead = true;


        // �ݶ��̴� ��Ȱ��ȭ
        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        // �ִϸ��̼� Ʈ����
        if (_animator != null)
        {
            _animator.SetTrigger("IsDead");
        }

        // ������ �˸�
        FindObjectOfType<EnemySpawner>()?.OnEnemyDestroyed();

        // ��� ����
        GoldManager.Instance.AddGold(rewardCoin);

        // 1�� �� �ı�
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    // �ִϸ��̼� �̺�Ʈ�� �Լ�
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
