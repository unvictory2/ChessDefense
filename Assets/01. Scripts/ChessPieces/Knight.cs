using UnityEngine;

public class Knight : ChessPiece
{
    [Header("Knight Settings")]
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private int _attackDamage = 15;

    protected override void Start() // override ���� (�θ� virtual Start ����)
    {
        //    base.Start();
        //    SetAttackStrategy(new KnightAttackStrategy(
        //        _attackRange,
        //        _attackDamage,
        //        ProjectilePrefab,
        //        LayerMask.GetMask("Enemy")
        //    ));
        //}
        // ShowMoveOptions ������
    }
}