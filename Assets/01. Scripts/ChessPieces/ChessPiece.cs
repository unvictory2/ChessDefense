using UnityEngine;

public abstract class ChessPiece : MonoBehaviour, IDamageable
{
    [Header("Combat Settings")]
    public int Team;
    public int Health = 100;
    public GameObject ProjectilePrefab;
    [SerializeField] private GameObject allyHealthBarPrefab; // 인스펙터에서 아군 체력바 프리팹 할당
    protected HealthBar _healthBar;

    protected IAttackStrategy _attackStrategy;
    public BoardTile CurrentTile { get; set; }

    protected virtual void Start()
    {
        // 체력바 생성
        if (allyHealthBarPrefab != null)
        {
            GameObject healthBarObj = Instantiate(allyHealthBarPrefab, transform);
            _healthBar = healthBarObj.GetComponent<HealthBar>();
            if (_healthBar != null)
                _healthBar.SetHealth(Health, Health);
        }
    }

    public void SetAttackStrategy(IAttackStrategy strategy) => _attackStrategy = strategy;

    public virtual void PerformAttack() => _attackStrategy?.Attack(this);

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (_healthBar != null)
            _healthBar.SetHealth(Health, 100); // 최대 체력 100으로 가정
        if (Health <= 0) Destroy(gameObject);
    }
}
