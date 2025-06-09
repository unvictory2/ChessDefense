using UnityEngine;

public abstract class ChessPiece : MonoBehaviour, IDamageable
{
    [Header("Combat Settings")]
    public int Team;
    public GameObject ProjectilePrefab;
    [SerializeField] private GameObject allyHealthBarPrefab; // �ν����Ϳ��� �Ҵ�
    protected HealthBar _healthBar;

    protected IAttackStrategy _attackStrategy;
    public BoardTile CurrentTile { get; set; }

    // ���� ������Ƽ: �ڽ� Ŭ�������� �������̵�
    public virtual int MaxHealth { get; } = 100;
    public int Health { get; protected set; }

    protected virtual void Start()
    {
        Health = MaxHealth; // �ִ� ü������ �ʱ�ȭ
        // ü�¹� ����
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
            _healthBar.SetHealth(Health, MaxHealth); // �ִ� ü�� ���� �ݿ�
        if (Health <= 0) Destroy(gameObject);
    }
}
