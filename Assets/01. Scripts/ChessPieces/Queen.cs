using UnityEngine;

public class Queen : ChessPiece
{
    [Header("Queen Settings")]
    [SerializeField] private float _attackRange = 6f;
    [SerializeField] private float _attackThickness = 0.6f;
    [SerializeField] private float _attackInterval = 1.8f;
    [SerializeField] private int _attackDamage = 25;
    [SerializeField] private float _attackHeight = 2.2f;

    protected override void Start()
    {
        base.Start();
        SetAttackStrategy(new QueenAttackStrategy(
            _attackRange,
            _attackThickness,
            _attackInterval,
            _attackDamage,
            _attackHeight,
            ProjectilePrefab
        ));
    }

    void Update()
    {
        if (_attackStrategy != null)
        {
            _attackStrategy.Attack(this);
        }
    }
}
