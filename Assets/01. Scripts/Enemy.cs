using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    public int Health = 100;
    public float MoveSpeed = 2f;
    public float AttackRange = 1.5f;
    public int AttackDamage = 10;
    public float AttackCooldown = 1f;

    [Header("Animation")]
    private Animator _animator;
    private bool _isMoving = false;
    private bool _isDead = false;

    private float _attackTimer;
    private Transform _king;
    private Vector3 _targetPosition;

    void Start()
    {
        // 킹 찾기
        _king = BoardManager.Instance.mainKing.transform;
        _targetPosition = _king.position;

        // 애니메이터 컴포넌트 찾기
        _animator = GetComponentInChildren<Animator>();

        // 이동 시작
        _isMoving = true;
        UpdateAnimation();
    }

    void Update()
    {
        if (_isDead) return; // 죽었으면 더 이상 동작 X

        _attackTimer += Time.deltaTime;

        if (_king == null) return;

        float distanceToKing = Vector3.Distance(transform.position, _king.position);

        // 공격 범위 밖이면 이동
        if (distanceToKing > AttackRange)
        {
            MoveTowardsKing();
        }
        else
        {
            // 공격 범위 안이면 정지 후 공격
            StopMoving();
            if (_attackTimer >= AttackCooldown)
            {
                AttackKing();
                _attackTimer = 0f;
            }
        }
    }

    void MoveTowardsKing()
    {
        if (!_isMoving)
        {
            _isMoving = true;
            UpdateAnimation();
        }

        // 킹 방향으로 이동
        Vector3 direction = (_king.position - transform.position).normalized;
        direction.y = 0; // Y축 고정

        transform.position += direction * MoveSpeed * Time.deltaTime;

        // Y축 회전 (킹 바라보기)
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
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
            // 이동 파라미터 설정 (Animator Controller에 "IsMoving" bool 파라미터 필요)
            _animator.SetBool("IsMoving", _isMoving);

            // 속도 파라미터 설정 (선택사항)
            _animator.SetFloat("Speed", _isMoving ? MoveSpeed : 0f);
        }
    }

    void AttackKing()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Attack"); // "Attack" 트리거 파라미터 필요
        }

        // 킹에게 데미지
        BoardManager.Instance.mainKing.TakeDamage(AttackDamage);
        Debug.Log($"킹 공격! 데미지: {AttackDamage}");
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        _isDead = true;

        // 콜라이더 비활성화 (중복 충돌 방지)
        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        // 애니메이터 Death 트리거 발동
        if (_animator != null)
        {
            _animator.SetTrigger("IsDead"); // Animator Controller에 "IsDead" Trigger 파라미터 필요
        }

        // 스포너에 알림
        FindObjectOfType<EnemySpawner>()?.OnEnemyDestroyed();

        // 골드 지급
        GoldManager.Instance.AddGold(10);

        // 애니메이션 이벤트 또는 코루틴으로 오브젝트 삭제
        StartCoroutine(DestroyAfterAnimation());
    }

    // 애니메이션 이벤트로 호출할 수도 있음
    IEnumerator DestroyAfterAnimation()
    {
        // 애니메이션 길이만큼 대기 (예: 1초)
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
