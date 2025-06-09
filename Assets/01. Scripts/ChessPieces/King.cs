using UnityEngine;

public class King : ChessPiece
{
    protected override void Start()
    {
        base.Start();
        BoardManager.Instance.RegisterPiece(this);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (Health <= 0)
        {
            GameManager.Instance.GameOver(false, "King Destroyed!");
            BoardManager.Instance.UnregisterPiece(this);
        }
    }
}
