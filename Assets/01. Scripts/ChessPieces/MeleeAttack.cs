using UnityEngine;

public class MeleeAttack : IAttackStrategy
{
    private float _attackRange;
    private int _damage;

    public MeleeAttack(float range, int damage)
    {
        _attackRange = range;
        _damage = damage;
    }

    public void Attack(ChessPiece piece)
    {
        Collider[] enemies = Physics.OverlapSphere(piece.transform.position, _attackRange, LayerMask.GetMask("Enemy"));
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<IDamageable>()?.TakeDamage(_damage);
        }
    }

    public void ShowAttackRange(ChessPiece piece)
    {
        int[,] moves = new int[,] { { 2, 1 }, { 1, 2 }, { -1, 2 }, { -2, 1 }, { -2, -1 }, { -1, -2 }, { 1, -2 }, { 2, -1 } };
        int x = piece.CurrentTile.x;
        int y = piece.CurrentTile.y;

        for (int i = 0; i < moves.GetLength(0); i++)
        {
            int nx = x + moves[i, 0];
            int ny = y + moves[i, 1];
            if (nx >= 0 && nx < BoardManager.Instance.boardSize && ny >= 0 && ny < BoardManager.Instance.boardSize)
            {
                BoardTile tile = BoardManager.Instance.tiles[nx, ny];
                if (tile.pieceOnTile == null || tile.pieceOnTile is Enemy)
                    tile.Highlight(true);
            }
        }
    }

    public float GetAttackRange() => _attackRange;
    public float GetAttackRate() => 1.5f; // 예시 값, 필요시 조정
}
