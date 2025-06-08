using UnityEngine;

public abstract class ChessPiece : MonoBehaviour, IDamageable
{
    [Header("Combat Settings")]
    public int Team;
    public int Health = 100;
    public GameObject ProjectilePrefab;

    protected IAttackStrategy _attackStrategy;
    public BoardTile CurrentTile { get; set; }

    protected virtual void Start()
    {
        // �⺻ �ʱ�ȭ�� ����
    }

    public void SetAttackStrategy(IAttackStrategy strategy) => _attackStrategy = strategy;

    public virtual void PerformAttack() => _attackStrategy?.Attack(this);

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0) Destroy(gameObject);
    }

    // �̵� ���� �޼��� ��� ������
}
