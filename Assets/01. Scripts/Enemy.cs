using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    public int Health = 100;
    public float MoveSpeed = 2f;
    public float AttackRange = 1.5f;
    public int AttackDamage = 10;
    public float AttackCooldown = 1f;

    private float _attackTimer;
    protected Transform _target;

    void Update()
    {
        _attackTimer += Time.deltaTime;

        if (_target == null) FindTarget();
        else MoveTowardsTarget();

        if (_attackTimer >= AttackCooldown && _target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _target.position);
            if (distanceToTarget <= AttackRange)
            {
                Attack();
                _attackTimer = 0f;
            }
        }
    }

    protected virtual void FindTarget()
    {
        // ���� ����� Ÿ�� Ž��
        Collider[] towers = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Tower"));
        if (towers.Length > 0)
        {
            _target = towers[0].transform;
        }
    }

    protected virtual void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, MoveSpeed * Time.deltaTime);
    }

    protected virtual void Attack()
    {
        // ���� ���� �ִϸ��̼� ���
        ChessPiece targetPiece = _target.GetComponent<ChessPiece>();
        if (targetPiece != null)
        {
            targetPiece.TakeDamage(AttackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            // �� óġ �� ��� ����
            GoldManager.Instance.AddGold(10);
            Destroy(gameObject);
        }
    }
}
