using UnityEngine;

public class Rook : ChessPiece
{
    [Header("Rook Settings")]
    [SerializeField] private float _attackRange = 4f;
    [SerializeField] private int _attackDamage = 18;

    protected override void Start() // override Ű���� �߰�
    {
        //base.Start();
        //SetAttackStrategy(new RookAttackStrategy(
        //    _attackRange,
        //    _attackDamage,
        //    ProjectilePrefab,
        //    LayerMask.GetMask("Enemy")
        //));
    }
    // ShowMoveOptions ������
}