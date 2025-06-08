using UnityEngine;

public class Knight : ChessPiece
{
    [Header("Knight Settings")]
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private int _attackDamage = 15;

    protected override void Start() // override 유지 (부모에 virtual Start 있음)
    {
        //    base.Start();
        //    SetAttackStrategy(new KnightAttackStrategy(
        //        _attackRange,
        //        _attackDamage,
        //        ProjectilePrefab,
        //        LayerMask.GetMask("Enemy")
        //    ));
        //}
        // ShowMoveOptions 삭제됨
    }
}