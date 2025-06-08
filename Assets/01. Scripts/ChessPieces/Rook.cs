using UnityEngine;

public class Rook : ChessPiece
{
    void Start()
    {
        SetAttackStrategy(new RangedAttack(5, 25, Resources.Load<GameObject>("PF_CannonBall")));
    }

    public override void ShowMoveOptions(BoardManager board)
    {
        // 룩 직선 이동 구현 (상하좌우)
        int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

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
                        break; // 다른 말이 있으면 더 이상 이동 불가
                }
                else
                    break;
            }
        }
    }
}
