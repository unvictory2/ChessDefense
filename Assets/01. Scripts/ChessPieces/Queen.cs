using UnityEngine;

public class Queen : ChessPiece
{
    void Start()
    {
        SetAttackStrategy(new RangedAttack(6, 35, Resources.Load<GameObject>("PF_MagicBullet")));
    }

    public override void ShowMoveOptions(BoardManager board)
    {
        // Äý 8¹æÇâ ÀÌµ¿ ±¸Çö (·è + ºñ¼ó)
        int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };

        for (int dir = 0; dir < directions.GetLength(0); dir++)
        {
            for (int i = 1; i < board.boardSize; i++)
            {
                int nx = CurrentTile.x + directions[dir, 0] * i;
                int ny = CurrentTile.y + directions[dir, 1] * i;

                if (nx >= 0 && nx < board.boardSize && ny >= 0 && ny < board.boardSize)
                {
                    BoardTile tile = board.tiles[nx, ny];
                    if (tile.pieceOnTile == null)
                        tile.Highlight(true);
                    else
                        break;
                }
                else
                    break;
            }
        }
    }
}
