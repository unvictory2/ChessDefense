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
        // ŷ ã��
        _king = BoardManager.Instance.mainKing.transform;
        _targetPosition = _king.position;

        // �ִϸ����� ������Ʈ ã��
        _animator = GetComponentInChildren<Animator>();

        // �̵� ����
        _isMoving = true;
        UpdateAnimation();
    }

    void Update()
    {
        if (_isDead) return; // �׾����� �� �̻� ���� X

        _attackTimer += Time.deltaTime;

        if (_king == null) return;

        float distanceToKing = Vector3.Distance(transform.position, _king.position);

        // ���� ���� ���̸� �̵�
        if (distanceToKing > AttackRange)
        {
            MoveTowardsKing();
        }
        else
        {
            // ���� ���� ���̸� ���� �� ����
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

        // ŷ �������� �̵�
        Vector3 direction = (_king.position - transform.position).normalized;
        direction.y = 0; // Y�� ����

        transform.position += direction * MoveSpeed * Time.deltaTime;

        // Y�� ȸ�� (ŷ �ٶ󺸱�)
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
            // �̵� �Ķ���� ���� (Animator Controller�� "IsMoving" bool �Ķ���� �ʿ�)
            _animator.SetBool("IsMoving", _isMoving);

            // �ӵ� �Ķ���� ���� (���û���)
            _animator.SetFloat("Speed", _isMoving ? MoveSpeed : 0f);
        }
    }

    void AttackKing()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Attack"); // "Attack" Ʈ���� �Ķ���� �ʿ�
        }

        // ŷ���� ������
        BoardManager.Instance.mainKing.TakeDamage(AttackDamage);
        Debug.Log($"ŷ ����! ������: {AttackDamage}");
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

        // �ݶ��̴� ��Ȱ��ȭ (�ߺ� �浹 ����)
        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        // �ִϸ����� Death Ʈ���� �ߵ�
        if (_animator != null)
        {
            _animator.SetTrigger("IsDead"); // Animator Controller�� "IsDead" Trigger �Ķ���� �ʿ�
        }

        // �����ʿ� �˸�
        FindObjectOfType<EnemySpawner>()?.OnEnemyDestroyed();

        // ��� ����
        GoldManager.Instance.AddGold(10);

        // �ִϸ��̼� �̺�Ʈ �Ǵ� �ڷ�ƾ���� ������Ʈ ����
        StartCoroutine(DestroyAfterAnimation());
    }

    // �ִϸ��̼� �̺�Ʈ�� ȣ���� ���� ����
    IEnumerator DestroyAfterAnimation()
    {
        // �ִϸ��̼� ���̸�ŭ ��� (��: 1��)
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
