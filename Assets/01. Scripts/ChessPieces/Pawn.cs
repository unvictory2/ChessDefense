using UnityEngine;

public class Pawn : ChessPiece
{
    void Start()
    {
        SetAttackStrategy(new RangedAttack(3, 10, Resources.Load<GameObject>("PF_Arrow")));
    }

    public override void ShowMoveOptions(BoardManager board)
    {
        // ���� �̵� ��Ģ ���� (����: �� 1ĭ)
        int x = CurrentTile.x;
        int y = CurrentTile.y + 1;
        if (y < board.boardSize && board.tiles[x, y].pieceOnTile == null)
        {
            board.tiles[x, y].Highlight(true);
        }
    }
}
