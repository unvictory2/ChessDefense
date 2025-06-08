using UnityEngine;

public class King : ChessPiece
{
    protected override void Start()
    {
        base.Start();
        BoardManager.Instance.RegisterPiece(this); // ���忡 ���
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (Health <= 0)
        {
            GameManager.Instance.GameOver("ŷ�� �ı��Ǿ����ϴ�!");
            BoardManager.Instance.UnregisterPiece(this);
        }
    }
}
