using UnityEngine;

public class Rook : ChessPiece
{
    [Header("Rook Settings")]
    [SerializeField] private float _attackRange = 7f;
    [SerializeField] private float _attackThickness = 0.5f;
    [SerializeField] private float _attackInterval = 2f;
    [SerializeField] private int _attackDamage = 20;
    [SerializeField] private float _attackHeight = 2f;

    protected override void Start()
    {
        base.Start();
        SetAttackStrategy(new RookAttackStrategy(
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
