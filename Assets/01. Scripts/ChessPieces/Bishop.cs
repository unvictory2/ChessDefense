using UnityEngine;

public class Bishop : ChessPiece
{
    [Header("Bishop Settings")]
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private int _attackDamage = 15;

    protected override void Start()
    {
        base.Start();
        SetAttackStrategy(new BishopAttackStrategy(
            _attackRange,
            _attackDamage,
            ProjectilePrefab,
            LayerMask.GetMask("Enemy")
        ));
    }
}
