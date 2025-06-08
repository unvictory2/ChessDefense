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
        // 기본 초기화만 수행
    }

    public void SetAttackStrategy(IAttackStrategy strategy) => _attackStrategy = strategy;

    public virtual void PerformAttack() => _attackStrategy?.Attack(this);

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0) Destroy(gameObject);
    }

    // 이동 관련 메서드 모두 삭제됨
}
