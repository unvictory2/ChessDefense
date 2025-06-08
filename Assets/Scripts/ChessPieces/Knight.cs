public class Knight : ChessPiece
{
    public override void ShowMoveOptions(BoardManager board)
    {
        // ����Ʈ�� L�� �̵� ����
        int[,] moves = new int[,] { { 2, 1 }, { 1, 2 }, { -1, 2 }, { -2, 1 }, { -2, -1 }, { -1, -2 }, { 1, -2 }, { 2, -1 } };
        int x = CurrentTile.x;
        int y = CurrentTile.y;

        for (int i = 0; i < moves.GetLength(0); i++)
        {
            int nx = x + moves[i, 0];
            int ny = y + moves[i, 1];
            if (nx >= 0 && nx < board.boardSize && ny >= 0 && ny < board.boardSize)
            {
                if (board.tiles[nx, ny].pieceOnTile == null)
                    board.tiles[nx, ny].Highlight(true);
            }
        }
    }
}