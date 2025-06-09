using UnityEngine;

public abstract class ChessPiece : MonoBehaviour, IDamageable
{
    [Header("Combat Settings")]
    public int Team;
    public GameObject ProjectilePrefab;
    [SerializeField] private GameObject allyHealthBarPrefab; // 인스펙터에서 할당
    protected HealthBar _healthBar;

    protected IAttackStrategy _attackStrategy;
    public BoardTile CurrentTile { get; set; }

    // 가상 프로퍼티: 자식 클래스에서 오버라이드
    public virtual int MaxHealth { get; } = 100;
    public int Health { get; protected set; }

    protected virtual void Start()
    {
        Health = MaxHealth; // 최대 체력으로 초기화
        // 체력바 생성
        if (allyHealthBarPrefab != null)
        {
            GameObject healthBarObj = Instantiate(allyHealthBarPrefab, transform);
            _healthBar = healthBarObj.GetComponent<HealthBar>();
            if (_healthBar != null)
                _healthBar.SetHealth(Health, MaxHealth);
        }
    }

    public void SetAttackStrategy(IAttackStrategy strategy) => _attackStrategy = strategy;

    public virtual void PerformAttack() => _attackStrategy?.Attack(this);

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (_healthBar != null)
            _healthBar.SetHealth(Health, MaxHealth); // 최대 체력 동적 반영
        if (Health <= 0) Destroy(gameObject);
    }
}
