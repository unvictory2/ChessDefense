using UnityEngine;

public class Queen : ChessPiece
{
    [Header("Queen Settings")]
    [SerializeField] private float _attackRange = 5f;
    [SerializeField] private int _attackDamage = 20;

    protected override void Start()
    {
        base.Start();
        SetAttackStrategy(new QueenAttackStrategy(
            _attackRange,
            _attackDamage,
            ProjectilePrefab,
            LayerMask.GetMask("Enemy")
        ));
    }
}