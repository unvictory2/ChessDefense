using UnityEngine;

public class King : ChessPiece
{
    protected override void Start()
    {
        base.Start();
        BoardManager.Instance.RegisterPiece(this); // 보드에 등록
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (Health <= 0)
        {
            GameManager.Instance.GameOver("킹이 파괴되었습니다!");
            BoardManager.Instance.UnregisterPiece(this);
        }
    }
}
