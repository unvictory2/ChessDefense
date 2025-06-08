using UnityEngine;

public class Bishop : ChessPiece
{
    void Start()
    {
        SetAttackStrategy(new RangedAttack(4, 15, Resources.Load<GameObject>("PF_MagicBullet")));
    }

    public override void ShowMoveOptions(BoardManager board)
    {
        // 비숍 대각선 이동 구현
        int[] directions = { 1, -1 };

        foreach (int dx in directions)
        {
            foreach (int dy in directions)
            {
                for (int i = 1; i < board.boardSize; i++)
                {
                    int nx = CurrentTile.x + dx * i;
                    int ny = CurrentTile.y + dy * i;

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
}
