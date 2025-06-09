using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    public int Health; // 인스펙터에서 설정된 체력 (현재 체력 + 최대 체력)
    public float MoveSpeed;
    public float AttackRange;
    public int AttackDamage;
    public float AttackCooldown;
    public int rewardCoin;

    [Header("Health Settings")]
    [SerializeField] private GameObject enemyHealthBarPrefab; // 적 체력바 프리팹
    private HealthBar _healthBar;
    private int _maxHealth; // 최대 체력 저장용 변수 추가

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
        _maxHealth = Health; // 최대 체력 저장
        _healthBar = Instantiate(enemyHealthBarPrefab, transform).GetComponent<HealthBar>();
        _healthBar.SetHealth(Health, _maxHealth); // 최대 체력 반영

        _animator = GetComponentInChildren<Animator>();
        FindNewTarget();
        UpdateAnimation();
    }

    void Update()
    {
        if (_isDead) return;

        _attackTimer += Time.deltaTime;

        // 0.5초마다 타겟 재탐색 (60FPS 기준 30프레임)
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
        // 주변 타워 탐지 (공격 범위의 120% 영역)
        Collider[] towers = Physics.OverlapSphere(
            transform.position,
            AttackRange * 1.2f,
            towerLayer,
            QueryTriggerInteraction.Collide // 트리거 콜라이더도 감지
        );

        _nearbyTowers.Clear();
        foreach (Collider col in towers)
        {
            ChessPiece tower = col.GetComponent<ChessPiece>();
            if (tower != null) _nearbyTowers.Add(tower);
        }

        // 가장 가까운 타워 선택
        ChessPiece closestTower = GetClosestTarget(_nearbyTowers);

        // 타겟 결정 우선순위: 타워 > 킹
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
        _healthBar.SetHealth(Health, _maxHealth); // 최대 체력으로 변경
        if (Health <= 0) Die();
    }

    void Die()
    {
        _isDead = true;


        // 콜라이더 비활성화
        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        // 애니메이션 트리거
        if (_animator != null)
        {
            _animator.SetTrigger("IsDead");
        }

        // 스포너 알림
        FindObjectOfType<EnemySpawner>()?.OnEnemyDestroyed();

        // 골드 지급
        GoldManager.Instance.AddGold(rewardCoin);

        // 1초 후 파괴
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    // 애니메이션 이벤트용 함수
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
