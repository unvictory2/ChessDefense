public class Pawn : ChessPiece
{
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