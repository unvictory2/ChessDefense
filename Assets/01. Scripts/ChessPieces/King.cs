using UnityEngine;

public class King : ChessPiece
{
    public override int MaxHealth => 1000;

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
            GameManager.Instance.GameOver(false, "Å· »ç¸Á!");
            BoardManager.Instance.UnregisterPiece(this);
        }
    }
}
