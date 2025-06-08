public class Pawn : ChessPiece
{
    public override void ShowMoveOptions(BoardManager board)
    {
        // 폰의 이동 규칙 구현 (예시: 앞 1칸)
        int x = CurrentTile.x;
        int y = CurrentTile.y + 1;
        if (y < board.boardSize && board.tiles[x, y].pieceOnTile == null)
        {
            board.tiles[x, y].Highlight(true);
        }
    }
}